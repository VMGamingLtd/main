using UnityEngine;
using GaoProtobuf;
using Gaos.WebSocket;
using Google.Protobuf;


namespace Gaos.Messages.Websocket
{

    public class PingPong 
    {
        public static string CLASS_NAME = typeof(PingPong).Name;


        static public void OnPing(Gaos.WebSocket.IWebSocketClient ws, byte[] message)
        {
            const string METHOD_NAME = "OnPing()";
            GaoProtobuf.Ping ping;
            try
            {
                ping = GaoProtobuf.Ping.Parser.ParseFrom(message);
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: could not parse message, {e}");
                return;
            }
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: received ping: {ping.Message}");
        }

        public static void OnPong(Gaos.WebSocket.IWebSocketClient ws, byte[] message)
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

        public static void SendPing(Gaos.WebSocket.IWebSocketClient ws, string message)
        {
            const string METHOD_NAME = "SendPing()";
            try
            {
                GaoProtobuf.Ping ping = new GaoProtobuf.Ping();
                ping.Message = message;
                byte[] data = ping.ToByteArray();

                Gaos.Websocket.Dispatcher.Send(ws, (int)Gaos.Websocket.NamespaceIds.WebbSocket, (int)Gaos.Websocket.WebSocketClassIds.PingPong, (int)Gaos.Websocket.WebSocketPingPongMethodIds.Ping, data);
            } 
            catch (System.Exception e) {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error sending message, {e}");
            }

        }

        public static void SendPong(Gaos.WebSocket.IWebSocketClient ws, string message)
        {
            const string METHOD_NAME = "SendPong()";
            try
            {
                GaoProtobuf.Pong pong = new GaoProtobuf.Pong();
                pong.Message = message;
                byte[] data = pong.ToByteArray();

                Gaos.Websocket.Dispatcher.Send(ws, (int)Gaos.Websocket.NamespaceIds.WebbSocket, (int)Gaos.Websocket.WebSocketClassIds.PingPong, (int)Gaos.Websocket.WebSocketPingPongMethodIds.Pong, data);
            } 
            catch (System.Exception e) {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error sending message, {e}");
            }
        }
    }
}
