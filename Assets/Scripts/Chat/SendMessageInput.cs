using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace Chat
{
    public class SendMessageInput : MonoBehaviour
    {
        public Chat.MessageList messageList;

        private TMP_InputField inputField;

        private void Start()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        public void CreateMessage()
        {
            string messageContent = inputField.text;
            if (!string.IsNullOrEmpty(messageContent))
            {
                SendChatMessage(messageContent).Forget();

                inputField.text = string.Empty;
            }
        }

        private async UniTaskVoid SendChatMessage(string messageContent)
        {
            // Send the message to the server
            await Gaos.ChatRoom.ChatRoom.WriteMessage.CallAsync(messageList.ChatRoomId, messageContent);
        }

    }

}
