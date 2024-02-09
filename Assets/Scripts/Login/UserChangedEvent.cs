using System;

namespace Assets.Scripts.Login
{
    public class UserChangedEventArgs: EventArgs
    {
        public string UserName { get; set; }
        public bool IsGuest { get; set; }
    }

    public class UserChangedEvent
    {
        public static event EventHandler<UserChangedEventArgs> UserChanged;

        public static void Emit(UserChangedEventArgs args)
        {
            // Thread-safe event invocation
            EventHandler<UserChangedEventArgs> handler = UserChanged;
            handler?.Invoke(null, args); // Using null as the sender since it's a static class
        }
    }
}
