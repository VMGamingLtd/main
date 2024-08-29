namespace Gaos.Routes.Model.UserJson
{
    [System.Serializable]
    public class UpdateCountryRequest
    {
        public int UserId;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? Country;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}
