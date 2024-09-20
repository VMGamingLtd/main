using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gaos.GroupData
{
    public class CurrentGroupData
    {
        public int slotId;
        public long version;
        public string GroupDataStr;
        public GroupDataModel GroupDataJson;

    }

    public class GroupDataSaver
    {
        public readonly static string CLASS_NAME = typeof(GroupDataSaver).Name;

        private class RequestQueueItem
        {
            public MutateGroupDataFn mutateFn;
            public System.Object mutateContext;
        }

        private Queue<RequestQueueItem> requestsQueue = new Queue<RequestQueueItem>();

        private static JsonSerializerSettings jsonSerializerSettings = jsondiff.Difference.GetJsonSerializerSettings();

        private enum SaveStatus
        {
            Success,
            VersionMismatch,
            InternalError
        }

        CurrentGroupData currentGroupData;

        public delegate GroupDataModel MutateGroupDataFn(GroupDataModel groupData, System.Object context);

        public void Save(MutateGroupDataFn mutateFn, System.Object mutateContext = null)
        {
            requestsQueue.Enqueue(new RequestQueueItem
            {
                mutateFn = mutateFn,
                mutateContext = mutateContext
            });
        }

        private async UniTask<SaveStatus> SaveData(GroupDataModel currentGroupData, long currentDataVersion,  MutateGroupDataFn mutateGroupDataFn, System.Object mutateContext, int slotId)
        {
            GroupDataModel newData = mutateGroupDataFn(currentGroupData, mutateContext);

            string currentDataStr = JsonConvert.SerializeObject(currentGroupData);
            string newDataStr = JsonConvert.SerializeObject(newData);


            JObject objA = JObject.Parse(currentDataStr);
            JObject objB = JObject.Parse(newDataStr);
            var diff = jsondiff.Difference.CompareValues(objA, objB);

            Gaos.Routes.Model.GroupDataJson.SaveGroupDataResponse saveResponse = await SaveGroupData.CallAsync(diff.ToString(), true, currentDataVersion, slotId);
            if (saveResponse != null) 
            {
                if (!(bool)saveResponse.IsError)
                {
                    return SaveStatus.Success;
                }
                else
                {
                    if (saveResponse.ErrorMessage == "version mismatch" )
                    {
                        // on mismatch is irecoverable, we need to fetch the newest version of the group data
                        return SaveStatus.VersionMismatch;
                    }
                    else
                    {
                        // internal error, perhaps we may recover from this
                        return SaveStatus.InternalError;
                    }

                }
            }
            else
            {
                return SaveStatus.InternalError;

            }
        }

        public async UniTaskVoid StartProcessing(CancellationToken cancellationToken)
        {
            const string METHOD_NAME = "StartProcessing()";
            const int slotId = 1;

            // fetch the newest version of the group data
            var gdResponse = await GetGroupData.CallAsync(-1, slotId);
            currentGroupData = new CurrentGroupData
            {
                slotId = slotId,
                version = gdResponse.Version,
                GroupDataStr = gdResponse.GroupDataJson,
                GroupDataJson = JsonConvert.DeserializeObject<GroupDataModel>(gdResponse.GroupDataJson)
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                if (requestsQueue.Count > 1)
                {
                    var queueItem = requestsQueue.Peek();
                    MutateGroupDataFn mutateFn = queueItem.mutateFn;
                    System.Object mutateContext = queueItem.mutateContext;

                    var saveStatus = await SaveData(currentGroupData.GroupDataJson, currentGroupData.version, mutateFn, mutateContext, slotId) ;
                    int retries = 0;
                    while (saveStatus != SaveStatus.Success && retries < 5)
                    {
                        if (saveStatus == SaveStatus.VersionMismatch)
                        {
                            // fetch the newest version of the group data
                            gdResponse = await GetGroupData.CallAsync(-1, slotId);
                            currentGroupData = new CurrentGroupData
                            {
                                slotId = slotId,
                                version = gdResponse.Version,
                                GroupDataStr = gdResponse.GroupDataJson,
                                GroupDataJson = JsonConvert.DeserializeObject<GroupDataModel>(gdResponse.GroupDataJson)
                            };
                        }

                        saveStatus = await SaveData(currentGroupData.GroupDataJson, currentGroupData.version, mutateFn, mutateContext, slotId) ;
                        await UniTask.Delay(10);
                    }
                    if (saveStatus != SaveStatus.Success)
                    {
                        Debug.LogError($"{CLASS_NAME}:{METHOD_NAME} Fatal Error: could nnot save group data (error: {saveStatus}), quitting the game...");
                        Application.Quit();
                    }
                }
                else
                {
                    await UniTask.Delay(10);
                }
            }
        }
    }
}
