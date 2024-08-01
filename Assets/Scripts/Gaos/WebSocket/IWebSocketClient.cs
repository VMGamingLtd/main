using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;

namespace Gaos.WebSocket
{
    public interface IWebSocketClient
    {
        public ConcurrentQueue<byte[]> GetOutboundQueue();
        public ConcurrentQueue<byte[]> GetInboundQueue();
        public void Open();
        public void Send(byte[] message);
        public void Process(byte[] message);
        public IEnumerator StartProcessingOutboundQueue();
        public IEnumerator StartProcessingInboundQueue(WebSocketClient ws);

        public bool GetIsAuthenticated();
        public void SetIsAuthenticated(bool isAuthenticated);
    }
}

