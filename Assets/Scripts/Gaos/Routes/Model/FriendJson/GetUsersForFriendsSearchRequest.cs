#pragma warning disable 8632
namespace gaos.Routes.Model.FriendJson
{
    [System.Serializable]
    public class GetUsersForFriendsSearchRequest
    {
        public int MaxCount;
        public string UserNamePattern;
    }
}
