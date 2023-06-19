using System;

namespace Gaos.WebSocket
{
    public static class WebSocketClientFactory
    {
        public static IWebSocketClient makeWebSocketClient()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
                return new WebSocketClientJs();
#else
            return new WebSocketClientSharp();
#endif
        }
    }
}

