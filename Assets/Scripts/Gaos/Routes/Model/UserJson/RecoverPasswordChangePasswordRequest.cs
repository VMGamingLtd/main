#pragma warning disable 8632
namespace Gaos.Routes.Model.UserJson
{
    public class RecoverPasswordChangePasswordRequest
    {
        public string? Password;
        public string? PasswordVerify;

        public string? VerificattionCode;
        public int UserId;
    }
}
