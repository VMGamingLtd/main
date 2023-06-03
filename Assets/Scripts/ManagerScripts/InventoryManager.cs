using System.Collections;
using System.Collections.Generic;
using ItemManagement;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, GameObject[]> itemArrays;
    public ItemCreator itemCreator;
    public EquipmentManager equipmentManager;

    public static string ShowItemProducts = "ALL";
    public static string ShowItemTypes = "ALL";
    public static string ShowItemClass = "ALL";
    public void PopulateInventoryArrays()
    {
        itemArrays = new Dictionary<string, GameObject[]>
        {
            { "BASIC", new GameObject[0] },
            { "PROCESSED", new GameObject[0] },
            { "REFINED", new GameObject[0] },
            { "ASSEMBLED", new GameObject[0] }
        };
    }
    public void AddToItemArray(string itemProduct, GameObject item)
    {
        item.transform.SetParent(transform);
        // Check if the item type already exists in the itemArrays dictionary
        if (itemArrays.ContainsKey(itemProduct))
        {
            // Update the existing item

            // Get the existing array of items for the item type
            GameObject[] itemArray = itemArrays[itemProduct];

            // Create a new array with increased length to accommodate the new item
            GameObject[] newArray = new GameObject[itemArray.Length + 1];

            // Copy the existing items to the new array
            itemArray.CopyTo(newArray, 0);

            // Add the new item to the new array
            newArray[newArray.Length - 1] = item;

            // Update the itemArray reference in the dictionary
            itemArrays[itemProduct] = newArray;
        }
        else
        {
            // Add the item to the itemArrays dictionary

            // Create a new array with a single item
            GameObject[] itemArray = new GameObject[] { item };

            // Add the item array to the dictionary with the item type as the key
            itemArrays.Add(itemProduct, itemArray);
        }

        // Get or add the ItemData component to the item
        ItemData itemData = item.GetComponent<ItemData>();
        if (itemData == null)
        {
            itemData = item.AddComponent<ItemData>();
        }
    }

    public void RemoveFromItemArray(string itemProduct, GameObject item)
    {
        // Check if the item type exists in the itemArrays dictionary
        if (itemArrays.ContainsKey(itemProduct))
        {
            // Get the array of items for the item type
            GameObject[] itemArray = itemArrays[itemProduct];

            // Find the index of the item in the array
            int index = Array.IndexOf(itemArray, item);

            if (index >= 0)
            {
                // Create a new array with reduced length
                GameObject[] newArray = new GameObject[itemArray.Length - 1];

                // Copy the items before the removed item
                Array.Copy(itemArray, 0, newArray, 0, index);

                // Copy the items after the removed item
                Array.Copy(itemArray, index + 1, newArray, index, itemArray.Length - index - 1);

                // Update the itemArray reference in the dictionary
                itemArrays[itemProduct] = newArray;
            }
            else
            {
                Debug.LogWarning("Item not found in the item array for the specified item type.");
            }
        }
        else
        {
            Debug.LogWarning("Item type not found in the itemArrays dictionary.");
        }
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

    public bool CheckItemQuantity(string prefabName, string itemProduct, int quantityThreshold)
    {
        if (itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray))
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
    public int GetItemQuantity(string prefabName, string itemProduct)
    {
        if (itemArrays.TryGetValue(itemProduct, out GameObject[] foundItemArray))
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
    public void AddItemQuantity(string prefabName, string itemProduct, int quantity) // changes quantity of already instantiated product if it doesn't exist, creates new one
    {
        if (itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray))
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
        if (prefabName == "FibrousLeaves")
        {
            itemCreator.CreatePlants(quantity);
        }
        else if (prefabName == "AlienWater")
        {
            itemCreator.CreateWater(quantity);
        }
        else if (prefabName == "Biofuel")
        {
            itemCreator.CreateBiofuel(quantity);
        }
        else if (prefabName == "PurifiedWater")
        {
            itemCreator.CreatePurifiedWater(quantity);
        }
        else if (prefabName == "Battery")
        {
            itemCreator.CreateBattery(quantity);
        }
        else if (prefabName == "BatteryCore")
        {
            itemCreator.CreateBatteryCore(quantity);
        }
    }
    public void ReduceItemQuantity(string prefabName, string itemProduct, int quantity)
    {
        if (itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray))
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
                        itemArrays[itemProduct] = itemArrays[itemProduct].Where(item => item != itemPrefab).ToArray();
                        Destroy(itemPrefab);
                    }

                    return;
                }
            }
        }
    }
    public void ShowFilteredItems()
    {
        ShowItems(InventoryManager.ShowItemProducts, InventoryManager.ShowItemTypes, InventoryManager.ShowItemClass);
    }
    public void ShowItems(string itemProduct, string itemType, string itemClass)
    {
        bool showAllProducts = itemProduct == "ALL";
        bool showAllTypes = itemType == "ALL";
        bool showAllClasses = itemClass == "ALL";

        foreach (var kvp in itemArrays)
        {
            string product = kvp.Key;
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                ItemData itemData = item.GetComponent<ItemData>();

                bool showItem =
                    (showAllProducts || itemProduct == itemData.itemProduct) &&
                    (showAllTypes || itemType == itemData.itemType) &&
                    (showAllClasses || itemClass == itemData.itemClass);

                item.SetActive(showItem);
            }
        }
    }
}
