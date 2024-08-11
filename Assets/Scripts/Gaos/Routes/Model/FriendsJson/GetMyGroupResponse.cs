#pragma warning disable 8632
namespace Gaos.Routes.Model.FriendsJson
{
    [System.Serializable]
    public class GetMyGroupResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public bool IsGroupOwner;
        public bool IsGroupMember;
        public int GroupId;
        public int GroupOwnerId;
        public string GroupOwnerName;
    }
}
