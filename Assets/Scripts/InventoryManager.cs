using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.NetworkInformation;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] RawItems;
    public GameObject[] IntermediateItems;
    public GameObject[] AssembledItems;

    public static string ShowItemTypes = "ALL";

    void OnEnable()
    {
        string showItemTypes = InventoryManager.ShowItemTypes;

        if (showItemTypes == "ALL")
        {
            ShowAllItems();
        }
        else if (showItemTypes == "RAW")
        {
            ShowRawItems();
        }
    }

    public void ShowAllItems()
    {
        InventoryManager.ShowItemTypes = "ALL";

        foreach (GameObject rawItem in RawItems)
        {
            if (HasChildTextGreaterThanZero(rawItem))
            {
                rawItem.SetActive(true);
            }  
        }

        foreach (GameObject intermediateItem in IntermediateItems)
        {
            if (HasChildTextGreaterThanZero(intermediateItem))
            {
                intermediateItem.SetActive(true);
            }      
        }

        foreach (GameObject assembledItem in AssembledItems)
        {
            if (HasChildTextGreaterThanZero(assembledItem))
            {
                assembledItem.SetActive(true);
            }     
        }
    }

    public void ShowRawItems()
    {
        InventoryManager.ShowItemTypes = "RAW";

        foreach (GameObject rawItem in RawItems)
        {
            if (HasChildTextGreaterThanZero(rawItem))
            {
                rawItem.SetActive(true);
            }
        }

        foreach (GameObject intermediateItem in IntermediateItems)
        {           
            {
                intermediateItem.SetActive(false);
            }
        }

        foreach (GameObject assembledItem in AssembledItems)
        {
            {
                assembledItem.SetActive(false);
            }
        }
    }

    private bool HasChildTextGreaterThanZero(GameObject parentObject)
    {
        TextMeshProUGUI childText = parentObject.GetComponentInChildren<TextMeshProUGUI>();
        if (childText != null && int.TryParse(childText.text, out int count))
        {
            return count > 0;
        }
        return false;
    }

}
