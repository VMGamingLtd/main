using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Gaos.WebSocket
{
    public class WebSocketClient: MonoBehaviour, Gaos.WebSocket.IWebSocketClient, Gaos.WebSocket.IWebSocketClientHandler
    {
        public WebSocketClientSharp webSocketClientSharp;
        public WebSocketClientJs webSocketClientJs;

        private IWebSocketClientHandler websocketHandler;
        private Gaos.Messages.Dispatcher dispatcher;

        public WebSocketClient()
        {

            this.dispatcher = new Gaos.Messages.Dispatcher(this);
            websocketHandler = new WebsocketHandler(this.dispatcher);

        }

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

        public void Process(byte[] data)
        {
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

        public IEnumerator StartProcessingInboundQueue(IWebSocketClientHandler handler)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return  webSocketClientJs.StartProcessingInboundQueue(handler);
            }
            else
            {
                return webSocketClientSharp.StartProcessingInboundQueue(handler);
            }
        }

        public void OnEnable()
        {
            Open();
            StartCoroutine(StartProcessingOutboundQueue());
            StartCoroutine(StartProcessingInboundQueue(websocketHandler));
            //Send("ping");
        }

    }
}

