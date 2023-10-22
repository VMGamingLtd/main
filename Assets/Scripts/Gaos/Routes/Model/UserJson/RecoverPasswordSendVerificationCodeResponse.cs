#pragma warning disable 8632
namespace Gaos.Routes.Model.UserJson
{
    public enum RecoverPasswordSendVerificationCodeErrorKind
    {
        UserNameOrEmailNotFound,
        InternalError,
    };

    public class RecoverPasswordSendVerificationCodeResponse
    {
        public bool? IsError;
        public string? ErrorMessage;
        public RecoverPasswordSendVerificationCodeErrorKind? ErrorKind;

        public int? UserId;
    }
}
