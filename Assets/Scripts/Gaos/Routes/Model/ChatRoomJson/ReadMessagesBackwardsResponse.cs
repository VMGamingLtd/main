#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.ChatRoomJson
{

    [System.Serializable]
    public class ReadMessagesBackwardsResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public ResponseMessage[]? Messages;
        public int MinMessageId;
        public int MaxMessageId;
        public int MessageCount;
    }
}
