using UnityEngine;
namespace Gaos.Device.Api
{

    [System.Serializable]
    public class DeviceRegisterRequest
    {
        public string identification;

        public string platformType;
        public string buildVersion;

    }

    [System.Serializable]
    public class DeviceRegisterResponse
    {
        public bool isError;

        public string errorMessage;

        public int deviceId;
        public string identification;
        public string platformType;
        public string buildVersion;

    }

    [System.Serializable]
    public class DeviceGetRegistrationRequest
    {
        public string identification { get; set; }
        public string platformType { get; set; }

    }

    [System.Serializable]
    public class DeviceGetRegistrationByIdRequest
    {
        public int deviceId { get; set; }
    }


    [System.Serializable]
    public class DeviceGetRegistrationResponse
    {
        public bool isError { get; set; }

        public string errorMessage { get; set; }

        public bool isFound { get; set; }

        public int deviceId { get; set; }
        public string identification { get; set; }
        public string platformType { get; set; }
        public string buildVersion { get; set; }

    }

}