using Gaos.Routes.Model.GameDataJson;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gaos.Environment;
using UnityEditor.Compilation;
using Newtonsoft.Json.Linq;


namespace Gaos.GameData
{

    public class DocumentVersion
    {
        public string DocId;
        public string DocVersion;
        public string GameDataJson; // game data at version DocVersion
    }

    public class LastGameDataVersion
    {
        // dictionary of slotId to version
        private static Dictionary<int, DocumentVersion> slotToVersion = new Dictionary<int, DocumentVersion>();

        public static DocumentVersion getVersion(int slotId)
        {
            if (!slotToVersion.ContainsKey(slotId))
            {
                return null;
            }
            return slotToVersion[slotId];
        }

        public static void setVersion(int slotId, string docVersion, string docId, string gameDataJson)
        {
            slotToVersion[slotId] = new DocumentVersion { DocId = docId, DocVersion = docVersion, GameDataJson = gameDataJson };
        }
    }

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
                if (Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true" && Environment.Environment.GetEnvironment()["IS_DEBUG_GAME_DATA"] == "true")
                {
                    Debug.Log($"game data: read version {response.Version}, in {apiCall.GetDurationMilliseconds()} ms");
                }
                LastGameDataVersion.setVersion(slotId, response.Version, response.Id, response.GameDataJson);
                onUserGameDataGetComplete(response);
            }

        }
    }

    public class UserGameDataSaveOld
    {
        private readonly static string CLASS_NAME = typeof(UserGameDataSave).Name;

        public delegate void OnUserGameDataSaveComplete(UserGameDataSaveResponse response);

        public static IEnumerator Save(int slotId, UserGameDataSaveRequest request, OnUserGameDataSaveComplete onUserGameDataSaveComplete)
        {
            const string METHOD_NAME = "Save()";

            request.UserId = Context.Authentication.GetUserId();
            request.SlotId = slotId;
            request.Version = LastGameDataVersion.getVersion(slotId).DocVersion;

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
                if (Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true" && Environment.Environment.GetEnvironment()["IS_DEBUG_GAME_DATA"] == "true")
                {
                    Debug.Log($"game data: saved version {response.Version}, in {apiCall.GetDurationMilliseconds()} ms");
                }
                onUserGameDataSaveComplete(response);
            }

        }
    }

    public class UserGameDataSave
    {

        private readonly static string CLASS_NAME = typeof(UserGameDataSave).Name;

        public delegate void OnUserGameDataSaveComplete(UserGameDataSaveResponse response);

        class RequestQueteItem
        {
            public int slotId;
            public UserGameDataSaveRequest request;
            public OnUserGameDataSaveComplete onUserGameDataSaveComplete;
        }
        private static Queue<RequestQueteItem> requestsQueue = new Queue<RequestQueteItem>();

        private static JsonSerializerSettings jsonSerializerSettings = jsondiff.Difference.GetJsonSerializerSettings();

        public static IEnumerator Save(int slotId, UserGameDataSaveRequest request, OnUserGameDataSaveComplete onUserGameDataSaveComplete)
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp  3000: enQueue(): {request.GameDataJson}");
            requestsQueue.Enqueue(new RequestQueteItem
            {
                slotId = slotId,
                request = request,
                onUserGameDataSaveComplete = onUserGameDataSaveComplete
            });
            yield return null;
        }


        private static OnUserGameDataSaveComplete makeOnRequestQueueItemSaveComplete(MonoBehaviour gameObject, RequestQueteItem item)
        {
            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp  2250: makeOnRequestQueueItemSaveComplete(): {item.request.GameDataJson}");
            return (UserGameDataSaveResponse response) =>
            {
                Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp  2300: makeOnRequestQueueItemSaveComplete(): {item.request.GameDataJson}");
                var previousVersion = LastGameDataVersion.getVersion(item.slotId);
                LastGameDataVersion.setVersion(item.slotId, response.Version, response.Id, item.request.GameDataJson);
                item.onUserGameDataSaveComplete(response);
                if (requestsQueue.Count > 0)
                {
                    RequestQueteItem item = requestsQueue.Dequeue();
                    while(requestsQueue.Count > 0)
                    {
                        item = requestsQueue.Dequeue();
                    }
                    var onUserGameDataSaveComplete = makeOnRequestQueueItemSaveComplete(gameObject, item);
                    if (Environment.Environment.GetEnvironment()["IS_SEND_GAME_DATA_DIFF"] == "true") 
                    {
                        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp  4100");
                        if (previousVersion != null && previousVersion.GameDataJson != null)
                        {
                            Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp  4200");
                            JObject objA = JObject.Parse(previousVersion.GameDataJson);
                            JObject objB = JObject.Parse(item.request.GameDataJson);
                            var diff = jsondiff.Difference.CompareValues(objA, objB);
                            var diffJson = JsonConvert.SerializeObject(diff, jsonSerializerSettings);
                            item.request.GameDataJson = diffJson;
                            item.request.IsGameDataDiff = true;
                        } else {
                            item.request.IsGameDataDiff = false;
                        }
                    }
                    else
                    {
                        item.request.IsGameDataDiff = false;
                    }
                    gameObject.StartCoroutine(Save1(item.slotId, item.request, onUserGameDataSaveComplete));
                }
                else
                {
                    gameObject.StartCoroutine(ProcessSendQueue(gameObject));
                }
            };
        }

        private static bool isProcessingSendQueueRunning = false;

        public static IEnumerator ProcessSendQueue(MonoBehaviour gameObject)
        {
            if (requestsQueue.Count > 0)
            {
                RequestQueteItem item = requestsQueue.Dequeue();
                while(requestsQueue.Count > 0)
                {
                    item = requestsQueue.Dequeue();
                }
                var onUserGameDataSaveComplete = makeOnRequestQueueItemSaveComplete(gameObject, item);

                if (Environment.Environment.GetEnvironment()["IS_SEND_GAME_DATA_DIFF"] == "true") 
                {
                    var previousVersion = LastGameDataVersion.getVersion(item.slotId);
                    if (previousVersion != null && previousVersion.GameDataJson != null)
                    {
                        JObject objA = JObject.Parse(previousVersion.GameDataJson);
                        JObject objB = JObject.Parse(item.request.GameDataJson);
                        var diff = jsondiff.Difference.CompareValues(objA, objB);
                        var diffJson = JsonConvert.SerializeObject(diff, jsonSerializerSettings);
                        item.request.GameDataJson = diffJson;
                        item.request.IsGameDataDiff = true;
                    } else {
                        item.request.IsGameDataDiff = false;
                    }
                }
                else
                {
                    item.request.IsGameDataDiff = false;
                }
                gameObject.StartCoroutine(Save1(item.slotId, item.request, onUserGameDataSaveComplete));
            }
            else
            {
                // sleep for 1 second before checking again
                yield return new WaitForSeconds(1);
                gameObject.StartCoroutine(ProcessSendQueue(gameObject));
            }
        }


        public static IEnumerator Save1(int slotId, UserGameDataSaveRequest request, OnUserGameDataSaveComplete onUserGameDataSaveComplete)
        {
            const string METHOD_NAME = "Save()";

            request.UserId = Context.Authentication.GetUserId();
            request.SlotId = slotId;
            request.Version = LastGameDataVersion.getVersion(slotId).DocVersion;


            if (Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true" && Environment.Environment.GetEnvironment()["IS_DEBUG_GAME_DATA"] == "true")
            {
                Debug.Log($"game data: saving version {request.Version}");
            }

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
                if (Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true" && Environment.Environment.GetEnvironment()["IS_DEBUG_GAME_DATA"] == "true")
                {
                    Debug.Log($"game data: saved version {response.Version}, in {apiCall.GetDurationMilliseconds()} ms");
                }
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
                Gaos.GameData.LastGameDataVersion.setVersion(slotId, response.MongoDocumentVersion, response.MongoDocumentVersion, null);
                onEnsureNewSlotComplete(response);
            }

        }
    }


}
