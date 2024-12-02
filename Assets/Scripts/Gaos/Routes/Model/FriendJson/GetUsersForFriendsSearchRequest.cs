#pragma warning disable 8632
namespace Gaos.Routes.Model.FriendJson
{
    [System.Serializable]
    public class GetUsersForFriendsSearchRequest
    {
        public string UserNamePattern;
        public int MaxCount;
    }
}
