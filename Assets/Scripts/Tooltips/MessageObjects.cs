using ItemManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageObjects : MonoBehaviour
{
    public Dictionary<string, GameObject> messageMap;
    public InventoryManager inventoryManager;
    public ItemCreator itemCreator;
    private SliderManager sliderManager;

    private void Start()
    {
        // Initialize the tooltip map
        messageMap = new Dictionary<string, GameObject>
        {
            {"OxygenTankEquipFail", OxygenTankEquipFail},
            {"ServerConnectionLost", ServerConnectionLost},
            {"IncorrectCredentials", IncorrectCredentials},
            {"SplitWindow", SplitWindow},
            {"SplitItemFail", SplitItemFail},
            {"DeleteSlotWindow", DeleteSlotWindow }
        };
    }
    public GameObject OxygenTankEquipFail;
    public GameObject ServerConnectionLost;
    public GameObject IncorrectCredentials;
    public GameObject SplitWindow;
    public GameObject SplitItemFail;
    public GameObject DeleteSlotWindow;

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
            string objType = sliderManager.GetCurrentItemType();
            int objID = sliderManager.GetCurrentObjID();
            int index = sliderManager.GetCurrentObjIndex();

            inventoryManager.ReduceSplitItemQuantity(objName, objProduct, quantity, objID);
            if (objType == "FABRICATOR")
            {
                itemCreator.SplitTool(index, quantity);
            }
            else if (objType == "HELMET")
            {
                itemCreator.SplitHelmet(index, quantity);
            }
            else if (objType == "SUIT")
            {
                itemCreator.SplitSuit(index, quantity);
            }
            else
            {
                itemCreator.SplitItem(index, quantity);
            }

        }
        HideAllMessages();
    }
    public void DisplayMessage(string objectName, int? slotNumber = null)
    {
        if (messageMap.TryGetValue(objectName, out GameObject messageObject))
        {
            messageObject.SetActive(true);

            if (slotNumber != null)
            {
                messageObject.transform.Find("TitleContext/SlotNumber").GetComponent<TextMeshProUGUI>().text = slotNumber.ToString();
            }
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
