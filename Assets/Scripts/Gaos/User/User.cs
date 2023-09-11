#pragma warning disable 8632

using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

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

            Gaos.Routes.Model.UserJson.GuestLoginRequest request = new Gaos.Routes.Model.UserJson.GuestLoginRequest();
            if (Gaos.Device.Device.Registration.IsDeviceRegistered == true)
            {
                request.DeviceId = (int)Gaos.Device.Device.Registration.DeviceRegisterResponse.DeviceId;
            }
            else
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Device is not registered, cannot login");
                TryToLoginAgain = true;
                yield break;
            }

            string requestJsonStr = JsonConvert.SerializeObject(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("user/guestLogin", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseTimeout == true)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Timeout logging in guest, will try again in {apiCall.Config.RequestTimeoutSeconds} seconds");
                TryToLoginAgain = true;
            }
            else
            {
                TryToLoginAgain = false;
                if (apiCall.IsResponseError == true)
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: logging in guest");
                }
                else
                {
                    GuestLoginResponse = JsonConvert.DeserializeObject<Gaos.Routes.Model.UserJson.GuestLoginResponse>(apiCall.ResponseJsonStr);
                    if (GuestLoginResponse.IsError == true)
                    {
                        Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: logging in guest: {GuestLoginResponse.ErrorMessage}");
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

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: logging in guest ...");

            int maxTryCount = 5;

            while (true)
            {
                --maxTryCount;
                if (maxTryCount <= 0)
                {
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: max try count reached");
                    break;
                }

                yield return Login_();

                if (TryToLoginAgain == true)
                {
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: trying again ...");
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
                Context.Authentication.SetJWT(GuestLoginResponse.Jwt);
                Context.Authentication.SetUserId(GuestLoginResponse.UserId);
                Context.Authentication.SetUserName(GuestLoginResponse.UserName);
                Context.Authentication.SetIsGuest(true);
            }
            else
            {
                Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: guest not logged in");
                Context.Authentication.SetUserId(-1);
                Context.Authentication.SetUserName("");
                Context.Authentication.SetIsGuest(false);
                Context.Authentication.SetJWT("");
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
        public static Gaos.Routes.Model.UserJson.RegisterResponseErrorKind? ResponseErrorKind = null;


        private static IEnumerator Register_(string userName, string email, string password)
        {
            const string METHOD_NAME = "Register_()";

            TryToRegisterAgain = false;
            RegisterResponse = null;
            IsRegistered = false;
            ResponseErrorKind = null;

            Gaos.Routes.Model.UserJson.RegisterRequest request = new Gaos.Routes.Model.UserJson.RegisterRequest();

            request.UserName = userName;
            request.Email = email;
            request.Password = password;

            if (Gaos.Device.Device.Registration.IsDeviceRegistered == true)
            {
                request.DeviceId = (int)Gaos.Device.Device.Registration.DeviceRegisterResponse.DeviceId;
            }
            else
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Device is not registered, cannot register");
                TryToRegisterAgain = true;
                yield break;
            }

            string requestJsonStr = JsonConvert.SerializeObject(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("user/register", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseTimeout == true)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Timeout registering user, will try again in {apiCall.Config.RequestTimeoutSeconds} seconds");
                TryToRegisterAgain = true;
            }
            else
            {
                TryToRegisterAgain = false;
                if (apiCall.IsResponseError == true)
                {
                    ResponseErrorKind = Gaos.Routes.Model.UserJson.RegisterResponseErrorKind.InternalError;
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: registering user");
                }
                else
                {
                    RegisterResponse = JsonConvert.DeserializeObject<Gaos.Routes.Model.UserJson.RegisterResponse>(apiCall.ResponseJsonStr);
                    ResponseErrorKind = RegisterResponse.ErrorKind;
                    if (RegisterResponse.IsError == true)
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: registering user: {RegisterResponse.ErrorMessage}");
                        IsRegistered = false;
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

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: registering user ...");

            while (true)
            {
                yield return Register_(userName, email, password);

                if (TryToRegisterAgain == true)
                {
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: trying again ...");
                    continue;
                }
                else
                {
                    if (IsRegistered == true)
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}:  user registered");

                        Context.Authentication.SetUserId(RegisterResponse.User.Id);
                        Context.Authentication.SetUserName(RegisterResponse.User.Name);
                        if (RegisterResponse.User.IsGuest == true)
                        {
                            Context.Authentication.SetIsGuest(true);
                        }
                        else
                        {
                            Context.Authentication.SetIsGuest(false);
                        }
                        Context.Authentication.SetJWT(RegisterResponse.Jwt);
                    }
                    else
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: user not registered");
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


            Gaos.Routes.Model.UserJson.LoginRequest request = new Gaos.Routes.Model.UserJson.LoginRequest();

            request.UserName = userName;
            request.Password = password;

            if (Gaos.Device.Device.Registration.IsDeviceRegistered == true)
            {
                request.DeviceId = (int)Gaos.Device.Device.Registration.DeviceRegisterResponse.DeviceId;
            }
            else
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Device is not registered, cannot login");
                TryToLoginAgain = true;
                yield break;
            }

            string requestJsonStr = JsonConvert.SerializeObject(request);

            Gaos.Api.ApiCall apiCall = new Gaos.Api.ApiCall("user/login", requestJsonStr);
            yield return apiCall.Call();

            if (apiCall.IsResponseTimeout == true)
            {
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: Timeout logging in user, will try again in {apiCall.Config.RequestTimeoutSeconds} seconds");
                TryToLoginAgain = true;
            }
            else
            {
                TryToLoginAgain = false;
                if (apiCall.IsResponseError == true)
                {
                    Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: logging in user");
                }
                else
                {
                    LoginResponse = JsonConvert.DeserializeObject<Gaos.Routes.Model.UserJson.LoginResponse>(apiCall.ResponseJsonStr);
                    if (LoginResponse.IsError == true)
                    {
                        Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: logging in user: {LoginResponse.ErrorMessage}");
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
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: trying again ...");
                    continue;
                }
                else
                {
                    if (IsLoggedIn == true)
                    {
                        Debug.Log($"{CLASS_NAME}:{METHOD_NAME}:  user logged in");
                        Gaos.Context.Authentication.SetJWT(LoginResponse.Jwt);
                        Gaos.Context.Authentication.SetUserId(LoginResponse.UserId);
                        Gaos.Context.Authentication.SetUserName(LoginResponse.UserName);
                    }
                    else
                    {
                        Debug.LogError($"{CLASS_NAME}:{METHOD_NAME}: ERROR: user not logged in");
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
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}:  loggin failed: user not registered");
                    Debug.Log($"{CLASS_NAME}:{METHOD_NAME}:  retrying again ...");

                }
                yield return new WaitForSeconds(2);
            }
        }

    }
}