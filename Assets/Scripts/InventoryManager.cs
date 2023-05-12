using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] RawItems;
    public GameObject[] IntermediateItems;
    public GameObject[] AssembledItems;
    public GameObject[] UtilityItems;
    public TextMeshProUGUI[] ItemCountText;

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
        else if (showItemTypes == "INTERMEDIATE")
        {
            ShowIntermediateItems();
        }
        else if (showItemTypes == "ASSEMBLED")
        {
            ShowAssembledItems();
        }
        else if (showItemTypes == "UTILITY")
        {
            ShowUtilityItems();
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

        foreach (GameObject utilityItem in UtilityItems)
        {
            if (HasChildTextGreaterThanZero(utilityItem))
            {
                utilityItem.SetActive(true);
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

        foreach (GameObject utilityItem in UtilityItems)
        {
            {
                utilityItem.SetActive(false);
            }
        }
    }

    public void ShowIntermediateItems()
    {
        InventoryManager.ShowItemTypes = "INTERMEDIATE";

        foreach (GameObject rawItem in RawItems)
        {
            {
                rawItem.SetActive(false);
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
            {
                assembledItem.SetActive(false);
            }
        }

        foreach (GameObject utilityItem in UtilityItems)
        {
            {
                utilityItem.SetActive(false);
            }
        }
    }

    public void ShowAssembledItems()
    {
        InventoryManager.ShowItemTypes = "ASSEMBLED";

        foreach (GameObject rawItem in RawItems)
        {
            {
                rawItem.SetActive(false);
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
            if (HasChildTextGreaterThanZero(assembledItem))
            {
                assembledItem.SetActive(true);
            }
        }

        foreach (GameObject utilityItem in UtilityItems)
        {
            {
                utilityItem.SetActive(false);
            }
        }
    }

    public void ShowUtilityItems()
    {
        InventoryManager.ShowItemTypes = "UTILITY";

        foreach (GameObject rawItem in RawItems)
        {
            {
                rawItem.SetActive(false);
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

        foreach (GameObject utilityItem in UtilityItems)
        {
            if (HasChildTextGreaterThanZero(utilityItem))
            {
                utilityItem.SetActive(true);
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

    public void AddToAssembledItems(GameObject newItem)
    {
        // Create a new array with increased size to accommodate the new item
        GameObject[] newAssembledItems = new GameObject[AssembledItems.Length + 1];

        // Copy existing items to the new array
        for (int i = 0; i < AssembledItems.Length; i++)
        {
            newAssembledItems[i] = AssembledItems[i];
        }

        // Add the new item to the end of the new array
        newAssembledItems[newAssembledItems.Length - 1] = newItem;

        // Update the reference to the new array
        AssembledItems = newAssembledItems;
    }
    public void AddToUtilityItems(GameObject newItem)
    {
        // Create a new array with increased size to accommodate the new item
        GameObject[] newUtilityItems = new GameObject[UtilityItems.Length + 1];

        // Copy existing items to the new array
        for (int i = 0; i < UtilityItems.Length; i++)
        {
            newUtilityItems[i] = UtilityItems[i];
        }

        // Add the new item to the end of the new array
        newUtilityItems[newUtilityItems.Length - 1] = newItem;

        // Update the reference to the new array
        UtilityItems = newUtilityItems;
    }

}
