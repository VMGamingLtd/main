using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

namespace Gaos.Device.Device
{
    public class Registration
    {
        public readonly static string CLASS_NAME = typeof(Registration).Name;

        public static bool? IsDeviceRegistered = false;

        public static Gaos.Routes.Model.DeviceJson.DeviceRegisterResponse DeviceRegisterResponse = null;

        private static bool TryToRegisterAgain = false;

        private static bool IS_PROFILE_HTTP_CALLS =  Gaos.Environment.Environment.GetEnvironment()["IS_PROFILE_HTTP_CALLS"] == "true";


        private static string GetPlatformType()
        {
            string platformType = null;
            platformType = Application.platform.ToString();
            return platformType;
        }

        private static IEnumerator RegisterDevice_()
        {
            const string METHOD_NAME = "RegisterDevice_()";

            Gaos.Routes.Model.DeviceJson.DeviceRegisterRequest request = new Gaos.Routes.Model.DeviceJson.DeviceRegisterRequest();
            request.Identification = SystemInfo.deviceUniqueIdentifier;
            request.PlatformType = GetPlatformType();
            request.BuildVersion = Application.version;

            string requestJsonStr = JsonConvert.SerializeObject(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("device/register", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseTimeout == true)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Timeout registering device, will try again in {apiCall.Config.RequestTimeoutSeconds} seconds");
                TryToRegisterAgain = true;
            }
            else
            {
                TryToRegisterAgain = false;
                if (apiCall.IsResponseError == true)
                {
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: registering device");
                }
                else
                {
                    DeviceRegisterResponse =  JsonConvert.DeserializeObject<Gaos.Routes.Model.DeviceJson.DeviceRegisterResponse>(apiCall.ResponseJsonStr);
                    if (DeviceRegisterResponse.IsError == true)
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: registering device: {DeviceRegisterResponse.ErrorMessage}");
                    }
                    else
                    {
                        IsDeviceRegistered = true;
                    }
                }
            }
        }

        public static IEnumerator RegisterDevice()
        {
            const string METHOD_NAME = "RegisterDevice()";

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            if (IS_PROFILE_HTTP_CALLS)
            {
                stopWatch.Start();
            }

            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: registering device ...");

            int maxTryCount = 20;

            while (true)
            {
                --maxTryCount;
                if (maxTryCount <= 0)
                {
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: max try count reached");
                    break;
                }

                yield return RegisterDevice_();

                if (TryToRegisterAgain == true)
                {
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: trying again ...");
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (IsDeviceRegistered == true)
            {
                Gaos.Context.Device.SetDeviceId(DeviceRegisterResponse.DeviceId);
                if (IS_PROFILE_HTTP_CALLS)
                {
                    stopWatch.Stop();
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: execution time: {stopWatch.ElapsedMilliseconds} ms");
                }
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: device registered");
            }
            else
            {
                if (IS_PROFILE_HTTP_CALLS)
                {
                    stopWatch.Stop();
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: execution time: {stopWatch.ElapsedMilliseconds} ms");
                }
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: device not registered");
                throw new System.Exception($"{CLASS_NAME}:{METHOD_NAME}: ERROR: device not registered");
            }
        }
    }
}