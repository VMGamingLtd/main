using Gaos.Routes.Model.GameDataJson;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Threading;


namespace Gaos.GameData
{

    public class DocumentVersion
    {
        public string DocId;
        public int DocVersion;
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

        public static void setVersion(int slotId, int docVersion, string docId, string gameDataJson)
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
            requestsQueue.Enqueue(new RequestQueteItem
            {
                slotId = slotId,
                request = request,
                onUserGameDataSaveComplete = onUserGameDataSaveComplete
            });
            yield return null;
        }

        private static RequestQueteItem cloneQueueItem(RequestQueteItem item)
        {
            return new RequestQueteItem
            {
                slotId = item.slotId,
                request = new UserGameDataSaveRequest
                {
                    UserId = item.request.UserId,
                    SlotId = item.request.SlotId,
                    Version = item.request.Version,
                    GameDataJson = item.request.GameDataJson,
                    IsGameDataDiff = item.request.IsGameDataDiff
                },
                onUserGameDataSaveComplete = item.onUserGameDataSaveComplete
            };
        }

        public delegate void OnProcessingSaveQueueFinished();

        private static OnUserGameDataSaveComplete makeOnRequestQueueItemSaveComplete(MonoBehaviour gameObject, RequestQueteItem item, 
                                                                                    bool finished, CancellationToken cancellationToken, 
                                                                                    OnProcessingSaveQueueFinished onProcessingSaveQueueFinished)
        {
            return (UserGameDataSaveResponse response) =>
            {
                var previousVersion = LastGameDataVersion.getVersion(item.slotId);
                if (response != null)
                {
                    if (finished)
                    {
                        Debug.Log($"{CLASS_NAME}: finished processing, last saving of game data finished ok");
                    }
                    LastGameDataVersion.setVersion(item.slotId, response.Version, response.Id, item.request.GameDataJson);
                }
                else
                {
                    if (finished)
                    {
                        Debug.Log($"{CLASS_NAME}: finished processing, ERROR while last saving game data");
                    }
                    // Error saving game data.
                    // Will be trying saving again with same version, since game data is null next save will be full save of game data (not just the json diff).
                    // TODO:
                    // If error is 'version mismatch' such error is irrecovarable and game should be terminated/restarted!!!!!
                    LastGameDataVersion.setVersion(item.slotId, previousVersion.DocVersion, previousVersion.DocId, null);

                }
                item.onUserGameDataSaveComplete(response);
                if (finished)
                {
                    requestsQueue.Clear();
                    onProcessingSaveQueueFinished();
                    return;
                }
                if (requestsQueue.Count > 0)
                {
                    RequestQueteItem item = requestsQueue.Dequeue();
                    while(requestsQueue.Count > 0)
                    {
                        item = requestsQueue.Dequeue();
                    }
                    var onUserGameDataSaveComplete = makeOnRequestQueueItemSaveComplete(gameObject, cloneQueueItem(item), finished, cancellationToken, onProcessingSaveQueueFinished);
                    if (Environment.Environment.GetEnvironment()["IS_SEND_GAME_DATA_DIFF"] == "true") 
                    {
                        if (previousVersion != null && previousVersion.GameDataJson != null)
                        {
                            JObject objA = JObject.Parse(previousVersion.GameDataJson);
                            JObject objB = JObject.Parse(item.request.GameDataJson);
                            var diff = jsondiff.Difference.CompareValues(objA, objB);
                            if (Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true" && Environment.Environment.GetEnvironment()["IS_DEBUG_GAME_DATA"] == "true")
                            {
                                var strA = JsonConvert.SerializeObject(objA, jsonSerializerSettings);
                                var strB = JsonConvert.SerializeObject(objB, jsonSerializerSettings);
                                var strDiff = JsonConvert.SerializeObject(diff, jsonSerializerSettings);
                                Debug.Log($"@@@@@@@@@@@@@@@@@@@@ gamedata: objaA: {strA.Length}");
                                Debug.Log(strA);
                                Debug.Log($"@@@@@@@@@@@@@@@@@@@@ gamedata: objaB: {strB.Length}");
                                Debug.Log(strB);
                                Debug.Log($"@@@@@@@@@@@@@@@@@@@@ gamedata: diff: {strDiff.Length}");
                                Debug.Log(strDiff);
                            }
                            if (Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true" && Environment.Environment.GetEnvironment()["IS_DEBUG_SEND_GAMEDATA_BASE"] == "true")
                            {
                                item.request.GameDataDiffBase = previousVersion.GameDataJson;
                            }
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
                    gameObject.StartCoroutine(ProcessSendQueue(gameObject, cancellationToken, onProcessingSaveQueueFinished));
                }
            };
        }

        public static IEnumerator ProcessSendQueue(MonoBehaviour gameObject, CancellationToken cancellationToken, OnProcessingSaveQueueFinished onProcessingSaveQueueFinished)
        {
            if (requestsQueue.Count > 0)
            {
                RequestQueteItem item = requestsQueue.Dequeue();
                while(requestsQueue.Count > 0)
                {
                    item = requestsQueue.Dequeue();
                }
                var onUserGameDataSaveComplete = makeOnRequestQueueItemSaveComplete(gameObject, cloneQueueItem(item), cancellationToken.IsCancellationRequested, cancellationToken, onProcessingSaveQueueFinished);

                if (Environment.Environment.GetEnvironment()["IS_SEND_GAME_DATA_DIFF"] == "true") 
                {
                    var previousVersion = LastGameDataVersion.getVersion(item.slotId);
                    if (previousVersion != null && previousVersion.GameDataJson != null)
                    {
                        JObject objA = JObject.Parse(previousVersion.GameDataJson);
                        JObject objB = JObject.Parse(item.request.GameDataJson);
                        var diff = jsondiff.Difference.CompareValues(objA, objB);
                        if (Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true" && Environment.Environment.GetEnvironment()["IS_DEBUG_GAME_DATA"] == "true")
                        {
                            if (Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true" && Environment.Environment.GetEnvironment()["IS_DEBUG_GAME_DATA"] == "true")
                            {
                                var strA = JsonConvert.SerializeObject(objA, jsonSerializerSettings);
                                var strB = JsonConvert.SerializeObject(objB, jsonSerializerSettings);
                                var strDiff = JsonConvert.SerializeObject(diff, jsonSerializerSettings);
                                Debug.Log($"@@@@@@@@@@@@@@@@@@@@ gamedata: objaA: {strA.Length}");
                                Debug.Log(strA);
                                Debug.Log($"@@@@@@@@@@@@@@@@@@@@ gamedata: objaB: {strB.Length}");
                                Debug.Log(strB);
                                Debug.Log($"@@@@@@@@@@@@@@@@@@@@ gamedata: diff: {strDiff.Length}");
                                Debug.Log(strDiff);
                            }
                        }
                        if (Environment.Environment.GetEnvironment()["IS_DEBUG"] == "true" && Environment.Environment.GetEnvironment()["IS_DEBUG_SEND_GAMEDATA_BASE"] == "true")
                        {
                            item.request.GameDataDiffBase = previousVersion.GameDataJson;
                        }
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
                gameObject.StartCoroutine(ProcessSendQueue(gameObject, cancellationToken, onProcessingSaveQueueFinished));
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
                    if (response.ErrorKind == UserGameDataSaveErrorKind.VersionMismatchError)
                    {
                        // There are 2 cases when this error can happen:
                        // 1) there was server communication interruption causing the game to miss the server reply with the updated version
                        // 2) concurrency problem - same game is being played in more then one browser 
                        // We could have recovered from 1) (by forcing save) but there's no way to detrmine with 100% certainty if it was 1) or 2),
                        // therefore we rather report fatal error and terminate the game to prevent game data being overwritten by old version
                        // from other browser.
                        Debug.LogError($"Fatal Error: game data out of sync, quitting the game...");
                        Application.Quit();
                    }
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
                Gaos.GameData.LastGameDataVersion.setVersion(slotId, response.MongoDocumentVersion, response.MongoDocumentId, null);
                onEnsureNewSlotComplete(response);
            }

        }
    }

    public class DeleteSlot
    {
        private readonly static string CLASS_NAME = typeof(DeleteSlot).Name;

        public delegate void OnDeleteSlotComplete(DeleteSlotResponse response);

        public static IEnumerator DeleteSlotCall(int slotId, OnDeleteSlotComplete onDeleteSlotComplete)
        {
            const string METHOD_NAME = "DeleteSlotCall()";
            Debug.Log("@@@@@@@@@@@@@@@@@@@@@ cp 3000: DeleteSlotCall()");

            DeleteSlotRequest request = new DeleteSlotRequest();
            request.UserId = Context.Authentication.GetUserId();
            request.SlotId = slotId;

            string requestJsonStr = JsonConvert.SerializeObject(request);

            Api.ApiCall apiCall = new("api/gameData/deleteGameSlot", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseError)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error deleting slot");
                onDeleteSlotComplete(null);

            }
            else
            {
                DeleteSlotResponse response = JsonConvert.DeserializeObject<DeleteSlotResponse>(apiCall.ResponseJsonStr);
                if (response.IsError == true)
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error deleting slot: {response.ErrorMessage}");
                    onDeleteSlotComplete(null);
                    yield break;
                }
                onDeleteSlotComplete(response);
            }

        }
    }


}
