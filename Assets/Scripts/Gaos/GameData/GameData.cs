using Gaos.Routes.Model.GameDataJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gaos.GameData
{
    public class UserGameDataGet
    {
        private readonly static string CLASS_NAME = typeof(UserGameDataGet).Name;


        public delegate void OnUserGameDataGetComplete(UserGameDataGetResponse response);

        public static IEnumerator Get(int slotId, OnUserGameDataGetComplete onUserGameDataGetComplete)
        {
            const string METHOD_NAME = "Get()";

            Gaos.Routes.Model.GameDataJson.UserGameDataGetRequest request = new Gaos.Routes.Model.GameDataJson.UserGameDataGetRequest();
            request.UserId = Gaos.Context.Authentication.GetUserId();
            request.SlotId = slotId;

            string requestJsonStr = JsonUtility.ToJson(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/gameData/userGameDataGet", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseError)
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting game data");
                onUserGameDataGetComplete(null);

            } 
            else
            {
                Gaos.Routes.Model.GameDataJson.UserGameDataGetResponse response = JsonUtility.FromJson<Gaos.Routes.Model.GameDataJson.UserGameDataGetResponse>(apiCall.ResponseJsonStr);
                onUserGameDataGetComplete(response);
            }

        }
    }

    public class UserGameDataSave
    {
        private readonly static string CLASS_NAME = typeof(UserGameDataSave).Name;


        public delegate void OnUserGameDataSaveComplete(UserGameDataSaveResponse response);

        public static IEnumerator Save(int slotId, Gaos.Routes.Model.GameDataJson.UserGameDataSaveRequest request, OnUserGameDataSaveComplete onUserGameDataSaveComplete)
        {
            const string METHOD_NAME = "Save()";

            request.UserId = Gaos.Context.Authentication.GetUserId();
            request.SlotId = slotId;

            string requestJsonStr = JsonUtility.ToJson(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/gameData/userGameDataSave", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseError)
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error saving game data");
                onUserGameDataSaveComplete(null);

            } 
            else
            {
                Gaos.Routes.Model.GameDataJson.UserGameDataSaveResponse response = JsonUtility.FromJson<Gaos.Routes.Model.GameDataJson.UserGameDataSaveResponse>(apiCall.ResponseJsonStr);
                onUserGameDataSaveComplete(response);
            }

        }
    }

}
