#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.ChatRoomJson
{
    [System.Serializable]
    public class ExistsChatRoomResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public bool? IsExists;
        public int ChatRoomId;
    }
}
