
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

        private int ChatRoomId = -1;
        private string ChatRoomName;

        private bool IsFinished = false;

        private string FriendUsername;
        public GameObject FriendListButton;

        public void SetFriendUserName(string username)
        {
            FriendUsername = username;

            Transform nicknameObject = FriendListButton.transform.Find("Info/Nickname");
            TextMeshProUGUI nicknameObjectTMP = nicknameObject.GetComponent<TextMeshProUGUI>();
            nicknameObjectTMP.text = FriendUsername;
        }

        public int GetChatRoomId()
        {
            return ChatRoomId;
        }

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

                Message message = messageLine.GetComponent<Message>();
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
            if (AllMessages.Count  >  0)
            {
                int lastMessageIdx = AllMessages.Last.Value.message.MessageId;
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
                for (var i = response.Messages.Length -1; i >= 0; i--)
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

        public void WakeupReadMessagesLoop()
        {
            if (ReadMessagesLoopWaitCancellationTokenSource != null)
            {
                ReadMessagesLoopWaitCancellationTokenSource.Cancel();
            }
        }



        private async UniTaskVoid ReadMessagesLoop()
        {
            Debug.Log("ReadMessagesLoop: starting"); //@@@@@@@@@@@@@@@@@@
            await EnsureChatRoomExists();

            ReadMessagesLoopWaitCancellationTokenSource = new System.Threading.CancellationTokenSource();
            while (true)
            {

                // read last messages
                int cntBefore = AllMessages.Count;
                int firstMessageIdBefore = AllMessages.Count > 0 ? AllMessages.First.Value.message.MessageId : -1;
                await readLastMessages();
                if (IsFinished)
                {
                    Debug.Log("ReadMessagesLoop: IsFinished"); //@@@@@@@@@@@@@@@@@@
                    break;
                }
                int cntAfter = AllMessages.Count;
                int firstMessageIdAfter = AllMessages.Count > 0 ? AllMessages.First.Value.message.MessageId : -1;
                if (cntBefore != cntAfter)
                {
                    DisplayAllMessages();
                }
                else if (firstMessageIdBefore != firstMessageIdAfter)
                {
                    DisplayAllMessages();
                }

                // read previous messages
                if (AllMessages.Count  < MAX_SCROLL_LIST_LINES_COUNT)
                {
                    cntBefore = AllMessages.Count;
                    firstMessageIdBefore = AllMessages.Count > 0 ? AllMessages.First.Value.message.MessageId : -1;
                    await readPreviousMessages();
                    if (IsFinished)
                    {
                        Debug.Log("ReadMessagesLoop: IsFinished"); //@@@@@@@@@@@@@@@@@@
                        break;
                    }
                    cntAfter = AllMessages.Count;
                    firstMessageIdAfter = AllMessages.Count > 0 ? AllMessages.First.Value.message.MessageId : -1;
                    if (cntBefore != cntAfter)
                    {
                        DisplayAllMessages();
                    }
                    else if (firstMessageIdBefore != firstMessageIdAfter)
                    {
                        DisplayAllMessages();
                    }

                }

                // sleep
                try
                {
                    await UniTask.Delay(System.TimeSpan.FromSeconds(5), ignoreTimeScale: false, PlayerLoopTiming.Update, ReadMessagesLoopWaitCancellationTokenSource.Token);
                }
                catch (System.OperationCanceledException)
                {
                    Debug.Log("ReadMessagesLoop: OperationCanceledException"); //@@@@@@@@@@@@@@@@@@
                    if (IsFinished)
                    {
                        Debug.Log("ReadMessagesLoop: IsFinished"); //@@@@@@@@@@@@@@@@@@
                        break;
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
            int previousChatRoomId = ChatRoomId;
            Debug.Log($"EnsureChatRoomExists(): previousChatRoomId: {previousChatRoomId}"); //@@@@@@@@@@@@@@@@@@
            ChatRoomName = MakeChatRoomName();
            Debug.Log($"EnsureChatRoomExists(): ChatRoomName: {ChatRoomName}"); //@@@@@@@@@@@@@@@@@@
            ChatRoomId = await Gaos.ChatRoom.ChatRoom.EnsureChatRoomExists.CallAsync(ChatRoomName);
            Debug.Log($"EnsureChatRoomExists(): ChatRoomId: {ChatRoomId}"); //@@@@@@@@@@@@@@@@@@
            if (ChatRoomId != previousChatRoomId)
            {
                AllMessages.Clear();
                DisplayAllMessages();
            }
        }

        private void  OnEnable()
        {
            Debug.Log("OnEnable()"); //@@@@@@@@@@@@@@@@@@
            IsFinished = false;
            ReadMessagesLoop().Forget();

        }
        private void  OnDisable()
        {
            Debug.Log("OnDisable()"); //@@@@@@@@@@@@@@@@@@
            IsFinished = true;
            WakeupReadMessagesLoop();
        }
    }
}
