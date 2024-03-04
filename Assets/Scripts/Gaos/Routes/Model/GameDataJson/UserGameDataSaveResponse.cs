﻿#pragma warning disable 8632
namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataSaveResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public string? Id;
        public string? Version;

        public string? GameDataJson;

    }
}
