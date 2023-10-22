using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Login
{
    public class UserChangedEventPayload
    {
        public string UserName { get; set; }
        public bool IsGuest { get; set; }
    }

    public  class UserCahngedEvent
    {
        public delegate void EventHandler(UserChangedEventPayload payload);
        public static event EventHandler OnEvent;

        public static void Emit(UserChangedEventPayload payload)
        {
            if (OnEvent != null)
            {
                OnEvent(payload);
            }
        }
    }
}
