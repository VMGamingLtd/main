using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

namespace Gaos.Friends.Friends
{
    public class GetUsersList
    {
        public readonly static string CLASS_NAME = typeof(GetUsersList).Name;

        public delegate void OnComplete(Gaos.Routes.Model.FriendsJson.GetUsersListResponse response);

        public static IEnumerator Call(int slotId, OnComplete onComplete)
        {
            const string METHOD_NAME = "Call()";

            Gaos.Routes.Model.FriendsJson.GetUsersListRequest request = new Gaos.Routes.Model.FriendsJson.GetUsersListRequest();

            string requestJsonStr = JsonUtility.ToJson(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/friends/getUsersList", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseError)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting users list");
                onComplete(null);

            } 
            else
            {
                Gaos.Routes.Model.FriendsJson.GetUsersListResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.FriendsJson.GetUsersListResponse>(apiCall.ResponseJsonStr);
                onComplete(response);
            }

        }


    }
}