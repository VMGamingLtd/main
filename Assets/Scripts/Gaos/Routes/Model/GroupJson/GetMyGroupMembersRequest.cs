namespace Gaos.Routes.Model.GroupJson
{

    [System.Serializable]
    public class GetMyGroupMembersRequest
    {
        public int GroupId;
        public  int MaxCount;
        public bool IsCountOnly;
    }
}
