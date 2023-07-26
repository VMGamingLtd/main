using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Gaos.Api
{
    public class UrlQuery {
        private Dictionary<string, object> QueryParams = new Dictionary<string, object>();

        public UrlQuery() { }

        public UrlQuery(Dictionary<string, object> queryParams)
        {
            this.QueryParams = new Dictionary<string, object>(queryParams);
        }

        public void Add(string key, object val)
        {
            this.QueryParams[key] = val;
        }

        public int Count
        {
            get
            {
                return this.QueryParams.Count;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, object> kvp in this.QueryParams)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append($"{UnityWebRequest.EscapeURL(kvp.Key)}={UnityWebRequest.EscapeURL(kvp.Value.ToString())}");
            }
            return sb.ToString();
        }
        
    }

    public class Common
    {
        public static void SetGaosHeaders(UnityWebRequest wr)
        {
            string jwt = Gaos.Context.Authentication.GetJWT();
            if (jwt != null && jwt.Trim() != "")
            {
                wr.SetRequestHeader("Authorization", $"Bearer {jwt}");
            }
            wr.SetRequestHeader("X-Gao-DeviceId", $"{Gaos.Context.Device.GetDeviceId()}");
            wr.SetRequestHeader("X-Gao-DeviceIdentification", $"{SystemInfo.deviceUniqueIdentifier}");
            wr.SetRequestHeader("X-Gao-DeviceIdentification", $"{Application.platform.ToString()}");

        }
    }

    public class Configuration
    {
        public static Configuration Config = new Configuration($"{Gaos.Environment.Environment.GetEnvironment()["GAOS_URL"]}");

        public string API_URL;
        public int RequestTimeoutSeconds = 10;

        Configuration(string apiUrl)
        {
            this.API_URL = apiUrl;
        }

        public Configuration clone()
        {
            Configuration cfg = new Configuration(this.API_URL);
            cfg.RequestTimeoutSeconds = this.RequestTimeoutSeconds;
            return cfg;
        }


    }

    public class PostResponse
    {
        public string ResponseJsonStr;
        public bool IsResponseError = false;
        public bool IsResponseTimeout = false;
    }

    public class HttpPostJsonCall
    {
        public readonly static string CLASS_NAME = typeof(HttpPostJsonCall).Name;
        private string Url;
        private string RequestJsonStr;
        private UnityWebRequest LastWebRequest;
        private UrlQuery Query;

        public string ResponseJsonStr;
        public bool IsResponseError = false;
        public bool IsResponseTimeout = false;


        private Configuration config = Configuration.Config.clone();

        public HttpPostJsonCall(string url, string requestJsonStr)
        {
            this.Url = url;
            this.RequestJsonStr = requestJsonStr;
            this.Query = new UrlQuery();

        }
        public void AddQueryParam(string key, string val)
        {
            this.Query.Add(key, val);
        }

        private UnityWebRequest makeWebRequest()
        {
            var wr = new UnityWebRequest();
            wr.method = UnityWebRequest.kHttpVerbPOST;

            string url;
            if (this.Query.Count > 0)
            {
                url = $"{this.Url}?{this.Query.ToString()}";
            }
            else
            {
                url = $"{this.Url}";
            }
            wr.url = url;

            Common.SetGaosHeaders(wr);

            Byte[] payload = Encoding.UTF8.GetBytes(this.RequestJsonStr);
            UploadHandler uploadHandler = new UploadHandlerRaw(payload);
            uploadHandler.contentType = "application/json";
            wr.uploadHandler = uploadHandler;
            wr.SetRequestHeader("Content-Type", "application/json");

            wr.downloadHandler = new DownloadHandlerBuffer();

            wr.useHttpContinue = false;
            wr.redirectLimit = 0;
            wr.timeout = this.config.RequestTimeoutSeconds;

            return wr;

        }

        public void SetConfig(Configuration config)
        {
            this.config = config;

        }

        public IEnumerator Call()
        {
            string METHOD = "Call()";
            var wr = this.makeWebRequest();
            this.LastWebRequest = wr;

            yield return wr.SendWebRequest();

            if (wr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD}: ERROR: {wr.error}, url: {wr.url}, status: {wr.responseCode}");
                this.IsResponseError = true;
                this.IsResponseTimeout = wr.error.Contains("timeout");

                string contenType = wr.GetResponseHeader("Content-Type");
                if (contenType != null && contenType.Contains("json"))
                {
                    this.ResponseJsonStr = wr.downloadHandler.text;
                }
            }
            else
            {
                string contenType = wr.GetResponseHeader("Content-Type");
                if (contenType != null && contenType.Contains("json"))
                {
                    this.ResponseJsonStr = wr.downloadHandler.text;
                    this.IsResponseError = false;

                }
                else
                {
                    Debug.Log($"{CLASS_NAME}:{METHOD}: ERROR: response content type is not json, url: {wr.url}");
                    this.IsResponseError = true;
                }
            }
        }

        public async UniTask<PostResponse> CallAsync()
        {
            string METHOD = "CallAsync()";
            UnityWebRequest wr;
            try
            {
                wr = this.makeWebRequest();
                this.LastWebRequest = wr;

                await wr.SendWebRequest();

                if (wr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"{CLASS_NAME}:{METHOD}: ERROR: {wr.error}, url: {wr.url}, status: {wr.responseCode}");
                    this.IsResponseError = true;
                    this.IsResponseTimeout = wr.error.Contains("timeout");

                    string contenType = wr.GetResponseHeader("Content-Type");
                    if (contenType != null && contenType.Contains("json"))
                    {
                        this.ResponseJsonStr = wr.downloadHandler.text;
                    }

                }
                else
                {
                    string contenType = wr.GetResponseHeader("Content-Type");
                    if (contenType != null && contenType.Contains("json"))
                    {
                        this.ResponseJsonStr = wr.downloadHandler.text;
                        this.IsResponseError = false;

                    }
                    else
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD}: ERROR: response content type is not json, url: {wr.url}");
                        this.IsResponseError = true;
                    }
                }

                return new PostResponse()
                {
                    ResponseJsonStr = this.ResponseJsonStr,
                    IsResponseError = this.IsResponseError,
                    IsResponseTimeout = this.IsResponseTimeout
                };
            }
            catch (Exception ex)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD}: ERROR: {ex.Message}, url: {this.Url}");
                throw ex;
            }

        }

    }

    public class ApiCall
    {
        public readonly static string CLASS_NAME = typeof(ApiCall).Name;

        private string UrlPath;

        public readonly string RequestJsonStr;
        public string ResponseJsonStr;
        public bool IsResponseError = false;
        public bool IsResponseTimeout = false;

        public Configuration Config = Configuration.Config.clone();

        public ApiCall(string urlPath, string requestJsonStr)
        {
            this.UrlPath = urlPath;
            this.RequestJsonStr = requestJsonStr;
        }
        public void SetConfig(Configuration config)
        {
            this.Config = config;
        }

        public IEnumerator Call()
        {
            string METHOD = "Call()";

            string url = $"{Configuration.Config.API_URL}/{this.UrlPath}";

            HttpPostJsonCall http = new HttpPostJsonCall($"{url}", RequestJsonStr);
            http.SetConfig(this.Config);

            yield return http.Call();

            if (http.IsResponseError)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD}: ERROR: calling api url: {url}, {http.ResponseJsonStr}");
            }

            this.ResponseJsonStr = http.ResponseJsonStr;
            this.IsResponseError = http.IsResponseError;
            this.IsResponseTimeout = http.IsResponseTimeout;

        }

        public async UniTask<PostResponse> CallAsync()
        {
            string METHOD = "CallAsync()";

            string url = $"{Configuration.Config.API_URL}/{this.UrlPath}";

            HttpPostJsonCall http = new HttpPostJsonCall($"{url}", RequestJsonStr);
            http.SetConfig(this.Config);

            await http.CallAsync();

            if (http.IsResponseError)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD}: ERROR: calling api url: {url}, {http.ResponseJsonStr}");
            }

            this.ResponseJsonStr = http.ResponseJsonStr;
            this.IsResponseError = http.IsResponseError;
            this.IsResponseTimeout = http.IsResponseTimeout;

            return new PostResponse()
            {
                ResponseJsonStr = this.ResponseJsonStr,
                IsResponseError = this.IsResponseError,
                IsResponseTimeout = this.IsResponseTimeout
            };

        }
    }



}
