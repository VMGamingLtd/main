using UnityEngine;
namespace Gaos.User.Api
{
    public class GuestLoginRequest
    {
        public string userName;
        public int deviceId;
    }

    public class GuestLoginResponse
    {
        public bool isError;

        public string errorMessage;
        public string userName;

        public string jwt;

    }

    public class RegisterRequest
    {
        public string userName;

        public string email;

        public string password;

        public string passwordVerify;
        public int deviceId;

    }


    public class RegisterResponse
    {
        public bool isError;

        public string errorMessage;

        public string jwt;
    }

    public class LoginRequest
    {
        public string userName;

        public string password;

        public int deviceId;
    }

    public class LoginResponse
    {
        public bool isError;

        public string errorMessage;


        public string jwt;

    }

}