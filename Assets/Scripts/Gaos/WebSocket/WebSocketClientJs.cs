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
        private static extern void WebSocketSend(int ws, IntPtr data, int length);

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

        public void OnMessage(IntPtr data)
        {
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
            Ws = WebSocketCreate(url);

            WebSocketOnOpen(Ws, "OnOpen");
            WebSocketOnClose(Ws, "OnClose");
            WebSocketOnError(Ws, "OnError");
            WebSocketOnMessage(Ws, "OnMessage");
        }

        public void Send(byte[] data)
        {
            MessagesOutbound.Enqueue(data);
        }

        public virtual void Process(byte[] data)
        {
            MessagesOutbound.Enqueue(data);
        }


        public IEnumerator StartProcessingOutboundQueue()
        {
            const string METHOD_NAME = "StartProcessingOutboundQueue()";
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
                    yield return new WaitForSeconds(0.1f);
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