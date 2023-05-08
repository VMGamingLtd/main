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

        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        private static void TestFromJson()
        {
            Gaos.Device.Api.DeviceRegisterResponse respone = new Gaos.Device.Api.DeviceRegisterResponse
            {
                isError = true,

                errorMessage = "some error",

                deviceId = 5,
                identification = "xyz",
                platformType = "windows",
                buildVersion = "1.2.3",

            };

            string responseJsonStr = JsonUtility.ToJson(respone);

            Gaos.Device.Api.DeviceRegisterResponse responseFromJson = JsonUtility.FromJson<Gaos.Device.Api.DeviceRegisterResponse>(responseJsonStr);
            Debug.Log(responseFromJson);

            string responseJsonStr1 = "{\"isError\":true,\"errorMessage\":\"platformType is not found\",\"deviceId\":null,\"identification\":null,\"platformType\":null,\"buildVersion\":null}";
            Gaos.Device.Api.DeviceRegisterResponse responseFromJson1 = JsonUtility.FromJson<Gaos.Device.Api.DeviceRegisterResponse>(responseJsonStr1);
            Debug.Log(responseFromJson);

        }
        // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


        private static IEnumerator RegisterDevice_()
        {
            const string METHOD_NAME = "RegisterDevice_()";

            Gaos.Device.Api.DeviceRegisterRequest request = new Gaos.Device.Api.DeviceRegisterRequest();
            request.identification = SystemInfo.deviceUniqueIdentifier;
            request.platformType = SystemInfo.operatingSystem;
            request.buildVersion = Application.version;

            string requestJsonStr = JsonUtility.ToJson(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("device/register", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseTimeout == true)
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: Timeout registering device, will try again in {apiCall.Config.RequestTimeoutSeconds} seconds");
                TryToRegisterAgain = true;
            }
            else
            {
                TryToRegisterAgain = false;
                if (apiCall.IsResponseError == true)
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: Error registering device");
                }
                else
                {
                    TestFromJson(); // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    DeviceRegisterResponse = JsonUtility.FromJson<Gaos.Device.Api.DeviceRegisterResponse>(apiCall.ResponseJsonStr);
                    if (DeviceRegisterResponse.isError == true)
                    {
                        Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: Error registering device: {DeviceRegisterResponse.errorMessage}");
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
                        Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: device not registered");
                    }
                    break;
                }
            }
        }
    }
}