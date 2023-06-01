using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Gaos.Websocket
{
    public class WebSocketClient: MonoBehaviour
    {
        public readonly static string CLASS_NAME = typeof(WebSocketClient).Name;

        public static Queue<string> MessagesOutbound = new Queue<string>();
        public static Queue<string> MessagesInbound = new Queue<string>();

        public static WebSocketSharp.WebSocket WebSocket = new WebSocketSharp.WebSocket(Gaos.Environment.Environment.GetEnvironment()["GAOS_WS"]);
        public static bool IsConnected = false;

        public static void Init()
        {
            const string METHOD_NAME = "Init()"; 

            WebSocket.OnOpen += (sender, e) =>
            {
                Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: OnOpen");
                IsConnected = true;
            };

            WebSocket.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: OnMessage: {e.Data}");
                    MessagesInbound.Enqueue(e.Data);
                }
                else
                {
                    Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: ERROR: OnMessage: message ignored");

                }

            };

            WebSocket.OnError += (sender, e) =>
            {
                Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: ERROR: OnError: {e.Message}");
            };

            WebSocket.OnClose += (sender, e) =>
            {
                Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: OnClose");
                IsConnected = false;

                WebSocket.Connect();
            };

            WebSocket.Connect();

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
                            WebSocket.Send(message);
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