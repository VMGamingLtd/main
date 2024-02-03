#pragma warning disable 8632
namespace Gaos.Routes.Model.UserJson
{
    public enum RecoverPasswordVerifyCodeReplyErrorKind
    {
        InternalError,
    };

    public class RecoverPasswordVerifyCodeResponse
    {
        public bool? IsError;
        public string? ErrorMessage;
        public RecoverPasswordVerifyCodeReplyErrorKind? ErrorKind;

        public int? UserId;
        public string? UserName;
        public bool? IsVerified;
    }
}
