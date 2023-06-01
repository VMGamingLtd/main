using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gaos.Websocket
{
    public class WebSocketClientJs: MonoBehaviour
    {
        public readonly static string CLASS_NAME = typeof(WebSocketClientJs).Name;

        public static Queue<string> MessagesOutbound = new Queue<string>();
        public static Queue<string> MessagesInbound = new Queue<string>();

        private static int Ws;
        private static bool IsConnected = false;

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

        private static void OnOpen()
        {
            IsConnected = true;
        }

        private static void OnClose()
        {
            IsConnected = false;
            Open();
        }

        private static void OnError(string errorStr)
        {
            const string METHOD_NAME = "OnError()";
            Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {errorStr}");
        }

        private static void OnMessage(string data)
        {
            MessagesInbound.Enqueue(data);
        }

        public static void Open()
        {
            const string METHOD_NAME = "Open()";
            string url = Gaos.Environment.Environment.GetEnvironment()["GAOS_WS"];
            Ws = WebSocketCreate(url);

            WebSocketOnOpen(Ws, "OnOpen");
            WebSocketOnClose(Ws, "OnClose");
            WebSocketOnError(Ws, "OnError");
            WebSocketOnMessage(Ws, "OnMessage");
        }

        public static void Init()
        {
            Open();
        }

        public static IEnumerator StartSending()
        {
            const string METHOD_NAME = "StartSending()";
            const int MAX_RETRY_COUNT = 5;

            int retryCount = 0;

            while (true)
            {
                if (IsConnected)
                {
                    if (MessagesOutbound.Count > 0)
                    {
                        string message = MessagesOutbound.Peek();
                        Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: Sending message: {message}");
                        try
                        {
                            WebSocketSend(Ws, message);
                            MessagesOutbound.Dequeue();
                            retryCount = 0;
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: Sending message: {message}: {e.Message}");
                            ++retryCount;
                            if (retryCount > MAX_RETRY_COUNT)
                            {
                                Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: Sending message: {message}: MAX_RETRY_COUNT reached, will not try again");
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