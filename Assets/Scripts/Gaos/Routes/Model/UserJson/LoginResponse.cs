#pragma warning disable 8632
using Gaos.Dbo.Model;

namespace Gaos.Routes.Model.UserJson
{
    public enum LoginResponseErrorKind
    {
        IncorrectUserNameOrEmailError,
        IncorrectPasswordError,
        InternalError,
    };


    [System.Serializable]
    public class LoginResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public LoginResponseErrorKind? ErrorKind;

        public string? UserName;
        public string? Country;
        public string? Language;
        public string? Email;
        public UserInterfaceColors? UserInterfaceColors;
        public int UserId;
        public bool? IsGuest;
        public string? Jwt;

    }
}
