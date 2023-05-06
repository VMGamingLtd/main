namespace Gaos.Device.Api
{
    public class DeviceRegisterResponse
    {
        public bool? isError { get; set; }

        public string? errorMessage { get; set; }

        public int? deviceId { get; set; }
        public string? identification { get; set; }
        public string? platformType { get; set; }
        public string? buildVersion { get; set; }

    }

    public class DeviceRegisterRequest
    {
        public string? identification { get; set; }

        public string? platformType { get; set; }
        public string? buildVersion { get; set; }

    }

    public class DeviceGetRegistrationRequest
    {
        public string? identification { get; set; }
        public string? platformType { get; set; }

    }

    public class DeviceGetRegistrationByIdRequest
    {
        public int? deviceId { get; set; }
    }


    public class DeviceGetRegistrationResponse
    {
        public bool? isError { get; set; }

        public string? errorMessage { get; set; }

        public bool? isFound { get; set; }

        public int? deviceId { get; set; }
        public string? identification { get; set; }
        public string? platformType { get; set; }
        public string? buildVersion { get; set; }

    }


    public class Register: Gaos.Api.ApiCall<DeviceRegisterRequest, DeviceRegisterResponse>
    {
        public Register(DeviceRegisterRequest request): base("/device/register", request) { }

    }
    public class GetRegistration: Gaos.Api.ApiCall<DeviceGetRegistrationRequest, DeviceRegisterResponse>
    {
        public GetRegistration(DeviceGetRegistrationRequest request): base("/device/getRegistration", request) { }
    }

    public class GetRegistrationById: Gaos.Api.ApiCall<DeviceGetRegistrationByIdRequest, DeviceRegisterResponse>
    {
        public GetRegistrationById(DeviceGetRegistrationByIdRequest request): base("/device/getRegistrationById", request) { }
    }


}