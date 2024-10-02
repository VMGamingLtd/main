using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;

namespace Gaos.WebSocket
{
    public interface IWebSocketClient
    {
        public ConcurrentQueue<byte[]> GetOutboundQueue();
        public ConcurrentQueue<byte[]> GetInboundQueue();
        public void Open();
        public void Send(byte[] message);
        public void Process(byte[] message);
        public IEnumerator StartProcessingOutboundQueue(CancellationToken cancellationToken);
        public IEnumerator StartProcessingInboundQueue(WebSocketClient ws, CancellationToken cancellationToken);
        public void ClearQueues();

        public bool GetIsAuthenticated();
        public void SetIsAuthenticated(bool isAuthenticated);
    }
}

