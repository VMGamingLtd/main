using Gaos.Routes.Model.GameDataJson;
using Newtonsoft.Json;
using System.Collections;
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

            UserGameDataGetRequest request = new()
            {
                UserId = Context.Authentication.GetUserId(),
                SlotId = slotId
            };

            string requestJsonStr = JsonConvert.SerializeObject(request);

            Api.ApiCall apiCall = new("api/gameData/userGameDataGet", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseError)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting game data");
                onUserGameDataGetComplete(null);

            }
            else
            {
                UserGameDataGetResponse response = JsonConvert.DeserializeObject<UserGameDataGetResponse>(apiCall.ResponseJsonStr);
                if (response.IsError == true)
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting game data: {response.ErrorMessage}");
                    onUserGameDataGetComplete(null);
                    yield break;
                }
                onUserGameDataGetComplete(response);
            }

        }
    }

    public class UserGameDataSave
    {
        private readonly static string CLASS_NAME = typeof(UserGameDataSave).Name;



        public delegate void OnUserGameDataSaveComplete(UserGameDataSaveResponse response);

        public static IEnumerator Save(int slotId, UserGameDataSaveRequest request, OnUserGameDataSaveComplete onUserGameDataSaveComplete)
        {
            const string METHOD_NAME = "Save()";

            request.UserId = Context.Authentication.GetUserId();
            request.SlotId = slotId;

            string requestJsonStr = JsonConvert.SerializeObject(request);

            Api.ApiCall apiCall = new("api/gameData/userGameDataSave", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseError)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error saving game data");
                onUserGameDataSaveComplete(null);

            }
            else
            {
                UserGameDataSaveResponse response = JsonConvert.DeserializeObject<UserGameDataSaveResponse>(apiCall.ResponseJsonStr);
                if (response.IsError == true)
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error saving game data: {response.ErrorMessage}");
                    onUserGameDataSaveComplete(null);
                    yield break;
                }
                onUserGameDataSaveComplete(response);
            }

        }
    }


}
