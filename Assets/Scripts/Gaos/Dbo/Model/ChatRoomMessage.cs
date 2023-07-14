#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class ChatRoomMessage
    {
        public int Id;
        public int UserId;
        public User? User;
        public string? ChatRoomMemberName;
        public int ChatRoomId;
        public ChatRoom? ChatRoom;
        public int MessageId;
        public string? Message;
        public System.DateTime CreatedAt;

    }
}
