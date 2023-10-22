#pragma warning disable CS8618, CS8632
namespace Gaos.Dbo.Model
{
    public class UserVerificationCode
    {
        public int Id;
        public int? UserId;
        public User? User;
        public string Code;
        public System.DateTime ExpiresAt;
    }
}
