using Google.Protobuf;
using System;
using UnityEngine;

namespace UnityBrowserChannel
{
    class BaseMessages
    {
        public static string CLASS_NAME = typeof(BaseMessages).Name;

        public static void sendString(Gaos.WebSocket.WebSocketClient wsClient, string str)
        {
            const string METHOD_NAME = "sendString()";
            if (Application.isEditor)
            {
                // Indirect call via websocket message
                try
                {
                    GaoProtobuf.StringMessage stringMessage = new GaoProtobuf.StringMessage();
                    stringMessage.Str = str;
                    byte[] data = stringMessage.ToByteArray();

                    Gaos.Websocket.Dispatcher.Send(wsClient, (int)Gaos.Websocket.NamespaceIds.UnityBrowserChannel, (int)Gaos.Websocket.UnityBrowserChannelClassIds.BaseMessages, (int)Gaos.Websocket.UnityBrowserChannelBaseMessagingMethodIds.ReceiveString, data);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error sending message");
                    UnityEngine.Debug.Log(e);
                }
            } 
            else
            {
                // Direct call 
                Gaos.WebSocket.WebSocketClientJs.UnityBrowserMessaging_BaseMessages_sendString(str);
            }
        }

        public static void receiveStringWs(Gaos.WebSocket.IWebSocketClient ws, byte[] message)
        {
            const string METHOD_NAME = "receiveString()";
            GaoProtobuf.StringMessage stringMessage;
            try
            {
                stringMessage = GaoProtobuf.StringMessage.Parser.ParseFrom(message);
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: could not parse message, {e}");
                return;
            }

            receiveString(stringMessage.Str);
        }

        public static void receiveString(string str)
        {
            const string METHOD_NAME = "receiveString()";
            Debug.Log($"{CLASS_NAME}.{METHOD_NAME}: str: {str}");
        }

    }
}
