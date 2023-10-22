#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    public class UserEmail
    {
        public int Id;
        public string? Email;
        public int? UserId;
        public User? User;
        public string? EmailVerificationCode;
        public bool? IsEmailVerified;
    }
}
