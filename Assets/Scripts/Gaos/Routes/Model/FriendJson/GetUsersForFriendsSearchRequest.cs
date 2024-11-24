#pragma warning disable 8632
namespace Gaos.Routes.Model.FriendJson
{
    [System.Serializable]
    public class GetUsersForFriendsSearchRequest
    {
        public int MaxCount;
        public string UserNamePattern;
    }
}
