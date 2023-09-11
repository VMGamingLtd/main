#pragma warning disable CS8632

namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class UserRole
    {
        public int Id;
        public int? UserId;
        public User? User;
        public int? RoleId;
        public Role? Role;
    }
}
