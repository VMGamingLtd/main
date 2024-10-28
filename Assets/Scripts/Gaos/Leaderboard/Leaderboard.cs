#pragma warning disable 8632

using Gaos.Dbo.Model;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;

namespace Gaos.Leaderboard
{
    public class Leaderboard
    {
        private readonly static string CLASS_NAME = typeof(Leaderboard).Name;

        public static IEnumerator UpdateLeaderboardData(LeaderboardData leaderboardData)
        {
            const string METHOD_NAME = "UpdateLeaderboardData()";

            string requestJsonStr = JsonConvert.SerializeObject(leaderboardData);
            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("api1/updateLeaderboardData", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseError == true)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Failed to update Leaderboard Data. Error: {apiCall.ResponseJsonStr}");
            }
            else
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: Leaderboard Data update successful. Response: {apiCall.ResponseJsonStr}");
            }
        }
    }
}
