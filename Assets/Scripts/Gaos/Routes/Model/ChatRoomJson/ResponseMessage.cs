#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.ChatRoomJson
{
    [System.Serializable]
    public class ResponseMessage
    {
        public int MessageId;
        public string? Message;
        public System.DateTime CreatedAt;
        public int UserId;
        public string? UserName;

    }

}
