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
        UnityBrowserChannel = 2,
        Group = 3
    }

    public enum WebSocketClassIds
    {
        PingPong  = 1,
        Authenticate = 2,
    }

    public enum WebSocketPingPongMethodIds
    {
        Ping = 1,
        Pong = 2,
    }

    public enum WebSocketAuthenticateMethodIds
    {
        AuthenticateRequest = 1,
        AuthenticateResponse = 2,
    }
    

    public enum UnityBrowserChannelClassIds
    {
        BaseMessages = 1,
    }

    public enum UnityBrowserChannelBaseMessagingMethodIds
    {
        ReceiveString = 1,
    }

    public enum GroupClassIds
    {
       Broadcast = 1,
    }

    public enum GroupBroadcastMethodIds
    {
        GroupGameDataChanged = 1,
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

            disposeRequests();

            try
            {
                switch (namespaceId)
                {
                    case (int)NamespaceIds.WebbSocket:
                        DispatchWebsocket(ws, namespaceId,   classId, methodId, message);
                        return;
                    case (int)NamespaceIds.UnityBrowserChannel:
                        DispatchUnityBrowserChannel(ws, namespaceId, classId, methodId, message);
                        return;
                    case (int)NamespaceIds.Group:
                        DispatchGroup(ws, namespaceId, classId, methodId, message);
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
                case (int)WebSocketClassIds.PingPong:
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
                case (int)WebSocketClassIds.Authenticate:
                        switch (methodId)
                        {
                            case (int)WebSocketAuthenticateMethodIds.AuthenticateResponse:
                                Gaos.Messages.WsAuthentication.receiveAuthenticateResponse(message);
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

        public static void DispatchUnityBrowserChannel(Gaos.WebSocket.IWebSocketClient ws, int namespaceId, int classId, int methodId, byte[] message)
        {
            const string METHOD_NAME = "DispatchUnityBrowserChannel()";
            switch (classId)
            {
                case (int)UnityBrowserChannelClassIds.BaseMessages:
                        switch (methodId)
                        {
                            case (int)UnityBrowserChannelBaseMessagingMethodIds.ReceiveString:
                                UnityBrowserChannel.BaseMessages.receiveStringWs(ws, message);
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

        public static void DispatchGroup(Gaos.WebSocket.IWebSocketClient ws, int namespaceId, int classId, int methodId, byte[] message)
        {
            const string METHOD_NAME = "DispatchGroup()";
            switch (classId)
            {
                case (int)GroupClassIds.Broadcast:
                    switch (methodId)
                    {
                        case (int)GroupBroadcastMethodIds.GroupGameDataChanged:
                            Gaos.Messages.Group.GroupGameDataChanged.receive(ws, message);
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

        static void disposeRequests()
        {
            Gaos.Messages.WsAuthentication.disposeRequests();
        }
    }
}
