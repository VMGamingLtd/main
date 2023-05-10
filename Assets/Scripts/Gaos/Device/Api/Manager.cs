using UnityEngine;
using System.Collections;

namespace Gaos.Device.Manager
{
    public class Registration
    {
        public readonly static string CLASS_NAME = typeof(Registration).Name;

        public static bool? IsDeviceRegistered = false;

        public static Gaos.Device.Api.DeviceRegisterResponse DeviceRegisterResponse = null;

        private static bool TryToRegisterAgain = false;


        private static string GetPlatformType()
        {
            string platformType = null;
            platformType = Application.platform.ToString();
            return platformType;
        }

        private static IEnumerator RegisterDevice_()
        {
            const string METHOD_NAME = "RegisterDevice_()";

            Gaos.Device.Api.DeviceRegisterRequest request = new Gaos.Device.Api.DeviceRegisterRequest();
            request.identification = SystemInfo.deviceUniqueIdentifier;
            request.platformType = GetPlatformType();
            request.buildVersion = Application.version;

            string requestJsonStr = JsonUtility.ToJson(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("device/register", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseTimeout == true)
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Timeout registering device, will try again in {apiCall.Config.RequestTimeoutSeconds} seconds");
                TryToRegisterAgain = true;
            }
            else
            {
                TryToRegisterAgain = false;
                if (apiCall.IsResponseError == true)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: registering device");
                }
                else
                {
                    DeviceRegisterResponse = JsonUtility.FromJson<Gaos.Device.Api.DeviceRegisterResponse>(apiCall.ResponseJsonStr);
                    if (DeviceRegisterResponse.isError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: registering device: {DeviceRegisterResponse.errorMessage}");
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
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: registering device ...");
            while (true)
            {
                yield return RegisterDevice_();

                if (TryToRegisterAgain == true)
                {
                    continue;
                }
                else
                {
                    if (IsDeviceRegistered == true)
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: device registered");
                    }
                    else
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: device not registered");
                    }
                    break;
                }
            }
        }
    }
}