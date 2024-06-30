using System;
using System.Collections.Generic;
using System.Collections;

namespace Gaos.WebSocket
{
    public interface IWebSocketClient
    {
        public Queue<byte[]> GetOutboundQueue();
        public Queue<byte[]> GetInboundQueue();
        public void Open();
        public void Send(byte[] message);
        public void Process(byte[] message);
        public IEnumerator StartProcessingOutboundQueue();
        public IEnumerator StartProcessingInboundQueue(WebSocketClient ws);

        public bool GetIsAuthenticated();
        public void SetAuthenticated();
    }
}

