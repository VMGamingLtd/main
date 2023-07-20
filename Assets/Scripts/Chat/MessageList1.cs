
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;


namespace Chat
{
    public class MessageModel
    {
        public Gaos.Routes.Model.ChatRoomJson.ResponseMessage message;
    }

    public class MessageList1 : MonoBehaviour
    {
        private static int MAX_SCROLL_LIST_LINES_CPUNT = 1000;
        private static int MAX_MESSAGE_COUNT_TO_PULL = 5;

        public GameObject chatMessageTemplate;
        public TMP_InputField typeMessageInput;
        public string targetUsername;

        private LinkedList<MessageModel> AllMessages = new LinkedList<MessageModel>();

        private GameObject[] AllMessageLines = new GameObject[MAX_SCROLL_LIST_LINES_CPUNT]; // all friends buttons in the scroll list
        private int LastIndexAllMessageLines = -1;

        private int ChatRoomId = -1;

        private void AllocateMessageLines()
        {
            int n = AllMessages.Count - (LastIndexAllMessageLines + 1);
            while (n > 0)
            {
                // Create a new message line object as a child of the current object
                GameObject messageLine = Instantiate(chatMessageTemplate, transform);
                messageLine.transform.position = Vector3.zero;
                messageLine.SetActive(false);
                AllMessageLines[++LastIndexAllMessageLines] = messageLine;
                --n;
            }
        }

        private void TrimAllMessages()
        {
            if (AllMessages.Count > MAX_SCROLL_LIST_LINES_CPUNT)
            {
                int n = AllMessages.Count - MAX_SCROLL_LIST_LINES_CPUNT;
                while (n > 0)
                {
                    AllMessages.RemoveFirst();
                    --n;
                }
            }

        }

        private async UniTaskVoid readFirstMessages()
        {
            Gaos.Routes.Model.ChatRoomJson.ReadMessagesBackwardsResponse response = await Gaos.ChatRoom.ChatRoom.ReadMessagesBackwards.CallAsync(ChatRoomId, -1, MAX_MESSAGE_COUNT_TO_PULL);
            for (var i = 0; i < response.Messages.Length; i++)
            {
                var message = response.Messages[i];
                MessageModel messageModel = new MessageModel();
                messageModel.message = message;
                AllMessages.AddFirst(messageModel);
            }
        }

        private void DisplayAllMessages() // display all messages in the scroll list
        {
            TrimAllMessages();
            AllocateMessageLines(); 

            int i = 0;
            foreach (var messageModel in AllMessages)
            {
                GameObject messageLine = AllMessageLines[i++];
                messageLine.SetActive(true);
                //messageLine.GetComponent<ChatMessage>().SetMessage(messageModel.message);
            }
        }
    }
}
