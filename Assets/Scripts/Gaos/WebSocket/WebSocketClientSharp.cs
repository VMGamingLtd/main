using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Gaos.WebSocket
{

    public class WebSocketClientSharp:  MonoBehaviour, Gaos.WebSocket.IWebSocketClient
    {
        public readonly static string CLASS_NAME = typeof(WebSocketClientSharp).Name;

        public Queue<byte[]> MessagesOutbound = new Queue<byte[]>();
        public Queue<byte[]> MessagesInbound = new Queue<byte[]>();

        public WebSocketSharp.WebSocket WebSocket;
        public bool IsConnected = false;

        public string WsUrl = Gaos.Environment.Environment.GetEnvironment()["GAOS_WS"];


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


            WebSocket.OnOpen += (sender, e) =>
            {
                Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: webcocket connected");
                IsConnected = true;
            };

            WebSocket.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: ERROR: message format is not binary, message ignored");
                }
                else
                {
                    MessagesInbound.Enqueue(e.RawData);
                }

            };

            WebSocket.OnError += (sender, e) =>
            {
                Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error occured: {e.Message}");
            };

            WebSocket.OnClose += (sender, e) =>
            {
                Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: websocket closed");
                IsConnected = false;

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
                if (IsConnected)
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
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else
                {
                    yield return new WaitForSeconds(2);
                    Open();
                }
            }
        }

        public IEnumerator StartProcessingInboundQueue(IWebSocketClientHandler handler)
        {
            const string METHOD_NAME = "StartProcessingInboundQueue()";
            while (true)
            {
                if (IsConnected)
                {
                    if (MessagesInbound.Count > 0)
                    {
                        byte[] data = MessagesInbound.Peek();
                        try
                        {
                            handler.Process(data);
                            MessagesOutbound.Dequeue();
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error processing message: {e.Message}");
                        }
                        yield return null;
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

}