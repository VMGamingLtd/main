#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataGetResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public string? Id;
        public string? Version;
        public string? GameDataJson;
    }
}
