using ItemManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, GameObject[]> itemArrays;
    public ItemCreator itemCreator;
    public EquipmentManager equipmentManager;

    // this variable is for DragAndDrop.cs that is in every item so it is better nested here
    public static bool endingDrag = false;

    public static string ShowItemProducts = "ALLITEMS";
    public static string ShowItemTypes = "ALLTYPES";
    public static string ShowItemClass = "ALLCLASSES";
    public void PopulateInventoryArrays()
    {
        itemArrays = new Dictionary<string, GameObject[]>
        {
            { "BASIC", new GameObject[0] },
            { "PROCESSED", new GameObject[0] },
            { "ENHANCED", new GameObject[0] },
            { "ASSEMBLED", new GameObject[0] }
        };
    }
    public void SortItemRecipeArraysByOrderAdded()
    {
        List<Transform> childTransforms = new();
        foreach (Transform child in transform)
        {
            childTransforms.Add(child);
        }

        // Sort the child GameObjects based on their 'orderAdded' value
        childTransforms.Sort(CompareByOrderAdded);

        // Remove all child GameObjects from the parent
        foreach (Transform child in childTransforms)
        {
            child.SetParent(null);
        }

        // Add the sorted child GameObjects back to the parent
        foreach (Transform child in childTransforms)
        {
            child.SetParent(transform);
        }
    }

    private int CompareByOrderAdded(Transform t1, Transform t2)
    {
        ItemData ItemData1 = t1.GetComponent<ItemData>();
        ItemData ItemData2 = t2.GetComponent<ItemData>();

        if (ItemData1 != null && ItemData2 != null)
        {
            return ItemData1.ID.CompareTo(ItemData2.ID);
        }
        return 0;
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
            _ = item.AddComponent<ItemData>();
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
    public bool CheckItemQuantity(string prefabName, string itemProduct, float quantityThreshold)
    {
        if (itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray))
        {
            foreach (GameObject item in itemArray)
            {
                if (item.name == prefabName)
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
    public float GetItemQuantity(string prefabName, string itemProduct)
    {
        float totalQuantity = 0;

        if (itemArrays.TryGetValue(itemProduct, out GameObject[] foundItemArray))
        {
            foreach (GameObject item in foundItemArray)
            {
                if (item.name == prefabName)
                {
                    ItemData itemData = item.GetComponent<ItemData>();
                    if (itemData != null)
                    {
                        totalQuantity += itemData.itemQuantity;
                    }
                }
            }
        }

        return totalQuantity;
    }

    public void AddItemQuantity(string prefabName, string itemProduct, float quantity, int? index = null) // changes quantity of already instantiated product if it doesn't exist, creates new one
    {
        if (itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray))
        {
            foreach (GameObject itemPrefab in itemArray)
            {
                if (itemPrefab.name == prefabName)
                {
                    ItemData itemData = itemPrefab.GetComponent<ItemData>();
                    itemData.itemQuantity += quantity;

                    TextMeshProUGUI countText = itemPrefab.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                    if (countText != null)
                    {
                        countText.text = itemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
                    }

                    return; // Exit the function since item was found and quantity was updated
                }
            }
        }
        else
        {
            itemCreator.CreateItem((int)index, quantity);
        }
    }
    public void ReduceSplitItemQuantity(string prefabName, string itemProduct, float quantity, int objID)
    {
        if (itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray))
        {
            foreach (GameObject itemPrefab in itemArray)
            {
                if (itemPrefab.name == prefabName)
                {
                    ItemData itemData = itemPrefab.GetComponent<ItemData>();
                    if (itemData.ID == objID)
                    {
                        itemData.itemQuantity -= quantity;

                        TextMeshProUGUI countText = itemPrefab.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                        if (countText != null)
                        {
                            countText.text = itemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
                        }
                        return;
                    }

                }
            }
        }
    }
    public void DestroySpecificItem(string prefabName, string itemProduct, int ID)
    {
        if (itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray))
        {
            foreach (GameObject itemPrefab in itemArray)
            {
                if (itemPrefab.name == prefabName)
                {
                    ItemData itemData = itemPrefab.GetComponent<ItemData>();

                    if (itemData.ID == ID)
                    {
                        Debug.Log($"Deleting item with ID {itemData.ID}");
                        itemArrays[itemProduct] = itemArrays[itemProduct].Where(item => item != itemPrefab).ToArray();
                        Destroy(itemPrefab);
                        return;
                    }
                }
            }
        }
    }
    public void ReduceItemQuantity(string prefabName, string itemProduct, float quantity)
    {
        if (itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray))
        {
            foreach (GameObject itemPrefab in itemArray)
            {
                if (itemPrefab.name == prefabName)
                {
                    ItemData itemData = itemPrefab.GetComponent<ItemData>();
                    itemData.itemQuantity -= quantity;

                    TextMeshProUGUI countText = itemPrefab.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                    if (countText != null)
                    {
                        countText.text = itemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
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
        ShowItems(ShowItemProducts, ShowItemTypes, ShowItemClass);
    }
    public void ShowItems(string itemProduct, string itemType, string itemClass)
    {
        bool showAllProducts = itemProduct == "ALLITEMS";
        bool showAllTypes = itemType == "ALLTYPES";
        bool showAllClasses = itemClass == "ALLCLASSES";

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
