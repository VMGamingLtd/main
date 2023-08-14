using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemManagement;

public class MessageObjects : MonoBehaviour
{
    public Dictionary<string, GameObject> messageMap;
    public InventoryManager inventoryManager;
    private SliderManager sliderManager;

    private void Start()
    {
        // Initialize the tooltip map
        messageMap = new Dictionary<string, GameObject>
        {
            {"OxygenTankEquipFail", OxygenTankEquipFail},
            {"ServerConnectionLost", ServerConnectionLost},
            {"IncorrectCredentials", IncorrectCredentials},
            {"SplitWindow", SplitWindow}
        };
    }
    public GameObject OxygenTankEquipFail;
    public GameObject ServerConnectionLost;
    public GameObject IncorrectCredentials;
    public GameObject SplitWindow;

    public void DisplaySplitWindow(ItemData itemData, string objName)
    {
        if (messageMap.TryGetValue("SplitWindow", out GameObject messageObject))
        {
            messageObject.SetActive(true);
            sliderManager = messageObject.GetComponentInChildren<SliderManager>();
            sliderManager.InitializeSlider(itemData, objName);
        }
    }
    public void SplitItem()
    {
        float quantity = sliderManager.GetCurrentSplitQuantity();
        if (quantity > 0)
        {
            string objName = sliderManager.GetCurrentObjName();
            string objProduct = sliderManager.GetCurrentItemProduct();
            int objID = sliderManager.GetCurrentObjID();

            inventoryManager.ReduceSplitItemQuantity(objName, objProduct, quantity, objID);
            inventoryManager.SplitItem(objName, quantity);
        }
        HideAllMessages();
    }
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
