﻿#pragma warning disable 8632
namespace Gaos.Routes.Model.FriendsJson
{
    [System.Serializable]
    public class GroupMembersListUser
    {
        public int Id;
        public string? Name;
        public bool? IsOwner;
    }

    [System.Serializable]
    public class GetMyFriendsResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public GroupMembersListUser[]? Users;
    }
}
