namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class UserInterfaceColors
    {
        public int Id;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public int? UserId;
        public User? User;
        public string? PrimaryColor;
        public string? SecondaryColor;
        public string? BackgroundColor;
        public string? SecondaryBackgroundColor;
        public string? NegativeColor;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}
