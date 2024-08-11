#pragma warning disable 8632
namespace Gaos.Routes.Model.FriendsJson
{
    [System.Serializable]
    public class AddFriendRequestExistsResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public bool? Exists;
    }
}
