using System;
using System.Collections.Generic;
using System.Collections;

namespace Gaos.WebSocket
{
    public interface IWebSocketClientHandler
    {
        public void Process(byte[] data);
    }

    public interface IWebSocketClient
    {
        public Queue<byte[]> GetOutboundQueue();
        public Queue<byte[]> GetInboundQueue();
        public void Open();
        public void Send(byte[] data);
        public IEnumerator StartProcessingOutboundQueue();
        public IEnumerator StartProcessingInboundQueue(IWebSocketClientHandler handler);
    }


}

