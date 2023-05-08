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

    public class Configuration
    {
        public static Configuration Config = new Configuration("https://localhost:7070");

        public string API_URL;
        public int RequestTimeoutSeconds = 10;
        public int RequestRetryCount = 5;
        public int RequestRetryDelaySeconds = 5;

        Configuration(string apiUrl)
        {
            this.API_URL = apiUrl;
        }

        public Configuration clone()
        {
            Configuration cfg = new Configuration(this.API_URL);
            cfg.RequestTimeoutSeconds = this.RequestTimeoutSeconds;
            cfg.RequestRetryCount = this.RequestRetryCount;
            cfg.RequestRetryDelaySeconds = this.RequestRetryDelaySeconds;
            return cfg;
        }


    }

    public class HttpGetJsonCall
    {
        private static readonly string CLASS_NAME = "HttpGetJsonCall";
        private string Url;
        private UnityWebRequest LastWebRequest;
        private UrlQuery Query;

        public string ResponseJsonStr;
        public bool IsResponseError = false;
        public bool IsResponseTimeout = false;


        private Configuration Config = Configuration.Config.clone();


        HttpGetJsonCall(string url)
        {
            this.Url = url;
            this.Query = new UrlQuery();
        }
        public void AddQueryParam(string key, string val)
        {
            this.Query.Add(key, val);
        }

        private UnityWebRequest makeWebRequest()
        {
            var wr = new UnityWebRequest();

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

            wr.method = UnityWebRequest.kHttpVerbGET;
            wr.downloadHandler = new DownloadHandlerBuffer();

            wr.useHttpContinue = false;
            wr.redirectLimit = 0;
            wr.timeout = this.Config.RequestTimeoutSeconds;

            return wr;
        }

        private IEnumerator Call_()
        {
            string METHOD = "Call_()";

            var wr = this.makeWebRequest();
            this.LastWebRequest = wr;

            yield return wr.SendWebRequest();

            if (wr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD}: error: {wr.error}, url: {wr.url}, status: {wr.responseCode}");
                this.IsResponseError = true;
                this.IsResponseTimeout = wr.error.Contains("timeout");
            }
            else
            {
                string contenType = wr.GetResponseHeader("Content-Type");
                if (!contenType.Contains("json"))
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD}: error: response content type is not json, url: {wr.url}");
                    this.IsResponseError = true;
                }
                this.ResponseJsonStr = wr.downloadHandler.text;
                this.IsResponseError = false;
            }


        }

        public IEnumerator Call()
        {
            var requestRetryCount = this.Config.RequestRetryCount;
            var requestRetryDelaySeconds = this.Config.RequestRetryDelaySeconds;
            yield return this.Call_();
        }

    }

    public class HttpPostJsonCall
    {
        private static readonly string CLASS_NAME = "HttpPostJsonCall";
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

        private IEnumerator Call_()
        {
            string METHOD = "_call()";
            var wr = this.makeWebRequest();
            this.LastWebRequest = wr;

            yield return wr.SendWebRequest();

            if (wr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD}: error: {wr.error}, url: {wr.url}, status: {wr.responseCode}");
                this.IsResponseError = true;
                this.IsResponseTimeout = wr.error.Contains("timeout");
            }
            else
            {
                string contenType = wr.GetResponseHeader("Content-Type");
                if (contenType.Contains("json"))
                {
                    this.ResponseJsonStr = wr.downloadHandler.text;
                    this.IsResponseError = false;

                }
                else
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD}: error: response content type is not json, url: {wr.url}");
                    this.IsResponseError = true;
                }
            }
        }

        public IEnumerator Call()
        {

            var requestRetryCount = this.config.RequestRetryCount;
            var requestRetryDelaySeconds = this.config.RequestRetryDelaySeconds;
            yield return this.Call_();
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

            HttpPostJsonCall http = new HttpPostJsonCall($"{Configuration.Config.API_URL}/{this.UrlPath}", RequestJsonStr);
            http.SetConfig(this.Config);

            yield return http.Call();

            if (http.IsResponseError)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD}: error calling api url: {Configuration.Config.API_URL}{this.UrlPath}");
                this.IsResponseError = true;
                this.IsResponseTimeout = http.IsResponseTimeout;
            }
            else
            {
                this.ResponseJsonStr = http.ResponseJsonStr;
                this.IsResponseError = false;
            }

        }
    }


}
