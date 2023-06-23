using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Gaos.WebSocket
{
    public class WebSocketClient: MonoBehaviour, Gaos.WebSocket.IWebSocketClient
    {
        public WebSocketClientSharp webSocketClientSharp;
        public WebSocketClientJs webSocketClientJs;

        public Queue<string> GetOutboundQueue()
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

        public Queue<string> GetInboundQueue()
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
        }

        public void Send(string data)
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

        public IEnumerator StartProcessing()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return  webSocketClientJs.StartProcessing();
            }
            else
            {
                return webSocketClientSharp.StartProcessing();
            }
        }

        public void OnEnable()
        {
            Open();
            StartCoroutine(StartProcessing());
            Send("ping");
        }

    }
}

