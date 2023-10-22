namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class Session
    {
        public int Id;
        public System.DateTime CreatedAt;
        public System.DateTime ExpiresdAt;
        public System.DateTime AccessedAt;
    }
}
