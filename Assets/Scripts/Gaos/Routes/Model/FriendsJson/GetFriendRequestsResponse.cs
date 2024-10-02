#pragma warning disable 8632
namespace Gaos.Routes.Model.FriendsJson
{
    public class GetFriendRequestsResponseListItem
    {
        public int GroupId;
        public int GroupOwnerId;
        public string? GroupOwnerName;
    }
    [System.Serializable]
    public class GetFriendRequestsResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public System.Collections.Generic.List<GetFriendRequestsResponseListItem>? FriendRequests;
        public int? TotalCount;
    }
}
