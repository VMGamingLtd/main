#pragma warning disable 8632
namespace Gaos.Routes.Model.FriendJson
{
    public class UserForFriendsSearch 
    {
        public int UserId;
        public string UserName;
        public bool IsFriend;
        public bool IsFriendRequest;
    };

    [System.Serializable]
    public class GetUsersForFriendsSearchResponse
    {
        public bool? IsError;
        public string? ErrorMessage;
        public UserForFriendsSearch[] Users;
    }
}
