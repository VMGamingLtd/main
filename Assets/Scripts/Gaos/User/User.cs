
using UnityEngine;
using System.Collections;

namespace Gaos.User.User
{
    public class GuestLogin 
    {
        private readonly static string CLASS_NAME = typeof(GuestLogin).Name;
        private static bool TryToLoginAgain = false;
        public static bool IsLoggedIn = false;

        //public static Gaos.User.Api.GuestLoginResponse GuestLoginResponse = null;
        public static Gaos.Routes.Model.UserJson.GuestLoginResponse GuestLoginResponse = null;

        private static IEnumerator Login_()
        {
            const string METHOD_NAME = "Login_()";

            Gaos.User.Api.GuestLoginRequest request = new Gaos.User.Api.GuestLoginRequest();
            if (Gaos.Device.Device.Registration.IsDeviceRegistered == true)
            {
                request.deviceId = (int)Gaos.Device.Device.Registration.DeviceRegisterResponse.DeviceId;
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
                    GuestLoginResponse = JsonUtility.FromJson<Gaos.Routes.Model.UserJson.GuestLoginResponse>(apiCall.ResponseJsonStr);
                    if (GuestLoginResponse.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: logging in guest: {GuestLoginResponse.ErrorMessage}");
                    }
                    else
                    {
                        IsLoggedIn = true;
                    }
                }
            }
        }

        public delegate void OnGuestLoginComplete();

        public static IEnumerator Login(OnGuestLoginComplete onComplete = null)
        {
            const string METHOD_NAME = "Login()";
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: logging in guest ...");

            int maxTryCount = 5;

            while (true)
            {
                --maxTryCount;
                if (maxTryCount <= 0)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: max try count reached");
                    break;
                }

                yield return Login_();

                if (TryToLoginAgain == true)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: trying again ...");
                    yield return new WaitForSeconds(1);
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (IsLoggedIn == true)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}:  guest logged in");
                Gaos.Context.Authentication.SetJWT(GuestLoginResponse.Jwt);
                Gaos.Context.Authentication.SetUserId(GuestLoginResponse.UserId);
            }
            else
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: guest not logged in");
            }

            if (onComplete != null)
            {
                onComplete();
            }

        }
    }

    public class UserRegister
    {
        public readonly static string CLASS_NAME = typeof(UserRegister).Name;

        public static bool TryToRegisterAgain = false;
        public static Gaos.Routes.Model.UserJson.RegisterResponse  RegisterResponse = null;
        public static bool IsRegistered = false;

        private static IEnumerator Register_(string userName, string email, string password)
        {
            const string METHOD_NAME = "Register_()";


            Gaos.User.Api.RegisterRequest request = new Gaos.User.Api.RegisterRequest();

            request.userName = userName;
            request.email = email;
            request.password = password;

            if (Gaos.Device.Device.Registration.IsDeviceRegistered == true)
            {
                request.deviceId = (int)Gaos.Device.Device.Registration.DeviceRegisterResponse.DeviceId;
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
                    RegisterResponse = JsonUtility.FromJson<Gaos.Routes.Model.UserJson.RegisterResponse>(apiCall.ResponseJsonStr);
                    if (RegisterResponse.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: registering user: {RegisterResponse.ErrorMessage}");
                    }
                    else
                    {
                        IsRegistered = true;
                    }
                }
            }
        }

        public delegate void OnUserRegisterComplete();

        public static IEnumerator Register(string userName, string email, string password, OnUserRegisterComplete onUserRegisterComplete = null)
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

            if (onUserRegisterComplete != null)
            {
                onUserRegisterComplete();
            }
        }

    }

    public class UserLogin
    {
        public readonly static string CLASS_NAME = typeof(UserLogin).Name;

        public static bool TryToLoginAgain = false;
        public static Gaos.Routes.Model.UserJson.LoginResponse LoginResponse = null;
        public static bool IsLoggedIn = false;

        private static IEnumerator Login_(string userName, string password)
        {
            const string METHOD_NAME = "Login_()";


            Gaos.User.Api.LoginRequest request = new Gaos.User.Api.LoginRequest();

            request.userName = userName;
            request.password = password;

            if (Gaos.Device.Device.Registration.IsDeviceRegistered == true)
            {
                request.deviceId = (int)Gaos.Device.Device.Registration.DeviceRegisterResponse.DeviceId;
            }
            else
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Device is not registered, cannot login");
                TryToLoginAgain = true;
                yield break;
            }

            string requestJsonStr = JsonUtility.ToJson(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("user/login", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseTimeout == true)
            {
                Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Timeout logging in user, will try again in {apiCall.Config.RequestTimeoutSeconds} seconds");
                TryToLoginAgain = true;
            }
            else
            {
                TryToLoginAgain = false;
                if (apiCall.IsResponseError == true)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: logging in user");
                }
                else
                {
                    LoginResponse = JsonUtility.FromJson<Gaos.Routes.Model.UserJson.LoginResponse>(apiCall.ResponseJsonStr);
                    if (LoginResponse.IsError == true)
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: logging in user: {LoginResponse.ErrorMessage}");
                    }
                    else
                    {
                        IsLoggedIn = true;
                    }
                }
            }
        }

        public delegate void OnUserLoginComplete();

        public static IEnumerator Login(string userName, string password, OnUserLoginComplete onUserLoginComplete = null)
        {
            const string METHOD_NAME = "Login()";
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: logging in user ...");
            while (true)
            {
                yield return Login_(userName, password);

                if (TryToLoginAgain == true)
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: trying again ...");
                    continue;
                }
                else
                {
                    if (IsLoggedIn == true)
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}:  user logged in");
                        Gaos.Context.Authentication.SetJWT(LoginResponse.Jwt);
                    }
                    else
                    {
                        Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}: ERROR: user not logged in");
                    }
                    break;
                }
            }

            if (onUserLoginComplete != null)
            {
                onUserLoginComplete();
            }
        }

        public static IEnumerator LoginWithWaitForRegistration(string userName, string password)
        {
            const string METHOD_NAME = "LoginWithWaitForRegistration()";
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: logging in user ...");
            while (true)
            {
                if (UserRegister.IsRegistered == true)
                {
                    yield return Login(userName, password);
                    if (IsLoggedIn == true)
                    {
                        break;
                    }
                } 
                else
                {
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}:  loggin failed: user not registered");
                    Debug.LogWarning($"{CLASS_NAME}:{METHOD_NAME}:  retrying again ...");

                }
                yield return new WaitForSeconds(2);
            }
        }

    }
}