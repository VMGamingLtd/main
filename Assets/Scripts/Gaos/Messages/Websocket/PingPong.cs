using UnityEngine;
using GaoProtobuf;
using Gaos.WebSocket;
using Google.Protobuf;


namespace Messages.WebSocket
{

    public class PingPong 
    {
        public static string CLASS_NAME = typeof(PingPong).Name;
        IWebSocketClient webSocketClient;

        public PingPong(IWebSocketClient _webSocketClient)
        {
            webSocketClient = _webSocketClient;
        }

        public void OnPing(byte[] message)
        {
            const string METHOD_NAME = "OnPing()";
            GaoProtobuf.Ping ping;
            try
            {
                ping = GaoProtobuf.Ping.Parser.ParseFrom(message);
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: could not parse message,  {e}");
                return;
            }
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: received ping: {ping.Message}");
        }

        public void OnPong(byte[] message)
        {
            const string METHOD_NAME = "OnPong()";
            GaoProtobuf.Pong pong;
            try
            {
                pong = GaoProtobuf.Pong.Parser.ParseFrom(message);
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: could not parse message,  {e}");
                return;
            }
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: received pong: {pong.Message}");
        }

        public void SendPing()
        {
            //const string METHOD_NAME = "SendPing()";
            GaoProtobuf.Ping ping = new GaoProtobuf.Ping();
            ping.Header.Namespace = (int)Gaos.Messages.NamespaceIds.WebbSocket;
            ping.Header.Classs = (int)Gaos.Messages.WebSocketClassIds.PingPong;
            ping.Header.Method = (int)Gaos.Messages.WebSocketPingPongMethodIds.Ping;
            ping.Message = "ping from unity!";

            byte[] data = ping.ToByteArray();
            webSocketClient.Send(data);

        }

        public void SendPong()
        {
            //const string METHOD_NAME = "SendPong()";
            GaoProtobuf.Pong pong = new GaoProtobuf.Pong();
            pong.Header.Namespace = (int)Gaos.Messages.NamespaceIds.WebbSocket;
            pong.Header.Classs = (int)Gaos.Messages.WebSocketClassIds.PingPong;
            pong.Header.Method = (int)Gaos.Messages.WebSocketPingPongMethodIds.Pong;
            pong.Message = "pong from unity!";

            byte[] data = pong.ToByteArray();
            webSocketClient.Send(data);
        }
    }
}
