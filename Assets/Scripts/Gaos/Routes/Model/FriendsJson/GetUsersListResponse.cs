#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.FriendsJson
{
    [System.Serializable]
    public class UsersListUser
    {
        public int Id;
        public string? Name;
    }

    [System.Serializable]
    public class GetUsersListResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public UsersListUser[]? Users;
    }
}
