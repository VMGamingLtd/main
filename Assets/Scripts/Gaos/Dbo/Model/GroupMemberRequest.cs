#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class GroupMemberRequest
    {
        public int Id;
        public int? GroupId;
        public Groupp? Group;
        public int? UserId;
        public User? User;
        public System.DateTime RequestDate;
    }
}
