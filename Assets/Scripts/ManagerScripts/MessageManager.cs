using System;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public GameObject eventMessage;
    public Transform eventMessageList;

    /// <summary>
    /// Creates a game object that represents a message in the Exploration menu - Event messages.
    /// </summary>
    /// <param name="messageText"></param>
    public void CreateEventMessage(string messageText)
    {
        GameObject newMessage = Instantiate(eventMessage, eventMessageList);
        newMessage.transform.Find("Timestamp").GetComponent<TextMeshProUGUI>().text = DateTime.Now.ToString("dd/mm/yyyy hh:mm:ss");
        newMessage.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = messageText;
        newMessage.transform.position = new Vector3(newMessage.transform.position.x, newMessage.transform.position.y, 0f);
        newMessage.transform.localScale = Vector3.one;
    }
}
