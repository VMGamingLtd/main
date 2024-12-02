namespace Gaos.Routes.Model.FriendJson
{
    public class UserForGetMyFriends 
    {
        public int UserId;
        public string UserName;
    };

    [System.Serializable]
    public class GetMyFriendsResponse
    {
        public bool? IsError;
        public string? ErrorMessage;
        public UserForGetMyFriends[] Users;
    }
}
