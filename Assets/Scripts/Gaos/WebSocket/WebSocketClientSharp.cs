using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Gaos.WebSocket
{

    public class WebSocketClientSharp:  MonoBehaviour, Gaos.WebSocket.IWebSocketClient
    {
        public readonly static string CLASS_NAME = typeof(WebSocketClientSharp).Name;

        public Queue<byte[]> MessagesOutbound = new Queue<byte[]>();
        public Queue<byte[]> MessagesInbound = new Queue<byte[]>();

        public WebSocketSharp.WebSocket WebSocket;

        public string WsUrl = Gaos.Environment.Environment.GetEnvironment()["GAOS_WS"];

        private bool IsAuthenticated = false;


        private enum SslProtocolsHack
        {
            Tls = 192,
            Tls11 = 768,
            Tls12 = 3072,
            Tls13 = 12288,

        }

        public Queue<byte[]> GetOutboundQueue()
        {
            return MessagesOutbound;
        }
        public Queue<byte[]> GetInboundQueue()
        {
            return MessagesInbound;
        }

        public void Open()
        {
            const string METHOD_NAME = "Open()"; 
            Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: webcocket - openning: {WsUrl}");

            WebSocket = new WebSocketSharp.WebSocket(WsUrl);

            var sslProtocolHack = (System.Security.Authentication.SslProtocols)(SslProtocolsHack.Tls13 | SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
            WebSocket.SslConfiguration.EnabledSslProtocols = sslProtocolHack;


            IsAuthenticated = false;
            WebSocket.OnOpen += (sender, e) =>
            {
                Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: webcocket connected");
                if (Gaos.Context.Authentication.GetJWT() != null)
                {
                    Gaos.Messages.WsAuthentication.authenticate(this, Gaos.Context.Authentication.GetJWT());
                }
            };

            WebSocket.OnMessage += (sender, e) =>
            {
                MessagesInbound.Enqueue(e.RawData);
            };

            WebSocket.OnError += (sender, e) =>
            {
                Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error occured: {e.Message}");
            };

            WebSocket.OnClose += (sender, e) =>
            {
                Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: websocket closed");
            };

            WebSocket.Connect();

        }

        public void Send(byte[] data)
        {
            MessagesOutbound.Enqueue(data);
        }

        public virtual void Process(byte[] data)
        {
        }

        public IEnumerator StartProcessingOutboundQueue()
        {
            const string METHOD_NAME = "StartProcessing()";
            const int MAX_RETRY_COUNT = 5;

            int retryCount = 0;

            while (true)
            {
                var state = WebSocket.ReadyState;

                if (state == WebSocketSharp.WebSocketState.Open)
                {
                    if (MessagesOutbound.Count > 0)
                    {
                        byte[] data = MessagesOutbound.Peek();
                        try
                        {
                            WebSocket.Send(data);
                            MessagesOutbound.Dequeue();
                            retryCount = 0;
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error sending message: {data}: {e.Message}");
                            ++retryCount;
                            if (retryCount > MAX_RETRY_COUNT)
                            {
                                Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: sending message: {data}: MAX_RETRY_COUNT reached, will not try again");
                                MessagesOutbound.Dequeue();
                                retryCount = 0;
                            }
                        }
                        yield return null;
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else
                {
                    if (state == WebSocketSharp.WebSocketState.Closed)
                    {
                        Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: websocket is closed, will try to open it again");
                        yield return new WaitForSeconds(2);
                        Open();
                    }
                    else if (state == WebSocketSharp.WebSocketState.Connecting)
                    {
                        yield return new WaitForSeconds(0.5f);
                    } 
                    if (state == WebSocketSharp.WebSocketState.Closing)
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    if (state == WebSocketSharp.WebSocketState.New)
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    else
                    {
                        Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: websocket state is unknown: {state}");
                        yield return new WaitForSeconds(0.5f);
                    }
                }
            }
        }

        public IEnumerator StartProcessingInboundQueue(WebSocketClient ws)
        {
            const string METHOD_NAME = "StartProcessingInboundQueue()";
            while (true)
            {
                var state = WebSocket.ReadyState;
                if (state == WebSocketSharp.WebSocketState.Open)
                {
                    if (MessagesInbound.Count > 0)
                    {
                        byte[] data = MessagesInbound.Peek();
                        try
                        {
                            ws.Process(data);
                            MessagesInbound.Dequeue();
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error processing message: {e}");
                            MessagesInbound.Dequeue();
                        }
                        yield return null;
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        public bool GetIsAuthenticated()
        {
            return IsAuthenticated;
        }

        public void SetIsAuthenticated(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }
    }


}