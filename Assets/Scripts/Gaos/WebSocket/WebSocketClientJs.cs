using AOT;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;

namespace Gaos.WebSocket
{
    public class WebSocketClientJs: MonoBehaviour, Gaos.WebSocket.IWebSocketClient
    {
        public readonly static string CLASS_NAME = typeof(WebSocketClientJs).Name;

        private Queue<byte[]> MessagesOutbound = new Queue<byte[]>();
        private Queue<byte[]> MessagesInbound = new Queue<byte[]>();

        private int Ws;
        private bool IsAuthenticated = false;

        [DllImport("__Internal")]
        private static extern int WebSocketCreate(string url, string fnNameOnOpen, string fnNameOnClose, string fnNameOnError, string fnNameOnMessage);

        [DllImport("__Internal")]
        private static extern void WebSocketSend(int ws, IntPtr data, int length);

        [DllImport("__Internal")]
        private static extern int WebSocketReadyState(int ws);

        private static void WebSocketSendBytes(int ws, byte[] buffer)
        {
            if (buffer.Length < 1)
            {
                return;
            }
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    WebSocketSend(ws, (IntPtr)p, buffer.Length);
                }
            }
        }


        public void OnOpen()
        {
            Debug.Log($"{CLASS_NAME}: OnOpen()"); //@@@@@@@@@@@@@@@@
            if (Gaos.Context.Authentication.GetJWT() != null)
            {
                Debug.Log($"{CLASS_NAME}: OnOpen()");
                Gaos.Messages.WsAuthentication.authenticate(this, Gaos.Context.Authentication.GetJWT());
            }
        }

        public void OnClose()
        {
            Debug.Log($"{CLASS_NAME}: OnClose()"); //@@@@@@@@@@@@@@@@
        }

        public void OnError(string errorStr)
        {
            Debug.Log($"{CLASS_NAME}: OnError(): {errorStr}"); //@@@@@@@@@@@@@@@@
            const string METHOD_NAME = "OnError()";
            Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {errorStr}");
        }

        public void OnMessage(int _data)
        {
            var data = new IntPtr(_data);

            Debug.Log($"{CLASS_NAME}: OnMessage()"); //@@@@@@@@@@@@@@@@
            int length =  Marshal.ReadInt32(data);
            byte[] buffer = new byte[length];

            unsafe
            {
                byte* p = (byte*)data + 4; // skip the length
                for (int i = 0; i < length; i++)
                {
                    buffer[i] = *p++;
                }
            }
            MessagesInbound.Enqueue(buffer);

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
            string url = Gaos.Environment.Environment.GetEnvironment()["GAOS_WS"];
            Ws = WebSocketCreate(url, "OnOpen", "OnClose", "OnError", "OnMessage");
            IsAuthenticated = false;
        }

        public void Send(byte[] data)
        {
            MessagesOutbound.Enqueue(data);
        }

        public void Process(byte[] data)
        {
        }


        public IEnumerator StartProcessingOutboundQueue()
        {
            const string METHOD_NAME = "StartProcessingOutboundQueue()";
            const int MAX_RETRY_COUNT = 5;

            int retryCount = 0;

            while (true)
            {
                int state = WebSocketReadyState(Ws);
                if (state == 1) // 1 is OPEN
                {
                    if (MessagesOutbound.Count > 0)
                    {
                        byte[] data = MessagesOutbound.Peek();
                        try
                        {
                            WebSocketSendBytes(Ws, data);
                            MessagesOutbound.Dequeue();
                            retryCount = 0;
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error sending message: {e.Message}");
                            ++retryCount;
                            if (retryCount > MAX_RETRY_COUNT)
                            {
                                Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error sending message: MAX_RETRY_COUNT reached therefore message shall be thrown away");
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
                    if (state == 3) // 3 is CLOSED
                    {
                        Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: websocket is closed, will try to open it again");
                        yield return new WaitForSeconds(2);
                        Open();
                    }
                    else if (state == 0) // 0 is CONNECTING
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    else if (state == 2) // 2 is CLOSING
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    else
                    {
                        Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: websocket is in unknown state: {state}");
                        yield return new WaitForSeconds(5);
                        Open();
                    }
                }
            }
        }

        public IEnumerator StartProcessingInboundQueue(WebSocketClient ws)
        {
            const string METHOD_NAME = "StartProcessingInboundQueue()";
            while (true)
            {
                int state = WebSocketReadyState(Ws);
                if (state == 1) // 1 is OPEN
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
                            Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error processing message: {e.Message}");
                            MessagesInbound.Dequeue();
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

        public bool GetIsAuthenticated()
        {
            return IsAuthenticated;
        }

        public void SetIsAuthenticated(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }

        // --------------------------   UnityBrowserMessaging --------------------------

        [DllImport("__Internal")]
        public static extern void UnityBrowserChannel_BaseMessages_sendString(string str);

        public void UnityBrowserChannel_BaseMessages_receiveString(string str)
        {
            const string METHOD_NAME = "UnityBrowserChannel_BaseMessages_receiveString()";
            Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: str: {str}");
            UnityBrowserChannel.BaseMessages.receiveString(str);
        }

    }



}