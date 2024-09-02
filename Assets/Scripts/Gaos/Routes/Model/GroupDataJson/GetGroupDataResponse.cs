#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GroupDataJson
{
    [System.Serializable]
    public class GetGroupDataResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public string? Id;
        public string? Version;
        public string? GroupDataJson;
    }
}
