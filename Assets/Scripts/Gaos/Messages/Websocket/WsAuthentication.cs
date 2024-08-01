using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gaos.Messages
{
    public enum WsAuthenticationStatus
    {
        UNAUTHENTICATED,
        AUTHENTICATING,
        AUTHENTICATED,
        ERROR
    }

    public class WsAuthentication
    {
        const string CLASS_NAME = "WsAuthentication";

        private static Dictionary<string, WsAuthentication> requests = new Dictionary<string, WsAuthentication>();

        public WebSocket.IWebSocketClient wsClient;
        public WsAuthenticationStatus status = WsAuthenticationStatus.UNAUTHENTICATED;
        public DateTime requestStartAt;

        public WsAuthentication(WebSocket.IWebSocketClient wsClient)
        {
            this.wsClient = wsClient;
        }


        public static void authenticate(WebSocket.IWebSocketClient wsClient, string token)
        {
            const string METHOD_NAME = "authenticate()";
            if (Gaos.Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true")
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}:authenticating...");
            }
            WsAuthentication wsAuthentication = new WsAuthentication(wsClient);
            try
            {
                string requestId = Guid.NewGuid().ToString();

                GaoProtobuf.AuthenticateRequest authenticateRequest = new GaoProtobuf.AuthenticateRequest();
                authenticateRequest.Token = token;
                authenticateRequest.RequestId = requestId;
                byte [] data = authenticateRequest.ToByteArray();

                wsAuthentication.requestStartAt = DateTime.Now;
                wsAuthentication.status = WsAuthenticationStatus.AUTHENTICATING;
                WsAuthentication.requests.Add(requestId, wsAuthentication);
                Gaos.Websocket.Dispatcher.Send(wsClient, (int)Gaos.Websocket.NamespaceIds.WebbSocket, (int)Gaos.Websocket.WebSocketClassIds.Authenticate, (int)Gaos.Websocket.WebSocketAuthenticateMethodIds.AuthenticateRequest, data);
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error sending message, {e}");
            }
        }

        public static void receiveAuthenticateResponse(byte[] message) {
            const string METHOD_NAME = "receiveAuthenticateResponse()";
            GaoProtobuf.AuthenticateResponse authenticateResponse;
            try
            {
                authenticateResponse = GaoProtobuf.AuthenticateResponse.Parser.ParseFrom(message);
            }
            catch (System.Exception e)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: could not parse message, {e}");
                return;
            }

            if (!WsAuthentication.requests.ContainsKey(authenticateResponse.RequestId))
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: request id not found: {authenticateResponse.RequestId}");
                return;
            }
            WsAuthentication wsAuthentication;
            if (WsAuthentication.requests.TryGetValue(authenticateResponse.RequestId, out wsAuthentication))
            {
                WsAuthentication.requests.Remove(authenticateResponse.RequestId);

                if (authenticateResponse.Result == GaoProtobuf.AuthenticationResult.Success)
                {
                    wsAuthentication.status = WsAuthenticationStatus.AUTHENTICATED;
                    wsAuthentication.wsClient.SetIsAuthenticated(true);
                    if (Gaos.Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true")
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: authenticated");
                    }
                }
                else if (authenticateResponse.Result == GaoProtobuf.AuthenticationResult.Unauthorized)
                {
                    wsAuthentication.status = WsAuthenticationStatus.UNAUTHENTICATED;
                    wsAuthentication.wsClient.SetIsAuthenticated(false);
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: unauthenticated");
                }
                else if (authenticateResponse.Result == GaoProtobuf.AuthenticationResult.Error)
                {
                    wsAuthentication.status = WsAuthenticationStatus.ERROR;
                    wsAuthentication.wsClient.SetIsAuthenticated(false);
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: error while authenticating");
                }
                else
                {
                    wsAuthentication.status = WsAuthenticationStatus.ERROR;
                    wsAuthentication.wsClient.SetIsAuthenticated(false);
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: warn: unknown authentication result: {authenticateResponse.Result}");
                }
            }
            else
            {
                    wsAuthentication.status = WsAuthenticationStatus.ERROR;
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: warn: unknown requestId not found");
            }
        }

        public static void disposeRequests()
        {
            const string METHOD_NAME = "disposeRequests()";
            List<string> keys = new List<string>(WsAuthentication.requests.Keys);
            foreach (string key in keys)
            {
                WsAuthentication wsAuthentication = WsAuthentication.requests[key];
                if (DateTime.Now.Subtract(wsAuthentication.requestStartAt).TotalSeconds > 10)
                {
                    if (wsAuthentication.status == WsAuthenticationStatus.AUTHENTICATING)
                    {
                            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: warn: request timed out");
                    }
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: removing request: {key}");
                    WsAuthentication.requests.Remove(key);
                } 
            }
        }
    }
}
