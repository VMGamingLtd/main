
using UnityEngine;
using System.Collections;

namespace Gaos.User.Manager
{
    public class Guest 
    {
        public readonly static string CLASS_NAME = typeof(Guest).Name;
        public static bool TryToLoginAgain = false;
        private static bool IsLoggedIn = false;

        public static Gaos.User.Api.GuestLoginResponse GuestLoginResponse = null;

        private static IEnumerator Login_()
        {
            const string METHOD_NAME = "Login_()";

            Gaos.User.Api.GuestLoginRequest request = new Gaos.User.Api.GuestLoginRequest();
            if (Gaos.Device.Manager.Registration.IsDeviceRegistered == true)
            {
                request.deviceId = Gaos.Device.Manager.Registration.DeviceRegisterResponse.deviceId;
            }
            else
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Device is not registered, cannot login");
                TryToLoginAgain = true;
                yield break;
            }

            string requestJsonStr = JsonUtility.ToJson(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("user/guestLogin", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseTimeout == true)
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Timeout logging in guest, will try again in {apiCall.Config.RequestTimeoutSeconds} seconds");
                TryToLoginAgain = true;
            }
            else
            {
                TryToLoginAgain = false;
                if (apiCall.IsResponseError == true)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: logging in guest");
                }
                else
                {
                    GuestLoginResponse = JsonUtility.FromJson<Gaos.User.Api.GuestLoginResponse>(apiCall.ResponseJsonStr);
                    if (GuestLoginResponse.isError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: logging in guest: {GuestLoginResponse.errorMessage}");
                    }
                    else
                    {
                        IsLoggedIn = true;
                    }
                }
            }
        }

        public static IEnumerator Login()
        {
            const string METHOD_NAME = "Login()";
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: logging in guest ...");
            while (true)
            {
                yield return Login_();

                if (TryToLoginAgain == true)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: trying again ...");
                    continue;
                }
                else
                {
                    if (IsLoggedIn == true)
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}:  guest logged in");
                    }
                    else
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: guest not logged in");
                    }
                    break;
                }
            }
        }

    }

    public class User
    {
        public readonly static string CLASS_NAME = typeof(User).Name;

        public static bool TryToRegisterAgain = false;
        public static Gaos.User.Api.RegisterResponse RegisterResponse = null;
        public static bool IsRegistered = false;

        private static IEnumerator Register_(string userName, string email, string password)
        {
            const string METHOD_NAME = "Register_()";


            Gaos.User.Api.RegisterRequest request = new Gaos.User.Api.RegisterRequest();

            request.userName = userName;
            request.email = email;
            request.password = password;

            if (Gaos.Device.Manager.Registration.IsDeviceRegistered == true)
            {
                request.deviceId = Gaos.Device.Manager.Registration.DeviceRegisterResponse.deviceId;
            }
            else
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Device is not registered, cannot register");
                TryToRegisterAgain = true;
                yield break;
            }

            string requestJsonStr = JsonUtility.ToJson(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("user/register", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseTimeout == true)
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Timeout registering user, will try again in {apiCall.Config.RequestTimeoutSeconds} seconds");
                TryToRegisterAgain = true;
            }
            else
            {
                TryToRegisterAgain = false;
                if (apiCall.IsResponseError == true)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: registering user");
                }
                else
                {
                    RegisterResponse = JsonUtility.FromJson<Gaos.User.Api.RegisterResponse>(apiCall.ResponseJsonStr);
                    if (RegisterResponse.isError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: registering user: {RegisterResponse.errorMessage}");
                    }
                    else
                    {
                        IsRegistered = true;
                    }
                }
            }
        }
        public static IEnumerator Register(string userName, string email, string password)
        {
            const string METHOD_NAME = "Register()";
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: registering user ...");
            while (true)
            {
                yield return Register_(userName, email, password);

                if (TryToRegisterAgain == true)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: trying again ...");
                    continue;
                }
                else
                {
                    if (IsRegistered == true)
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}:  user registered");
                    }
                    else
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: user not registered");
                    }
                    break;
                }
            }
        }

    }
}