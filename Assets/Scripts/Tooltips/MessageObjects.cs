using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageObjects : MonoBehaviour
{
    public Dictionary<string, GameObject> messageMap;

    private void Start()
    {
        // Initialize the tooltip map
        messageMap = new Dictionary<string, GameObject>
        {
            {"OxygenTankEquipFail", OxygenTankEquipFail},
            {"ServerConnectionLost", ServerConnectionLost},
            {"IncorrectCredentials", IncorrectCredentials},
        };
    }
    public GameObject OxygenTankEquipFail;
    public GameObject ServerConnectionLost;
    public GameObject IncorrectCredentials;


    public void DisplayMessage(string objectName)
    {
        if (messageMap.TryGetValue(objectName, out GameObject messageObject))
        {
            messageObject.SetActive(true);
        }
    }
    public void HideAllMessages()
    {
        foreach (var kvp in messageMap)
        {
            kvp.Value.SetActive(false);
        }
    }

}
