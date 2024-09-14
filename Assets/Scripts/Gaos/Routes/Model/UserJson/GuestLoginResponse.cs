#pragma warning disable 8632
using Gaos.Dbo.Model;

namespace Gaos.Routes.Model.UserJson
{
    [System.Serializable]
    public class GuestLoginResponse
    {
        public bool? IsError;

        public string? ErrorMessage;
        public string? UserName;
        public string? Country;
        public string? Language;
        public UserInterfaceColors? UserInterfaceColors;
        public int UserId;

        public bool? IsGuest;

        public string? Jwt;

    }
}