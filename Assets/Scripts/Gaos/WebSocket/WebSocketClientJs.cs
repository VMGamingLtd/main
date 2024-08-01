using AOT;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;

namespace Gaos.WebSocket
{
    public class WebSocketClientJs: MonoBehaviour, Gaos.WebSocket.IWebSocketClient
    {
        public readonly static string CLASS_NAME = typeof(WebSocketClientJs).Name;

        private ConcurrentQueue<byte[]> MessagesOutbound = new ConcurrentQueue<byte[]>();
        private ConcurrentQueue<byte[]> MessagesInbound = new ConcurrentQueue<byte[]>();

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
            if (Gaos.Context.Authentication.GetJWT() != null)
            {
                Debug.Log($"{CLASS_NAME}: OnOpen()");
                Gaos.Messages.WsAuthentication.authenticate(this, Gaos.Context.Authentication.GetJWT());
            }
        }

        public void OnClose()
        {
        }

        public void OnError(string errorStr)
        {
            const string METHOD_NAME = "OnError()";
            Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {errorStr}");
        }

        public void OnMessage(int _data)
        {
            var data = new IntPtr(_data);

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

        public ConcurrentQueue<byte[]> GetOutboundQueue()
        {
            return MessagesOutbound;
        }
        public ConcurrentQueue<byte[]> GetInboundQueue()
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

            while (true)
            {
                int state = WebSocketReadyState(Ws);
                if (state == 1) // 1 is OPEN
                {
                    if (MessagesOutbound.Count > 0)
                    {
                        byte[] data;
                        bool dataReturned = MessagesOutbound.TryDequeue(out data);
                        if (dataReturned)
                        {
                            try
                            {
                                WebSocketSendBytes(Ws, data);
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error sending message: {e.Message}");
                            }
                            yield return null;
                        }
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
                                Debug.LogWarning($"{CLASS_NAME}.{METHOD_NAME}: ERROR: error processing message: {e.Message}");
                            }
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
            UnityBrowserChannel.BaseMessages.receiveString(str);
        }

    }



}