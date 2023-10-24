namespace Assets.Scripts.Login
{
    public class UserChangedEventPayload
    {
        public string UserName { get; set; }
        public bool IsGuest { get; set; }
    }

    public class UserChangedEvent
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
