using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gaos.WebSocket
{
    public class WebSocketClientJs: MonoBehaviour, Gaos.WebSocket.IWebSocketClient
    {
        public readonly static string CLASS_NAME = typeof(WebSocketClientJs).Name;

        private Queue<string> MessagesOutbound = new Queue<string>();
        private Queue<string> MessagesInbound = new Queue<string>();

        private int Ws;
        private bool IsConnected = false;

        [DllImport("__Internal")]
        private static extern int WebSocketCreate(string url);

        [DllImport("__Internal")]
        private static extern void WebSocketOnOpen(int ws, string fnName);

        [DllImport("__Internal")]
        private static extern void WebSocketOnClose(int ws, string fnName);

        [DllImport("__Internal")]
        private static extern void WebSocketOnError(int ws, string fnName);

        [DllImport("__Internal")]
        private static extern void WebSocketOnMessage(int ws, string fnName);

        [DllImport("__Internal")]
        private static extern void WebSocketSend(int ws, string data);

        public void OnOpen()
        {
            IsConnected = true;
        }

        public void OnClose()
        {
            IsConnected = false;
        }

        public void OnError(string errorStr)
        {
            const string METHOD_NAME = "OnError()";
            Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {errorStr}");
        }

        public void OnMessage(string data)
        {
            MessagesInbound.Enqueue(data);
        }

        public Queue<string> GetOutboundQueue()
        {
            return MessagesOutbound;
        }
        public Queue<string> GetInboundQueue()
        {
            return MessagesInbound;
        }


        public void Open()
        {
            string url = Gaos.Environment.Environment.GetEnvironment()["GAOS_WS"];
            Ws = WebSocketCreate(url);

            WebSocketOnOpen(Ws, "OnOpen");
            WebSocketOnClose(Ws, "OnClose");
            WebSocketOnError(Ws, "OnError");
            WebSocketOnMessage(Ws, "OnMessage");
        }

        public void Send(string data)
        {
            MessagesOutbound.Enqueue(data);
        }


        public IEnumerator StartProcessing()
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
                        string message = MessagesOutbound.Peek();
                        Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: websocket sending message: {message}");
                        try
                        {
                            WebSocketSend(Ws, message);
                            MessagesOutbound.Dequeue();
                            retryCount = 0;
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error sending message: {e.Message}");
                            ++retryCount;
                            if (retryCount > MAX_RETRY_COUNT)
                            {
                                Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error sending message: MAX_RETRY_COUNT reached, will not try again");
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
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

    }

}