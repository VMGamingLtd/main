using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Utils
{
    public class HttpUtils
    {
        public readonly static string CLASS_NAME = typeof(HttpUtils).Name;

        private static UnityWebRequest makeWebRequest(string url, Gaos.Api.UrlQuery query = null)
        {
            var wr = new UnityWebRequest();
            wr.method = UnityWebRequest.kHttpVerbGET;

            string _url;
            if ((query != null) && (query.Count > 0))
            {
                _url = $"{url}?{query.ToString()}";
            }
            else
            {
                _url = $"{url}";
            }
            wr.url = _url;


            wr.downloadHandler = new DownloadHandlerBuffer();

            return wr;

        }

        public static async UniTask<string> Get(string url, Gaos.Api.UrlQuery query = null)
        {
            const string METHOD = "Get";
            UnityWebRequest wr = makeWebRequest(url, query);
            await wr.SendWebRequest();

            if (wr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD}: ERROR: {wr.error}, url: {wr.url}, status: {wr.responseCode}");
                throw new System.Exception($"error calling ${url}");
            }
            else
            {
                    return wr.downloadHandler.text;
            }
        }
    }
}
