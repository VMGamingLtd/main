#pragma warning disable 8632
namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class EnsureNewSlotResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public string MongoDocumentId;
        public string MongoDocumentVersion;

    }
}
