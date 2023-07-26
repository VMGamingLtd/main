#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.ChatRoomJson
{
    [System.Serializable]
    public class ReadMessagesRequest
    {
        public int ChatRoomId;
        public int LastMessageId;
        public int Count;

    }
}
