using Gaos.WebSocket;
using Messages.WebSocket;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gaos.Messages
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

        IWebSocketClient webSocketClient;
        PingPong pingPong;

        public Dispatcher(IWebSocketClient webSocketClient)
        {
            this.webSocketClient = webSocketClient;
            this.pingPong = new PingPong(webSocketClient);
        }

        public bool Dispatch(int namespaceId, int classId, int methodId, byte[] message)
        {
            const string METHOD_NAME = "Dispatch()";

            try
            {
                switch (namespaceId)
                {
                    case (int)NamespaceIds.WebbSocket:
                        return DispatchWebsocket(namespaceId,   classId, methodId, message);
                    default:
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such namespaceId, namespaceId: {namespaceId}, classId: {classId}, methodId: {methodId}");
                        return false;
                }
            }
            catch (System.Exception e) 
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message, namespaceId: {namespaceId}, classId: {classId}, methodId: {methodId}");
                Debug.Log(e);
                return false;
            }
        }

        public bool DispatchWebsocket(int namespaceId, int classId, int methodId, byte[] message)
        {
            const string METHOD_NAME = "DispatchWebsocket()";
            switch (classId)
            {
            case (int)WebSocketPingPongMethodIds.Ping:
                    switch (methodId)
                    {
                        case (int)WebSocketPingPongMethodIds.Ping:
                            pingPong.OnPing(message);
                            return true;
                        case (int)WebSocketPingPongMethodIds.Pong:
                            pingPong.OnPong(message);
                            return true;
                        default:
                            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such methodId, namespaceId: {namespaceId}, classId: {classId}, methodId: {methodId}");
                            return false;
                    }
            default:
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such classId, namespaceId: {namespaceId}, classId: {classId}, methodId: {methodId}");
                return false;
            }
        }
    }
}
