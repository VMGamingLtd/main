#pragma warning disable 8632
namespace Gaos.Routes.Model.GroupDataJson
{
    [System.Serializable]
    public class GetGroupDataGetRequest
    {
        public int UserId;
        public int SlotId;

        public string? Version;
    }
}
