#pragma warning disable 8632
namespace Gaos.Routes.Model.FriendsJson
{
    [System.Serializable]
    public class GroupMembersListUser
    {
        public int UserId;
        public string? UserName;
    }

    [System.Serializable]
    public class GetMyFriendsResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public GroupMembersListUser[]? Users;
    }
}
