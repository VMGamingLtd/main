#pragma warning disable 8632
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
        public int UserId;
        public bool? IsGuest;
        public string? Jwt;

    }
}
