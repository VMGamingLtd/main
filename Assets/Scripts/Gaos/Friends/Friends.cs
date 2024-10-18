using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

namespace Gaos.Friends.Friends
{

    public class GetUsersList
    {
        public readonly static string CLASS_NAME = typeof(GetUsersList).Name;

        public static async UniTask<Gaos.Routes.Model.FriendsJson.GetUsersListResponse> CallAsync(string filterUserName, int maxCount)
        {
            const string METHOD_NAME = "api/friends/getUsersList";
            try
            {

                Gaos.Routes.Model.FriendsJson.GetUsersListRequest request = new Gaos.Routes.Model.FriendsJson.GetUsersListRequest();
                request.FilterUserName = filterUserName;
                request.MaxCount = maxCount;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getUsersList", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting users list");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.GetUsersListResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.GetUsersListResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting users list: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }

    }

    public class AddFriend
    {
        public readonly static string CLASS_NAME = typeof(AddFriend).Name;


        public static async UniTask<Gaos.Routes.Model.FriendsJson.AddFriendResponse> CallAsync(int userIdOfFriend)
        {
            const string METHOD_NAME = "api/friends/addFriend";
            try
            {

                Gaos.Routes.Model.FriendsJson.AddFriendRequest request = new Gaos.Routes.Model.FriendsJson.AddFriendRequest();
                request.UserId = userIdOfFriend;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/addFriend", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error adding friend");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.AddFriendResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.AddFriendResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting users list: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }

    }

