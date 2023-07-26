using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

namespace Gaos.Friends.Friends
{
    public class GetUsersList
    {
        public readonly static string CLASS_NAME = typeof(GetUsersList).Name;

        public delegate void OnComplete(Gaos.Routes.Model.FriendsJson.GetUsersListResponse response);

        public static IEnumerator Call(OnComplete onComplete)
        {
            const string METHOD_NAME = "Call()";

            Gaos.Routes.Model.FriendsJson.GetUsersListRequest request = new Gaos.Routes.Model.FriendsJson.GetUsersListRequest();

            string requestJsonStr = JsonUtility.ToJson(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getUsersList", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseError)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting users list");
                onComplete(null);

            } 
            else
            {
                Gaos.Routes.Model.FriendsJson.GetUsersListResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.GetUsersListResponse>(apiCall.ResponseJsonStr);
                if (response.IsError == true)
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting users list: {response.ErrorMessage}");
                    onComplete(null);
                    yield break;
                }
                onComplete(response);
            }
        }

        public static async UniTask<Gaos.Routes.Model.FriendsJson.GetUsersListResponse> CallAsync()
        {
            const string METHOD_NAME = "CallAsync()";
            try
            {

                Gaos.Routes.Model.FriendsJson.GetUsersListRequest request = new Gaos.Routes.Model.FriendsJson.GetUsersListRequest();
                string requestJsonStr = JsonUtility.ToJson(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getUsersList", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogError($"ERROR: error getting users list");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.FriendsJson.GetUsersListResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.GetUsersListResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting users list: {response.ErrorMessage}");
                        throw new System.Exception($"ERROR: error getting users list: {response.ErrorMessage}");
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