#pragma warning disable 8632

using Gaos.Dbo.Model;

namespace Gaos.Routes.Model.UserJson
{

    [System.Serializable]
    public class RegisterRequest
    {
        public string? UserName;

        public string? Email;

        public string? Password;
        public string? Language;
        public string? Country;

        public UserInterfaceColors? UserInterfaceColors;

        public string? PasswordVerify;
        public int? DeviceId;

    }

}
