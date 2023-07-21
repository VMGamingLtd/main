using UnityEngine;
using TMPro;

namespace Chat
{
    public class Message : MonoBehaviour
    {
        public TMP_Text dateText;
        public TMP_Text usernameText;
        public TMP_Text messageText;

        public void SetContent(string date, string username, string message)
        {
            dateText.text = date;
            usernameText.text = username;
            messageText.text = message;
        }
    }
}
