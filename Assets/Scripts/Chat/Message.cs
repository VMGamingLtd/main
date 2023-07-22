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

        public void OnEnable()
        {
            dateText = transform.Find("Header/Date").GetComponent<TMP_Text>();
            if (dateText == null)
            {
                throw new System.Exception("cannot find dateText");
            }
            usernameText = transform.Find("Header/Username").GetComponent<TMP_Text>();
            if (usernameText == null)
            {
                throw new System.Exception("cannot find usernameText");
            }
            messageText = transform.Find("Header/TextLine").GetComponent<TMP_Text>();
            if (dateText == null)
            {
                throw new System.Exception("cannot find dateText");
            }
        }
    }
}
