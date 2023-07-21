using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaos.Context
{
    public class Authentication
    {
        private static string JWT = null;
        private static int UserId = -1;
        private static string UserName;
        public static void SetJWT(string jwt)
        { 
            JWT = jwt;
        }
        public static string GetJWT()
        {
            return JWT;

        }

        public static void SetUserId(int userId)
        {
            UserId = userId;
        }

        public static int GetUserId() 
        {
            return UserId;
        }

        public static void SetUserName(string userName)
        {
            UserName = userName;
        }

        public static string GetUserName()
        {
            return UserName;
        }
    }

    public class Device
    {
        private static int DeviceId = -1;
        public static void SetDeviceId(int deviceId)
        {
            DeviceId = deviceId;
        }
        public static int GetDeviceId()
        {
            return DeviceId;
        }
    }
}
