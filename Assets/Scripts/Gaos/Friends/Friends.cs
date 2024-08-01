using UnityEngine;
using System.Collections;
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
}