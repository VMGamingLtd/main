﻿#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.ChatRoomJson
{
    [System.Serializable]
    public class AddMemberRequest
    {
        public int ChatRoomId;
        public int UserId;
    }
}
