#pragma warning disable 8632
namespace Gaos.Routes.Model.DeviceJson
{
    [System.Serializable]
    public class DeviceRegisterResponseUserSlot
    {
        public int MongoDocumentId;
        public int SlotId;
    }

    [System.Serializable]
    public class DeviceRegisterResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public int DeviceId;
        public string? Identification;
        public string? PlatformType;
        public string? BuildVersion;

        public Dbo.Model.User? User;
        public Dbo.Model.JWT? JWT;

        public List<DeviceRegisterResponseUserSlot>? UserSlots;

    }
}