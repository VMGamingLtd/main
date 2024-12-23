using Gaos.Dbo.Model;
using Gaos.Routes.Model.DeviceJson;
using System.Linq;
using UnityEngine;

namespace Gaos.Context
{
    public class Authentication
    {
        private static string JWT = null;
        private static int UserId = -1;
        private static string UserName;
        private static string Country;
        private static string Email;
        private static string Language = null;
        private static bool IsGuest = false;
        private static DeviceRegisterResponseUserSlot[] UserSlots = new DeviceRegisterResponseUserSlot[0];
        public static UserInterfaceColors UserInterfaceColors = null;

        public static void SetJWT(string jwt)
        {
            JWT = jwt;
            if ((Gaos.Environment.Environment.GetEnvironment()["ENV_NAME"] == "Development") || (Gaos.Environment.Environment.GetEnvironment()["ENV_NAME"] == "Development_multi") )
            {
                Debug.Log($"JWT: {jwt}");
            }
            if (Gaos.WebSocket.WebSocketClient.CurrentWesocketClient != null)
            {
                Gaos.Messages.WsAuthentication.authenticate(Gaos.WebSocket.WebSocketClient.CurrentWesocketClient, jwt);
            }
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

        public static string GetCountry()
        {
            return Country;
        }


        public static void SetCountry(string country)
        {
            Country = country;
        }

        public static string GetEmail()
        {
            return Email;
        }

        public static void SetEmail(string email)
        {
            Email = email;
        }

        public static string GetLanguage()
        {
            return Language;
        }

        public static void SetLanguage(string language)
        {
            Language = language;
        }
        public static string GetUserName()
        {
            return UserName;
        }

        public static void SetIsGuest(bool isGuest)
        {
            IsGuest = isGuest;
        }

        public static bool GetIsGuest()
        {
            return IsGuest;
        }

        public static void SetUserSlots(DeviceRegisterResponseUserSlot[] userSlots)
        {
            UserSlots = userSlots;
        }

        public static DeviceRegisterResponseUserSlot[] GetUserSlots()
        {
            return UserSlots;
        }

        public static void RemoveUserSlot(int slotId)
        {
            UserSlots = UserSlots.Where(slot => slot.SlotId != slotId).ToArray();
        }

        public static UserInterfaceColors GetUserInterfaceColors()
        {
            return UserInterfaceColors;
        }

        public static void SetUserInterfaceColors(UserInterfaceColors userColors)
        {
            UserInterfaceColors = userColors;
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
