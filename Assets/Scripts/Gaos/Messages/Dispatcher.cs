using Gaos.WebSocket;
using Gaos.Messages.Websocket;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gaos.Websocket
{
    public enum NamespaceIds
    {
        WebbSocket = 1,
    }

    public enum WebSocketClassIds
    {
        PingPong  = 1,
    }

    public enum WebSocketPingPongMethodIds
    {
        Ping = 1,
        Pong = 2,
    }

    public class Dispatcher 
    {
        public static string CLASS_NAME = typeof(Dispatcher).Name;

        public static void Send(Gaos.WebSocket.IWebSocketClient ws, int namespaceId, int classId, int methodId, byte[] message)
        {
            const string METHOD_NAME = "Send()";

            try
            {

                GaoProtobuf.MessageHeader messageHeader = new GaoProtobuf.MessageHeader();
                messageHeader.NamespaceId = namespaceId;
                messageHeader.ClassId = classId;
                messageHeader.MethodId = methodId;

                byte[] data = Gaos.WebSocket.WebSocketClient.SerializeMessage(messageHeader, message);

                ws.Send(data);
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error sending message, namespaceId: {namespaceId}, classId: {classId}, methodId: {methodId}");
                Debug.Log(e);
            }
        }


        public static void Dispatch(Gaos.WebSocket.IWebSocketClient ws, int namespaceId, int classId, int methodId, byte[] message)
        {
            const string METHOD_NAME = "Dispatch()";

            try
            {
                switch (namespaceId)
                {
                    case (int)NamespaceIds.WebbSocket:
                        DispatchWebsocket(ws, namespaceId,   classId, methodId, message);
                        return;
                    default:
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such namespaceId, namespaceId: {namespaceId}, classId: {classId}, methodId: {methodId}");
                        return;
                }
            }
            catch (System.Exception e) 
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message, namespaceId: {namespaceId}, classId: {classId}, methodId: {methodId}");
                Debug.Log(e);
                return;
            }
        }

        public static void DispatchWebsocket(Gaos.WebSocket.IWebSocketClient ws, int namespaceId, int classId, int methodId, byte[] message)
        {
            const string METHOD_NAME = "DispatchWebsocket()";
            switch (classId)
            {
            case (int)WebSocketPingPongMethodIds.Ping:
                    switch (methodId)
                    {
                        case (int)WebSocketPingPongMethodIds.Ping:
                            PingPong.OnPing(ws, message);
                            return;
                        case (int)WebSocketPingPongMethodIds.Pong:
                            PingPong.OnPong(ws, message);
                            return;
                        default:
                            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such methodId, namespaceId: {namespaceId}, classId: {classId}, methodId: {methodId}");
                            return;
                    }
            default:
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such classId, namespaceId: {namespaceId}, classId: {classId}, methodId: {methodId}");
                return;
            }
        }
    }
}
