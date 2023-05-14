using System.Collections;
using System.Collections.Generic;
using ItemManagement;
using UnityEngine;
using TMPro;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, GameObject[]> itemArrays;
    public GameObject[] RawItems;
    public GameObject[] IntermediateItems;
    public GameObject[] AssembledItems;
    public GameObject[] UtilityItems;
    public ItemCreator itemCreator;

    public static string ShowItemTypes = "ALL";

    public void PopulateInventoryArrays()
    {
        // Initialize the dictionary
        itemArrays = new Dictionary<string, GameObject[]>();
        itemArrays.Add("RAW", new GameObject[0]);
        itemArrays.Add("INTERMEDIATE", new GameObject[0]);
        itemArrays.Add("ASSEMBLED", new GameObject[0]);
        itemArrays.Add("UTILITY", new GameObject[0]);
    }
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

    public void AddToItemArray(string itemType, GameObject item)
    {
        item.transform.SetParent(transform);
        // Check if the item type already exists in the itemArrays dictionary
        if (itemArrays.ContainsKey(itemType))
        {
            // Update the existing item

            // Get the existing array of items for the item type
            GameObject[] itemArray = itemArrays[itemType];

            // Create a new array with increased length to accommodate the new item
            GameObject[] newArray = new GameObject[itemArray.Length + 1];

            // Copy the existing items to the new array
            itemArray.CopyTo(newArray, 0);

            // Add the new item to the new array
            newArray[newArray.Length - 1] = item;

            // Update the itemArray reference in the dictionary
            itemArrays[itemType] = newArray;
        }
        else
        {
            // Add the item to the itemArrays dictionary

            // Create a new array with a single item
            GameObject[] itemArray = new GameObject[] { item };

            // Add the item array to the dictionary with the item type as the key
            itemArrays.Add(itemType, itemArray);
        }

        // Get or add the ItemData component to the item
        ItemData itemData = item.GetComponent<ItemData>();
        if (itemData == null)
        {
            itemData = item.AddComponent<ItemData>();
        }
        /*Debug.Log($"itemArrays[{itemType}] contents:");
        foreach (GameObject itemObj in itemArrays[itemType])
        {
            Debug.Log(itemObj.name);
        }*/
    }



    public void IncreaseItemQuantity(string prefabName, GameObject item, ItemData itemData) // this function is initiated only after production
    {
        // Check if the item exists in the dictionary
        if (itemArrays.ContainsKey(prefabName))
        {
            // Get the array of items for the prefabName
            GameObject[] itemArray = itemArrays[prefabName];

            // Find the index of the item in the array
            int index = System.Array.IndexOf(itemArray, item);

            // Check if the item is present in the array
            if (index != -1)
            {
                // Increase the item quantity
                itemData.itemQuantity++;

                // Update the item in the array
                itemArray[index] = item;
            }
        }
    }

    public bool CheckItemQuantity(string prefabName, string itemType, int quantityThreshold)
    {
        if (itemArrays.TryGetValue(itemType, out GameObject[] itemArray))
        {
            foreach (GameObject item in itemArray)
            {
                if (item.name == prefabName + "(Clone)")
                {
                    ItemData itemData = item.GetComponent<ItemData>();

                    if (itemData != null && itemData.itemQuantity >= quantityThreshold)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public int GetItemQuantity(string prefabName, string itemType)
    {
        if (itemArrays.TryGetValue(itemType, out GameObject[] foundItemArray))
        {
            foreach (GameObject item in foundItemArray)
            {
                if (item.name == prefabName + "(Clone)")
                {
                    ItemData itemData = item.GetComponent<ItemData>();
                    if (itemData != null)
                    {
                        return itemData.itemQuantity;
                    }
                }
            }
        }

        return 0;
    }
    public void AddItemQuantity(string prefabName, string itemType, int quantity) // changes quantity of already instantiated product if it doesn't exist, creates new one
    {
        if (itemArrays.TryGetValue(itemType, out GameObject[] itemArray))
        {
            foreach (GameObject itemPrefab in itemArray)
            {
                if (itemPrefab.name == prefabName + "(Clone)")
                {
                    ItemData itemData = itemPrefab.GetComponent<ItemData>();
                    itemData.itemQuantity += quantity;

                    TextMeshProUGUI countText = itemPrefab.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                    if (countText != null)
                    {
                        countText.text = itemData.itemQuantity.ToString();
                    }

                    return; // Exit the function since item was found and quantity was updated
                }
            }
        }
        if (prefabName == "Plants")
        {
            itemCreator.CreatePlants(quantity);
        }
        else if (prefabName == "Water")
        {
            itemCreator.CreateWater(quantity);
        }
        else if (prefabName == "Biofuel")
        {
            itemCreator.CreateBiofuel(quantity);
        }
        else if (prefabName == "WaterBottle")
        {
            itemCreator.CreateWaterBottle(quantity);
        }
        else if (prefabName == "Battery")
        {
            itemCreator.CreateBattery(quantity);
        }
    }
    public void ReduceItemQuantity(string prefabName, string itemType, int quantity)
    {
        if (itemArrays.TryGetValue(itemType, out GameObject[] itemArray))
        {
            foreach (GameObject itemPrefab in itemArray)
            {
                if (itemPrefab.name == prefabName + "(Clone)")
                {
                    ItemData itemData = itemPrefab.GetComponent<ItemData>();
                    itemData.itemQuantity -= quantity;

                    TextMeshProUGUI countText = itemPrefab.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                    if (countText != null)
                    {
                        countText.text = itemData.itemQuantity.ToString();
                    }

                    // Ensure the item quantity doesn't go below zero
                    if (itemData.itemQuantity <= 0)
                    {
                        itemArrays[itemType] = itemArrays[itemType].Where(item => item != itemPrefab).ToArray();
                        Destroy(itemPrefab);
                    }

                    return;
                }
            }
        }
    }


    public void InventoryCheckUp()
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
        newItem.transform.SetParent(transform);
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
        newItem.transform.SetParent(transform);
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
