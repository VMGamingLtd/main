using UnityEngine;
using TMPro;

public class ChatOpener : MonoBehaviour
{
    public MessageList messageList;

    public void OpenChatWithUser()
    {
        Transform parent = transform.parent;
        Transform usernameObject = parent.Find("Info/FriendUsername");

        if (usernameObject != null)
        {
            TextMeshProUGUI usernameText = usernameObject.GetComponent<TextMeshProUGUI>();

            if (usernameText != null)
            {
                messageList.targetUsername = usernameText.text;
                Debug.Log("Opened chat with user: " + messageList.targetUsername);
            }
            else
            {
                Debug.LogWarning("FriendUsername object does not have TextMeshProUGUI component.");
            }
        }
        else
        {
            Debug.LogWarning("Info/FriendUsername object not found in parent.");
        }
    }
    public void GetTargetUsername()
    {
        //return messageList.targetUsername;
    }
}
