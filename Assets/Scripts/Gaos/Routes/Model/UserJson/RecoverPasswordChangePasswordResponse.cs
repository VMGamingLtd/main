#pragma warning disable 8632
namespace Gaos.Routes.Model.UserJson
{
    public enum  RecoverPasswordChangePassworErrorKind
    {
        InvalidVerificationCodeError,
        InternalError,
        PasswordIsEmptyError,
        PasswordsDoNotMatchError,
    };

    public class RecoverPasswordChangePasswordResponse
    {
        public bool? IsError;
        public string? ErrorMessage;
        public RecoverPasswordChangePassworErrorKind? ErrorKind;
    }
}
