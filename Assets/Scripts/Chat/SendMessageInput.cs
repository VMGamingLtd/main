using UnityEngine;
using TMPro;

public class SendMessageInput : MonoBehaviour
{
    public MessageList messageList;
    public GameObject chatMessageTemplate;

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
            messageList.CreateMessage(messageContent);

            if (chatMessageTemplate != null)
            {
                // Create a new game object based on the chatMessageTemplate
                GameObject newMessageObject = Instantiate(chatMessageTemplate);

                // Get the Message component from the new message object
                Message messageComponent = newMessageObject.GetComponent<Message>();
                if (messageComponent != null)
                {
                    // Get the TMP_Text component from the Message component
                    TMP_Text messageTextComponent = messageComponent.GetComponentInChildren<TMP_Text>();
                    if (messageTextComponent != null)
                    {
                        // Assign the message content to the text property of the TMP_Text component
                        messageTextComponent.text = messageContent;
                    }
                    else
                    {
                        Debug.LogWarning("ChatMessageTemplate does not have a TMP_Text component.");
                    }
                }
                else
                {
                    Debug.LogWarning("ChatMessageTemplate does not have the Message component attached.");
                }
            }
            else
            {
                Debug.LogWarning("ChatMessageTemplate is not assigned.");
            }

            inputField.text = string.Empty;
        }
    }


}
