using Cysharp.Threading.Tasks;
using Gaos.GroupData.GroupData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Gaos.GroupData.GroupDataSaver
{
    public class GroupDataSaver
    {
        public readonly static string CLASS_NAME = typeof(GroupDataSaver).Name;

        private static Queue<string> requestsQueue = new Queue<string>();

        private static JsonSerializerSettings jsonSerializerSettings = jsondiff.Difference.GetJsonSerializerSettings();

        private enum SaveStatus
        {
            Success,
            VersionMismatch,
            InternalError
        }

        public static void Save(string groupDataJson)
        {
            requestsQueue.Enqueue(groupDataJson);
        }

        private static async UniTask<SaveStatus> SaveData(string currentData, long currentDataVersion, string newData, int slotId)
        {
            JObject objA = JObject.Parse(currentData);
            JObject objB = JObject.Parse(newData);
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

        public static async UniTaskVoid StartProcessing(CancellationToken cancellationToken)
        {
            const string METHOD_NAME = "StartProcessing()";
            const int slotId = 1;

            // fetch the ewest version of the group data
            Gaos.Routes.Model.GroupDataJson.GetGroupDataResponse currentGroupData = await GetGroupData.CallAsync(-1, slotId);

            while (!cancellationToken.IsCancellationRequested)
            {
                if (requestsQueue.Count > 1)
                {
                    string groupDataJson = requestsQueue.Dequeue();
                    var saveStatus = await SaveData(currentGroupData.GroupDataJson, currentGroupData.Version, groupDataJson, slotId);
                    int retries = 0;
                    while (saveStatus == SaveStatus.InternalError && retries < 5)
                    {
                        saveStatus = await SaveData(currentGroupData.GroupDataJson, currentGroupData.Version, groupDataJson, slotId);
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
