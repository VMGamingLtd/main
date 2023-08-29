﻿#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataSaveRequest
    {
        public int  UserId;
        public int  SlotId;
        public string? GameDataJson;
    }
}
