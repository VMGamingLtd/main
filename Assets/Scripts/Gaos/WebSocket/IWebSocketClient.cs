using System;
using System.Collections.Generic;
using System.Collections;

namespace Gaos.Websocket
{
    public interface IWebSocketClient
    {
        public Queue<string> GetOutboundQueue();
        public Queue<string> GetInboundQueue();
        public void Open();
        public void Send(string data);
        public IEnumerator StartProcessing();
    }

}

