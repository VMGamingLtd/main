using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GaoProtobuf;

namespace Gaos.WebSocket
{
    public class WebsocketHandler: Gaos.WebSocket.IWebSocketClientHandler
    {
        Gaos.Messages.Dispatcher dispatcher;
        public WebsocketHandler(Gaos.Messages.Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void Process(byte[] data)
        {
            MessageHeader header = MessageHeader.Parser.ParseFrom(data);
            dispatcher.Dispatch(header.Namespace, header.Classs, header.Method, data);
        }
    }
}
