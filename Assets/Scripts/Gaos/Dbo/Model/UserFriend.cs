#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class UserFriend
    {
        public int Id;
        public int? UserId;
        public User? User;
        public int? FriendUserId;
        public User? FriendUser;
    }
}
