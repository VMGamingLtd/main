#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.ChatRoomJson
{
    [System.Serializable]
    public class WriteMessageResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public int MinMessageId;
        public int MaxMessageId;
        public int MessageCount;
    }
}
