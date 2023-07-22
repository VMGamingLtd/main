
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

    public class MessageList : MonoBehaviour
    {
        private static int MAX_SCROLL_LIST_LINES_COUNT = 1000;
        private static int MAX_MESSAGE_COUNT_TO_PULL = 5;

        public GameObject chatMessageTemplate;
        public TMP_InputField typeMessageInput;

        private LinkedList<MessageModel> AllMessages = new LinkedList<MessageModel>();

        private GameObject[] AllMessageLines = new GameObject[MAX_SCROLL_LIST_LINES_COUNT]; // all friends buttons in the scroll list
        private int LastIndexAllMessageLines = -1;

        public int ChatRoomId = -1;
        public string ChatRoomName;

        private bool IsFinished = false;

        public string FriendUsername;

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
            if (AllMessages.Count > MAX_SCROLL_LIST_LINES_COUNT)
            {
                int n = AllMessages.Count - MAX_SCROLL_LIST_LINES_COUNT;
                while (n > 0)
                {
                    AllMessages.RemoveFirst();
                    --n;
                }
            }

        }

        private void DisplayAllMessages() // display all messages in the scroll list
        {
            TrimAllMessages();
            AllocateMessageLines(); 

            int i = 0;
            foreach (Chat.MessageModel messageModel in AllMessages)
            {
                GameObject messageLine = AllMessageLines[i++];
                messageLine.SetActive(true);

                Message message = messageLine.GetComponent<Chat.Message>();
                //messageLine.GetComponent<ChatMessage>().SetMessage(messageModel.message);

                message.dateText.text = messageModel.message.CreatedAt.ToString();
                message.usernameText.text = messageModel.message.UserName;
                message.messageText.text = messageModel.message.Message;
            }

            for(; i <= LastIndexAllMessageLines; i++)
            {
                GameObject messageLine = AllMessageLines[i];
                messageLine.SetActive(false);
            }
        }

        private int GetLastMessageIdx()
        {
            return AllMessages.Count - 1;
        }

        private async UniTask readLastMessages()
        {
            int lastMessageIdx;
            if (AllMessages.Count  >  0)
            {
                lastMessageIdx = AllMessages.Last.Value.message.MessageId;
                Gaos.Routes.Model.ChatRoomJson.ReadMessagesResponse response = await Gaos.ChatRoom.ChatRoom.ReadMessages.CallAsync(ChatRoomId, lastMessageIdx, MAX_MESSAGE_COUNT_TO_PULL);
                for (var i = 0; i < response.Messages.Length; i++)
                {
                    var message = response.Messages[i];
                    MessageModel messageModel = new MessageModel();
                    messageModel.message = message;
                    AllMessages.AddLast(messageModel);
                }
                TrimAllMessages();
            }
            else
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

        }

        private async UniTask readPreviousMessages()
        {
            if (AllMessages.Count > 0 && AllMessages.Count < MAX_SCROLL_LIST_LINES_COUNT)
            {
                int firstMessageIdx = AllMessages.First.Value.message.MessageId;
                Gaos.Routes.Model.ChatRoomJson.ReadMessagesBackwardsResponse response = await Gaos.ChatRoom.ChatRoom.ReadMessagesBackwards.CallAsync(ChatRoomId, firstMessageIdx, MAX_MESSAGE_COUNT_TO_PULL);
                for (var i = 0; i < response.Messages.Length; i++)
                {
                    var message = response.Messages[i];
                    MessageModel messageModel = new MessageModel();
                    messageModel.message = message;
                    AllMessages.AddFirst(messageModel);
                }
                TrimAllMessages();
            }

        }


        private System.Threading.CancellationTokenSource ReadMessagesLoopWaitCancellationTokenSource;  



        private async UniTaskVoid ReadMessagesLoop()
        {
            await EnsureChatRoomExists();

            ReadMessagesLoopWaitCancellationTokenSource = new System.Threading.CancellationTokenSource();
            while (true)
            {
                try
                {
                    await UniTask.Delay(System.TimeSpan.FromSeconds(5), ignoreTimeScale: false, PlayerLoopTiming.Update, ReadMessagesLoopWaitCancellationTokenSource.Token);
                }
                catch (System.OperationCanceledException)
                {
                    if (IsFinished)
                    {
                        break;
                    }
                }

                int cnt = AllMessages.Count;
                await readLastMessages();
                if (IsFinished)
                {
                    break;
                }
                if (cnt != AllMessages.Count)
                {
                    DisplayAllMessages();
                }

                if (AllMessages.Count  < MAX_SCROLL_LIST_LINES_COUNT)
                {
                    cnt = AllMessages.Count;
                    await readPreviousMessages();
                    if (IsFinished)
                    {
                        break;
                    }
                    if (cnt != AllMessages.Count)
                    {
                        DisplayAllMessages();
                    }

                }
            }
        }

        private string MakeChatRoomName()
        {
            string name = $"Friends_{Gaos.Context.Authentication.GetUserName()}_{FriendUsername}";
            return name;

        }

        private async UniTask EnsureChatRoomExists()
        {
            if (ChatRoomId == -1)
            {
                ChatRoomName = MakeChatRoomName();
                Gaos.Routes.Model.ChatRoomJson.CreateChatRoomResponse response = await Gaos.ChatRoom.ChatRoom.CreateChatRoom.CallAsync(ChatRoomName);
                ChatRoomId = response.ChatRoomId;
            }
        }

        private void  OnEnable()
        {
            ReadMessagesLoop().Forget();

        }
        private void  OnDisable()
        {
            IsFinished = true;
            ReadMessagesLoopWaitCancellationTokenSource.Cancel();
        }
    }
}
