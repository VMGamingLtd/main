using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent;

namespace Gaos.WebSocket
{

    public class WebSocketClientSharp:  MonoBehaviour, Gaos.WebSocket.IWebSocketClient
    {
        public readonly static string CLASS_NAME = typeof(WebSocketClientSharp).Name;

        public ConcurrentQueue<byte[]> MessagesOutbound = new ConcurrentQueue<byte[]>();
        public ConcurrentQueue<byte[]> MessagesInbound = new ConcurrentQueue<byte[]>();

        public WebSocketSharp.WebSocket WebSocket;

        public string WsUrl = Gaos.Environment.Environment.GetEnvironment()["GAOS_WS"];

        private bool IsAuthenticated = false;

        CancellationTokenSource threadCancelationToken;
        Thread thread;


        private enum SslProtocolsHack
        {
            Tls = 192,
            Tls11 = 768,
            Tls12 = 3072,
            Tls13 = 12288,

        }

        public ConcurrentQueue<byte[]> GetOutboundQueue()
        {
            return MessagesOutbound;
        }
        public ConcurrentQueue<byte[]> GetInboundQueue()
        {
            return MessagesInbound;
        }

        public void OpenWs()
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

        public void Open()
        {
            threadCancelationToken = new CancellationTokenSource();
            thread = new Thread(() => {
                OpenWs();
                Thread.Sleep(2000);
                StartProcessingOutboundQueue_inThread(threadCancelationToken.Token);
            });

            thread.Start();
        }
        
        void OnDisable()
        {
            threadCancelationToken.Cancel();
        }


        public void Send(byte[] data)
        {
            MessagesOutbound.Enqueue(data);
        }

        public virtual void Process(byte[] data)
        {
        }

        public void StartProcessingOutboundQueue_inThread(CancellationToken token)
        {
            const string METHOD_NAME = "StartProcessingOutboundQueue()";

            while (!token.IsCancellationRequested)
            {
                var state = WebSocket.ReadyState;

                if (state == WebSocketSharp.WebSocketState.Open)
                {
                    if (MessagesOutbound.Count > 0)
                    {
                        byte[] data; 
                        bool dataReturned = MessagesOutbound.TryDequeue(out data);
                        if (dataReturned)
                        {
                            try
                            {
                                WebSocket.Send(data);
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error sending message: {data}: {e.Message}");
                            }
                        }
                        else
                        {
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
                else
                {
                    if (state == WebSocketSharp.WebSocketState.Closed)
                    {
                        if (!token.IsCancellationRequested)
                        {
                            Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: websocket is closed, will try to open it again");
                            Thread.Sleep(2000);
                            OpenWs();
                        }
                    }
                    else if (state == WebSocketSharp.WebSocketState.Connecting)
                    {
                        Thread.Sleep(500);
                    } 
                    if (state == WebSocketSharp.WebSocketState.Closing)
                    {
                        Thread.Sleep(500);
                    }
                    if (state == WebSocketSharp.WebSocketState.New)
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: websocket state is unknown: {state}");
                        Thread.Sleep(500);
                    }
                }
            }
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: thread finished");
        }

        public IEnumerator StartProcessingOutboundQueue()
        {
            yield return null;
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
                        byte[] data;
                        bool dataReturned = MessagesInbound.TryDequeue(out data);

                        if (dataReturned)
                        {
                            try
                            {
                                ws.Process(data);
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error processing message: {e}");
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