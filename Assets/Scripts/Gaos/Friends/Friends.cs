using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

namespace Gaos.Friends.Friends
{
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

    public class RemoveFriend
    {
        public readonly static string CLASS_NAME = typeof(RemoveFriend).Name;
        public static async UniTask<Gaos.Routes.Model.FriendsJson.RemoveFriendResponse> CallAsync(int userIdOfFriend)
        {
            const string METHOD_NAME = "api/friends/removeFriend";
            try
            {
                Gaos.Routes.Model.FriendsJson.RemoveFriendRequest request = new Gaos.Routes.Model.FriendsJson.RemoveFriendRequest();
                request.UserId = userIdOfFriend;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/removeFriend", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error removing friend");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.RemoveFriendResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.RemoveFriendResponse>(apiCall.ResponseJsonStr);
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
        

}