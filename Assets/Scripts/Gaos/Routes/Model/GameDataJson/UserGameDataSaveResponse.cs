#pragma warning disable 8632
namespace Gaos.Routes.Model.GameDataJson
{
    public enum UserGameDataSaveErrorKind
    {
        JsonDiffBaseMismatchError,
        VersionMismatchError,
        InternalError,
    }

    [System.Serializable]
    public class UserGameDataSaveResponse
    {
        public bool? IsError;
        public string? ErrorMessage;
        public UserGameDataSaveErrorKind? ErrorKind;

        public string? Id;
        public long Version;

        public string? GameDataJson;

    }
}