    public class RevokeFriendRequest
    {
        public readonly static string CLASS_NAME = typeof(RevokeFriendRequest).Name;
        public static async UniTask<Gaos.Routes.Model.FriendsJson.RevokeFriendRequestResponse> CallAsync(int userIdOfFriend)
        {
            const string METHOD_NAME = "api/friends/revokeFriendRequest";
            try
            {
                Gaos.Routes.Model.FriendsJson.RevokeFriendRequestRequest request = new Gaos.Routes.Model.FriendsJson.RevokeFriendRequestRequest();
                request.UserId = userIdOfFriend;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/revokeFriendRequest", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error revoking friend request");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.RevokeFriendRequestResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.RevokeFriendRequestResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting users list: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class RemoveFromGroup
    {
        public readonly static string CLASS_NAME = typeof(RemoveFromGroup).Name;
        public static async UniTask<Gaos.Routes.Model.FriendsJson.RemoveFromGroupResponse> CallAsync(int userIdOfFriend)
        {
            const string METHOD_NAME = "api/friends/removeFromGroup";
            try
            {
                Gaos.Routes.Model.FriendsJson.RemoveFromGroupRequest request = new Gaos.Routes.Model.FriendsJson.RemoveFromGroupRequest();
                request.UserId = userIdOfFriend;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/removeFromGroup", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error removing friend from group");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.RemoveFromGroupResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.RemoveFromGroupResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error removing friend from group: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class LeaveGroup
    {
        public readonly static string CLASS_NAME = typeof(LeaveGroup).Name;
        public static async UniTask<Gaos.Routes.Model.FriendsJson.LeaveGroupResponse> CallAsync()
        {
            const string METHOD_NAME = "api/friends/leaveGroup";
            try
            {
                Gaos.Routes.Model.FriendsJson.LeaveGroupRequest request = new Gaos.Routes.Model.FriendsJson.LeaveGroupRequest();
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/leaveGroup", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error leaving group");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.LeaveGroupResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.LeaveGroupResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error leaving group: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class GetFriendRequests
    {
        public readonly static string CLASS_NAME = typeof(GetFriendRequests).Name;
        public static async UniTask<Gaos.Routes.Model.FriendsJson.GetFriendRequestsResponse> CallAsync(int MaxCount, string OwnerNamePattern)
        {
            const string METHOD_NAME = "api/friends/getFriendRequests";
            try
            {
                Gaos.Routes.Model.FriendsJson.GetFriendRequestsRequest request = new Gaos.Routes.Model.FriendsJson.GetFriendRequestsRequest();
                request.MaxCount = MaxCount;
                request.IsCountOnly = false;
                if (OwnerNamePattern != null && OwnerNamePattern != "")
                {
                    request.OwnerNamePattern = OwnerNamePattern;
                }
                else
                {
                    request.OwnerNamePattern = null;
                }
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getFriendRequests", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting friend requests");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.GetFriendRequestsResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.GetFriendRequestsResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting friend requests: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class GetFriendRequestsCount
    {
        public readonly static string CLASS_NAME = typeof(GetFriendRequestsCount).Name;
        public static async UniTask<int> CallAsync()
        {
            const string METHOD_NAME = "api/friends/getFriendRequests";
            try
            {
                Gaos.Routes.Model.FriendsJson.GetFriendRequestsRequest request = new Gaos.Routes.Model.FriendsJson.GetFriendRequestsRequest();
                request.IsCountOnly = true;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getFriendRequests", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting friend requests");
                    return 0;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.GetFriendRequestsResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.GetFriendRequestsResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting friend requests: {response.ErrorMessage}");
                        return 0;
                    }
                    return (int)response.TotalCount;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class AcceptFriendRequest
    {
        public readonly static string CLASS_NAME = typeof(AcceptFriendRequest).Name;
        public static async UniTask<Gaos.Routes.Model.FriendsJson.AcceptFriendRequestResponse> CallAsync(int groupId)
        {
            const string METHOD_NAME = "api/friends/acceptFriendRequest";
            try
            {
                Gaos.Routes.Model.FriendsJson.AcceptFriendRequestRequest request = new Gaos.Routes.Model.FriendsJson.AcceptFriendRequestRequest();
                request.GroupId = groupId;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/acceptFriendRequest", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error accepting friend request");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.AcceptFriendRequestResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.AcceptFriendRequestResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error accepting friend request: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class RejectFriendRequest
    {
        public readonly static string CLASS_NAME = typeof(RejectFriendRequest).Name;
        public static async UniTask<Gaos.Routes.Model.FriendsJson.RejectFriendRequestResponse> CallAsync(int groupId)
        {
            const string METHOD_NAME = "api/friends/rejectFriendRequest";
            try
            {
                Gaos.Routes.Model.FriendsJson.RejectFriendRequestRequest request = new Gaos.Routes.Model.FriendsJson.RejectFriendRequestRequest();
                request.GroupId = groupId;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/rejectFriendRequest", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error rejecting friend request");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.RejectFriendRequestResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.RejectFriendRequestResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error rejecting friend request: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class GetMyGroup
    {
        public readonly static string CLASS_NAME = typeof(GetMyGroup).Name;

        public static async UniTask<Gaos.Routes.Model.FriendsJson.GetMyGroupResponse> CallAsync()
        {
            const string METHOD_NAME = "api/friends/getMyGroup";
            try
            {
                Gaos.Routes.Model.FriendsJson.GetMyGroupRequest request = new Gaos.Routes.Model.FriendsJson.GetMyGroupRequest();
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getMyGroup", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting my group");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.GetMyGroupResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.GetMyGroupResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting my group: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class GetMyGroupMembers
    {
        public readonly static string CLASS_NAME = typeof(GetMyGroupMembers).Name;

        public static async UniTask<Gaos.Routes.Model.FriendsJson.GetMyGroupMembersResponse> CallAsync(int groupId, int maxCount)
        {
            const string METHOD_NAME = "api/friends/getMyGroupMembers";
            try
            {
                Gaos.Routes.Model.FriendsJson.GetMyGroupMembersRequest request = new Gaos.Routes.Model.FriendsJson.GetMyGroupMembersRequest();
                request.GroupId = groupId;
                request.MaxCount = maxCount;
                request.IsCountOnly = false;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getMyGroupMembers", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting my group members");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.GetMyGroupMembersResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.GetMyGroupMembersResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting my group members: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }

    public class GetMyGroupMembersCount
    {
        public readonly static string CLASS_NAME = typeof(GetMyGroupMembersCount).Name;

        public static async UniTask<int> CallAsync(int groupId)
        {
            const string METHOD_NAME = "api/friends/getMyGroupMembers";
            try
            {
                Gaos.Routes.Model.FriendsJson.GetMyGroupMembersRequest request = new Gaos.Routes.Model.FriendsJson.GetMyGroupMembersRequest();
                request.GroupId = groupId;
                request.IsCountOnly = true;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getMyGroupMembers", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting my friends");
                    return 0;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.GetMyGroupMembersResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.GetMyGroupMembersResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting my friends: {response.ErrorMessage}");
                        return 0;
                    }
                    return (int)response.TotalCount;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: {ex.Message}");
                throw ex;
            }
        }
    }
        

}