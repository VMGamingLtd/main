using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

namespace Gaos.GroupData1
{
    public class GetCredits
    {
        public readonly static string CLASS_NAME = typeof(GetCredits).Name;

        public static async UniTask<Gaos.Routes.Model.GroupData1Json.GetCreditsResponse> CallAsync(int groupId, int userId)
        {
            const string METHOD_NAME = "api/groupData1/getCredits";
            try
            {
                Gaos.Routes.Model.GroupData1Json.GetCreditsRequest request = new Gaos.Routes.Model.GroupData1Json.GetCreditsRequest();
                request.GroupId = groupId;
                request.UserId = userId;


                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall(METHOD_NAME, requestJsonStr);
                await apiCall.CallAsync();

                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting credits");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.GroupData1Json.GetCreditsResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.GroupData1Json.GetCreditsResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting credits: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }

            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error getting credits: {e.Message}");
                return null;
            }
        }
    }

    public class AddMyCredits
    {
        public readonly static string CLASS_NAME = typeof(AddMyCredits).Name;

        public static async UniTask<Gaos.Routes.Model.GroupData1Json.AddCreditsResponse> CallAsync(float credits)
        {
            const string METHOD_NAME = "api/groupData1/addMyCredits";
            try
            {
                Gaos.Routes.Model.GroupData1Json.AddCreditsRequest request = new Gaos.Routes.Model.GroupData1Json.AddCreditsRequest();
                request.Credits = credits;

                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall(METHOD_NAME, requestJsonStr);
                await apiCall.CallAsync();

                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error adding my credits");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.GroupData1Json.AddCreditsResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.GroupData1Json.AddCreditsResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error adding my credits: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }

            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error adding my credits: {e.Message}");
                return null;
            }
        }
    }

    public class ResetMyCredits
    {
        public readonly static string CLASS_NAME = typeof(ResetMyCredits).Name;

        public static async UniTask<Gaos.Routes.Model.GroupData1Json.ResetCreditsResponse> CallAsync(float credits)
        {
            const string METHOD_NAME = "api/groupData1/resetMyCredits";
            try
            {
                Gaos.Routes.Model.GroupData1Json.ResetCreditsRequest request = new Gaos.Routes.Model.GroupData1Json.ResetCreditsRequest();
                request.Credits = credits;

                string requestJsonStr = JsonConvert.SerializeObject(request);
                Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall(METHOD_NAME, requestJsonStr);
                await apiCall.CallAsync();

                if (apiCall.IsResponseError)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error resetting my credits");
                    return null;
                }
                else
                {
                    Gaos.Routes.Model.GroupData1Json.ResetCreditsResponse response = JsonConvert.DeserializeObject<Gaos.Routes.Model.GroupData1Json.ResetCreditsResponse>(apiCall.ResponseJsonStr);
                    if (response.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error resetting my credits: {response.ErrorMessage}");
                        return null;
                    }
                    return response;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: error resetting my credits: {e.Message}");
                return null;
            }
        }
    }


}