using System;
using System.Collections.Generic;
using System.Collections;

namespace Gaos.Websocket
{
    public interface IWebSocketClient
    {
        public Queue<string> GetMessagesOutbound();
        public Queue<string> GetMessagesInbound();
        public void Open();
        public void Send(string data);
        public IEnumerator StartProcessing();
    }

}

