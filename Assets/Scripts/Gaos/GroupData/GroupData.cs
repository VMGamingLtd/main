using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Gaos.GroupData.GroupData
{
    public class GetGroupData
    {
        public readonly static string CLASS_NAME = typeof(GetGroupData).Name;

        public static async UniTask<Gaos.Routes.Model.GroupDataJson.GetGroupDataResponse> CallAsync(long version = -1, int slotId = 1)
        {
            const string METHOD_NAME = "/api/groupData/getGroupData";
            try
            {

                Gaos.Routes.Model.GroupDataJson.GetGroupDataGetRequest request = new Gaos.Routes.Model.GroupDataJson.GetGroupDataGetRequest();
                request.SlotId = slotId;
                request.Version = version;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/groupData/getGroupData", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting group data");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.GroupDataJson.GetGroupDataResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.GroupDataJson.GetGroupDataResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting group dasa: {response.ErrorMessage}");
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

    public class SaveGroupData
    {
        public readonly static string CLASS_NAME = typeof(SaveGroupData).Name;


        public static async UniTask<Gaos.Routes.Model.GroupDataJson.SaveGroupDataResponse> CallAsync(string groupDataJson, bool isJsonDiff, long version, int slotId = 1)
        {
            const string METHOD_NAME = "/api/groupData/saveGroupData";
            try
            {
                Gaos.Routes.Model.GroupDataJson.SaveGroupDataRequest request = new Gaos.Routes.Model.GroupDataJson.SaveGroupDataRequest();
                request.SlotId = 1;
                request.GroupDataJson = groupDataJson;
                request.IsJsonDiff = isJsonDiff;
                request.Version = version;

                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/groupData/saveGroupData", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error saving group data");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.GroupDataJson.SaveGroupDataResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.GroupDataJson.SaveGroupDataResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error saving group data: {response.ErrorMessage}");
                        return response;
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

    public class GetOwnersData
    {
        public readonly static string CLASS_NAME = typeof(GetOwnersData).Name;

        public static async UniTask<Gaos.Routes.Model.GroupDataJson.GetOwnersDataDataResponse> CallAsync()
        {
            const string METHOD_NAME = "/api/groupData/getOwnersData";
            try
            {
                Gaos.Routes.Model.GroupDataJson.GetOwnersDataRequest request = new Gaos.Routes.Model.GroupDataJson.GetOwnersDataRequest();
                request.SlotId = 1;
                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api/groupData/getOwnersData", requestJsonStr);
                await apiCall.CallAsync();
                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting owners data");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.GroupDataJson.GetOwnersDataDataResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.GroupDataJson.GetOwnersDataDataResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting owners data: {response.ErrorMessage}");
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
