#pragma warning disable 8632
namespace Gaos.Routes.Model.GroupJson
{
    [System.Serializable]
    public class GroupMembersListUser
    {
        public int UserId;
        public string? UserName;
    }

    [System.Serializable]
    public class GetMyGroupMembersResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public GroupMembersListUser[]? Users;
        public int? TotalCount;
    }
}
