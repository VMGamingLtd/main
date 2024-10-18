namespace Gaos.Routes.Model.FriendsJson
{

    [System.Serializable]
    public class GetMyGroupMembersRequest
    {
        public int GroupId;
        public  int MaxCount;
        public bool IsCountOnly;
    }
}
