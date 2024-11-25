using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Gaos.Friends
{
    public class GetUsersForFriendsSearch
    {
        public readonly static string CLASS_NAME = typeof(GetUsersForFriendsSearch).Name;

        public static async UniTask<Gaos.Routes.Model.FriendJson.GetUsersForFriendsSearchResponse> CallAsync(string filterUserName, int maxCount)
        {
            const string METHOD_NAME = "api/friends/getUsersForFriendsSearch";
            try
            {
                Gaos.Routes.Model.FriendJson.GetUsersForFriendsSearchRequest request = new Gaos.Routes.Model.FriendJson.GetUsersForFriendsSearchRequest();
                request.UserNamePattern = filterUserName;
                request.MaxCount = maxCount;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getUsersForFriendsSearch", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting users list");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendJson.GetUsersForFriendsSearchResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendJson.GetUsersForFriendsSearchResponse>(apiCall.ResponseJsonStr);
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

    public class RequestFriend 
    {
        public readonly static string CLASS_NAME = typeof(RequestFriend).Name; 

        public static async UniTask<Gaos.Routes.Model.FriendJson.RequestFriendResponse> CallAsync(int friendUserId)
        {
            const string METHOD_NAME = "api/friends/requestFriend";
            try
            {
                Gaos.Routes.Model.FriendJson.RequestFriendRequest request = new Gaos.Routes.Model.FriendJson.RequestFriendRequest();
                request.UserId = friendUserId;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/requestFriend", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error requesting friend");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendJson.RequestFriendResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendJson.RequestFriendResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error requesting friend: {response.ErrorMessage}");
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

        public static async UniTask<Gaos.Routes.Model.FriendJson.RemoveFriendResponse> CallAsync(int friendUserId)
        {
            const string METHOD_NAME = "api/friends/removeFriend";
            try
            {
                Gaos.Routes.Model.FriendJson.RemoveFriendRequest request = new Gaos.Routes.Model.FriendJson.RemoveFriendRequest();
                request.UserId = friendUserId;
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
                    Gaos.Routes.Model.FriendJson.RemoveFriendResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendJson.RemoveFriendResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error removing friend: {response.ErrorMessage}");
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
