namespace Gaos.Dbo.Model
{
    public class LeaderboardData
    {
        public int Id;
        public int UserId;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? Username;
        public string? Country;
        public int? Hours;
        public int? Minutes;
        public int? Seconds;
        public int? Score;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}
