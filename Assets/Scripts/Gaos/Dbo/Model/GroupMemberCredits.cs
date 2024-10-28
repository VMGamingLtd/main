namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class GroupCredits
    {
        public int Id;
        public float Credits;
        public int GroupId;
        public Groupp Group;
        public int UserId;
        public User User;
    }
}
