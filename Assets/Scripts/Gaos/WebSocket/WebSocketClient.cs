using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Gaos.Websocket;
using Google.Protobuf;

namespace Gaos.WebSocket
{
    public class WebSocketClient: MonoBehaviour, Gaos.WebSocket.IWebSocketClient
    {
        static public string CLASS_NAME = typeof(WebSocketClient).Name;
        public WebSocketClientSharp webSocketClientSharp;
        public WebSocketClientJs webSocketClientJs;

        public static WebSocketClient CurrentWesocketClient = null;


        public Queue<byte[]> GetOutboundQueue()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return webSocketClientJs.GetOutboundQueue();
            }
            else
            {
                return webSocketClientSharp.GetOutboundQueue();
            }
        }

        public Queue<byte[]> GetInboundQueue()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return webSocketClientJs.GetInboundQueue();
            }
            else
            {
                return webSocketClientSharp.GetInboundQueue();
            }
        }

        public void Open()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                webSocketClientJs.Open();
            }
            else
            {
                webSocketClientSharp.Open();
            }
            Gaos.Messages.Websocket.PingPong.SendPing(this, "Hello from unity!");


        }

        public void Send(byte[] data)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                webSocketClientJs.Send(data);
            }
            else
            {
                webSocketClientSharp.Send(data);
            }
        }

        public void Process(byte[] buffer)
        {
             string METHOD_NAME = "Process()";

            try
            {
                uint bytesReadHeader = 0;
                GaoProtobuf.MessageHeader header = ParseMessageHeader(buffer, ref bytesReadHeader);

                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: header: {header}"); //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

                uint bytesReadSize = 0;
                uint messageObjectSize = DeserializeMessageSize(buffer, bytesReadHeader, ref bytesReadSize);

                byte[] data = new byte[messageObjectSize];
                Array.Copy(buffer, bytesReadHeader + bytesReadSize, data, 0, messageObjectSize);


                Dispatcher.Dispatch(this, header.NamespaceId, header.ClassId, header.MethodId, data);
            } 
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message: {e}");
            }
        }

        public IEnumerator StartProcessingOutboundQueue()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return  webSocketClientJs.StartProcessingOutboundQueue();
            }
            else
            {
                return webSocketClientSharp.StartProcessingOutboundQueue();
            }
        }

        public IEnumerator StartProcessingInboundQueue(WebSocketClient ws)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return  webSocketClientJs.StartProcessingInboundQueue(ws);
            }
            else
            {
                return webSocketClientSharp.StartProcessingInboundQueue(ws);
            }
        }

        public void OnEnable()
        {
            CurrentWesocketClient = this;
            Open();
            StartCoroutine(StartProcessingOutboundQueue());
            StartCoroutine(StartProcessingInboundQueue(this));
        }

        public static string ToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        public static uint ToNetworkByteOrder(uint value)
        {
            return ((value & 0x000000FF) << 24) |
                   ((value & 0x0000FF00) << 8) |
                   ((value & 0x00FF0000) >> 8) |
                   ((value & 0xFF000000) >> 24);
        }

        public static uint FromNetworkByteOrder(uint value)
        {
            // Note that this function is the same as toNetworkByteOrder becasuse both operations
            // are actually only reversing the byte order of the value.
            return ToNetworkByteOrder(value);
        }

        public static byte[] SerializeMessageaSize(uint size)
        {
            byte[] sizeBytes = BitConverter.GetBytes(ToNetworkByteOrder(size));
            return sizeBytes;
        }


        public static uint DeserializeMessageSize(byte[] message, uint bufferOffset, ref uint bytesRead)
        {
            byte[] data = new byte[4];
            Array.Copy(message, bufferOffset, data, 0, 4);
            bytesRead = 4;
            return FromNetworkByteOrder(BitConverter.ToUInt32(data, 0));
        }

        public static byte[] SerializeMessageHeader(GaoProtobuf.MessageHeader messageHeader)
        {
            byte[] message = messageHeader.ToByteString().ToByteArray();
            byte[] size = BitConverter.GetBytes(ToNetworkByteOrder((uint)message.Length));
            byte[] buffer = new byte[size.Length + message.Length];
            size.CopyTo(buffer, 0);
            message.CopyTo(buffer, size.Length);
            return buffer;
        }

        public static GaoProtobuf.MessageHeader  ParseMessageHeader(byte[] message, ref uint bytesRead)
        {
            uint bytesReadSize = 0;
            uint messageSize = DeserializeMessageSize(message, 0,  ref bytesReadSize);

            byte[] data = new byte[messageSize];
            Array.Copy(message, bytesReadSize, data, 0, messageSize);
            var messageHeader =  GaoProtobuf.MessageHeader.Parser.ParseFrom(data);

            bytesRead = bytesReadSize + messageSize;
            return messageHeader;
        }

        public static byte[] SerializeMessage(GaoProtobuf.MessageHeader messageHeader, byte [] message)
        {
            byte[] header = SerializeMessageHeader(messageHeader);

            byte[] size = BitConverter.GetBytes(ToNetworkByteOrder((uint)message.Length));


            // concatente header, size, and message
            byte[] buffer = new byte[header.Length + size.Length + message.Length];
            header.CopyTo(buffer, 0);
            size.CopyTo(buffer, header.Length);
            message.CopyTo(buffer, header.Length + size.Length);


            return buffer;
        }

        public bool GetIsAuthenticated()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return  webSocketClientJs.GetIsAuthenticated();
            }
            else
            {
                return webSocketClientSharp.GetIsAuthenticated();
            }
        }

        public void SetAuthenticated()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                webSocketClientJs.SetAuthenticated();
            }
            else
            {
                webSocketClientSharp.SetAuthenticated();
            }
        }
    }
}

