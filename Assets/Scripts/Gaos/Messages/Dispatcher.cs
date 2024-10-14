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
        Group = 3,
        Gaos = 4
          
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

    public enum GaosClassIds
    {
        Broadcast = 1,
    }

    public enum GaosBroadcastMethodIds
    {
        GroupCreditsChange = 1,
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


        public static void Dispatch(Gaos.WebSocket.IWebSocketClient ws, GaoProtobuf.MessageHeader header, byte[] message)
        {
            const string METHOD_NAME = "Dispatch()";

            disposeRequests();

            try
            {
                switch (header.NamespaceId)
                {
                    case (int)NamespaceIds.WebbSocket:
                        DispatchWebsocket(ws, header, message);
                        return;
                    case (int)NamespaceIds.UnityBrowserChannel:
                        DispatchUnityBrowserChannel(ws, header, message);
                        return;
                    case (int)NamespaceIds.Group:
                        DispatchGroup(ws, header, message);
                        return;
                    default:
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such namespaceId, namespaceId: {header.NamespaceId}, classId: {header.ClassId}, methodId: {header.MethodId}");
                        return;
                }
            }
            catch (System.Exception e) 
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message, namespaceId: {header.NamespaceId}, classId: {header.ClassId}, methodId: {header.MethodId}");
                Debug.Log(e);
                return;
            }
        }

        public static void DispatchWebsocket(Gaos.WebSocket.IWebSocketClient ws, GaoProtobuf.MessageHeader header, byte[] message)
        {
            const string METHOD_NAME = "DispatchWebsocket()";
            switch (header.ClassId)
            {
                case (int)WebSocketClassIds.PingPong:
                    switch (header.MethodId)
                    {
                        case (int)WebSocketPingPongMethodIds.Ping:
                            PingPong.OnPing(ws, message);
                            return;
                        case (int)WebSocketPingPongMethodIds.Pong:
                            PingPong.OnPong(ws, message);
                            return;
                        default:
                            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such methodId, namespaceId: {header.NamespaceId} , classId:  {header.ClassId} , methodId:  {header.MethodId}");
                            return;
                    }
                case (int)WebSocketClassIds.Authenticate:
                        switch (header.MethodId)
                        {
                            case (int)WebSocketAuthenticateMethodIds.AuthenticateResponse:
                                Gaos.Messages.WsAuthentication.receiveAuthenticateResponse(message);
                                return;
                            default:
                                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such methodId, namespaceId: {header.NamespaceId} , classId:  {header.ClassId} , methodId:  {header.MethodId}");
                                return;
                        }
                default:
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such classId, namespaceId: {header.NamespaceId} , classId:  {header.ClassId} , methodId:  {header.MethodId}");
                    return;
                }
        }

        public static void DispatchUnityBrowserChannel(Gaos.WebSocket.IWebSocketClient ws, GaoProtobuf.MessageHeader header, byte[] message)
        {
            const string METHOD_NAME = "DispatchUnityBrowserChannel()";
            switch (header.ClassId)
            {
                case (int)UnityBrowserChannelClassIds.BaseMessages:
                        switch (header.MethodId)
                        {
                            case (int)UnityBrowserChannelBaseMessagingMethodIds.ReceiveString:
                                UnityBrowserChannel.BaseMessages.receiveStringWs(ws, message);
                                return;
                            default:
                                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such methodId, namespaceId: {header.NamespaceId}  , classId:   {header.ClassId}  , methodId:   {header.MethodId}");
                                return;
                        }
                default:
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such classId, namespaceId: {header.NamespaceId}  , classId:   {header.ClassId}  , methodId:   {header.MethodId}");
                    return;
                }
        }

        public static void DispatchGroup(Gaos.WebSocket.IWebSocketClient ws, GaoProtobuf.MessageHeader header, byte[] message)
        {
            const string METHOD_NAME = "DispatchGroup()";
            switch (header.ClassId)
            {
                case (int)GroupClassIds.Broadcast:
                    switch (header.MethodId)
                    {
                        case (int)GroupBroadcastMethodIds.GroupGameDataChanged:
                            Gaos.Messages.Group.GroupGameDataChanged.receive(ws, message);
                            return;
                        default:
                            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such methodId, namespaceId: {header.NamespaceId}  , classId:   {header.ClassId}  , methodId:   {header.MethodId}");
                            return;
                    }
                default:
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such classId, namespaceId: {header.NamespaceId}, classId: {header.ClassId}, methodId: {header.MethodId}");
                    return;
            }
        }

        public static void DispatchGaos(Gaos.WebSocket.IWebSocketClient ws, GaoProtobuf.MessageHeader header, byte[] message)
        {
            const string METHOD_NAME = "DispatchGaos()";
            switch (header.ClassId)
            {
                case (int)GaosClassIds.Broadcast:
                    switch (header.MethodId)
                    {
                        case (int)GaosBroadcastMethodIds.GroupCreditsChange:
                            Gaos.Messages.GaosServer.GroupCreditsChange.receive(ws, header, message);
                            return;
                        default:
                            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such methodId, namespaceId: {header.NamespaceId} , classId:  {header.ClassId}, methodId: {header.MethodId}");
                            return;
                    }
                default:
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error processing message - no such classId, namespaceId: {header.NamespaceId} , classId:  {header.ClassId}, methodId: {header.MethodId}");
                    return;
            }
        }

        static void disposeRequests()
        {
            Gaos.Messages.WsAuthentication.disposeRequests();
        }
    }
}
