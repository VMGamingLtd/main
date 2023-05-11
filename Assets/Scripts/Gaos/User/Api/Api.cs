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
}