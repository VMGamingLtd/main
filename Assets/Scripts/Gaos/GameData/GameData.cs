using Gaos.Routes.Model.GameDataJson;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;


namespace Gaos.GameData
{
    public class LastGameDataVersion
    {
        public static string Version = "0";
    }

    public class UserGameDataGet
    {
        private readonly static string CLASS_NAME = typeof(UserGameDataGet).Name;


        public delegate void OnUserGameDataGetComplete(UserGameDataGetResponse response);

        public static string Version = "0";

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
                LastGameDataVersion.Version = response.Version;
                Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 1000: read, version ${response.Version}");
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
            request.Version = UserGameDataGet.Version;

            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 1100: saving, version ${request.Version}");

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
                LastGameDataVersion.Version = response.Version;
                Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 1200: save response, version ${response.Version}");
                onUserGameDataSaveComplete(response);
            }

        }
    }

    public class EnsureNewSlot
    {
        private readonly static string CLASS_NAME = typeof(EnsureNewSlot).Name;



        public delegate void OnEnsureNewSlotComplete(EnsureNewSlotResponse response);

        public static IEnumerator EnsureNewSlotExists(int slotId, OnEnsureNewSlotComplete onEnsureNewSlotComplete)
        {
            const string METHOD_NAME = "Save()";

            EnsureNewSlotRequest request = new EnsureNewSlotRequest();
            request.UserId = Context.Authentication.GetUserId();
            request.SlotId = slotId;

            string requestJsonStr = JsonConvert.SerializeObject(request);

            Api.ApiCall apiCall = new("api/gameData/ensureNewSlot", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseError)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error saving game data");
                onEnsureNewSlotComplete(null);

            }
            else
            {
                EnsureNewSlotResponse response = JsonConvert.DeserializeObject<EnsureNewSlotResponse>(apiCall.ResponseJsonStr);
                if (response.IsError == true)
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error ensuring new slot exists: {response.ErrorMessage}");
                    onEnsureNewSlotComplete(null);
                    yield break;
                }
                onEnsureNewSlotComplete(response);
            }

        }
    }


}
