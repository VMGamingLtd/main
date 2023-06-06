using UnityEngine;
using TMPro;

public class TargetUsernameLoad : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    public MessageList messageList;

    private void Awake()
    {
        nameText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (messageList != null)
        {
            string targetUsername = messageList.targetUsername;
            nameText.text = targetUsername;
        }
        else
        {
            Debug.LogWarning("MessageList script not found.");
        }
    }
}
