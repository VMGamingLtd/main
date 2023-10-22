#pragma warning disable 8632
namespace Gaos.Routes.Model.UserJson
{
    public enum VerifyEmailResponseErrorKind
    {
        InvalidCodeError,
        InternalError,
    };

    public class VerifyEmailResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public string? Email;

        public VerifyEmailResponseErrorKind? ErrorKind;
    }
}
