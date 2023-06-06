using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Gaos.Websocket
{

    public class WebSocketClientSharp:  Gaos.Websocket.IWebSocketClient
    {
        public readonly static string CLASS_NAME = typeof(WebSocketClientSharp).Name;

        public Queue<string> MessagesOutbound = new Queue<string>();
        public Queue<string> MessagesInbound = new Queue<string>();

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

        public Queue<string> GetMessagesOutbound()
        {
            return MessagesOutbound;
        }
        public Queue<string> GetMessagesInbound()
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
                    Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: websocket message: {e.Data}");
                    MessagesInbound.Enqueue(e.Data);
                }
                else
                {
                    Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: ERROR: message format is not text, message ignored");

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

                WebSocket.Connect();
            };

            WebSocket.Connect();

        }

        public void Send(string data)
        {
            MessagesOutbound.Enqueue(data);
        }

        public IEnumerator StartProcessing()
        {
            const string METHOD_NAME = "StartProcessing()";
            const int MAX_RETRY_COUNT = 5;

            Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: websocket start processing");

            int retryCount = 0;

            while (true)
            {
                if (IsConnected)
                {
                    if (MessagesOutbound.Count > 0)
                    {
                        string message = MessagesOutbound.Peek();
                        Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: websocket sending message: {message}");
                        try
                        {
                            WebSocket.Send(message);
                            MessagesOutbound.Dequeue();
                            retryCount = 0;
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error sending message: {message}: {e.Message}");
                            ++retryCount;
                            if (retryCount > MAX_RETRY_COUNT)
                            {
                                Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: sending message: {message}: MAX_RETRY_COUNT reached, will not try again");
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

    }

}