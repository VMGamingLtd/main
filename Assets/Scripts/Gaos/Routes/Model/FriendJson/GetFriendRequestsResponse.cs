#pragma warning disable 8632
namespace Gaos.Routes.Model.FriendJson
{
    public class FriendRequest 
    {
        public int UserId;
        public string UserName;
    };

    [System.Serializable]
    public class GetFriendRequestsResponse
    {
        public bool? IsError;
        public string? ErrorMessage;
        public FriendRequest[] FriendRequest;
    }
}
