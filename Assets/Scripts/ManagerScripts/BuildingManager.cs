using System.Collections;
using System.Collections.Generic;
using BuildingManagement;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class BuildingManager : MonoBehaviour
{
    public Dictionary<string, GameObject[]> buildingArrays;
    public BuildingCreator buildingCreator;
    public static string ShowBuildingTypes = "ALL";
    public static bool isDraggingBuilding = false;
    public InventoryManager inventoryManagerRef;
    public CoroutineManager coroutineManagerRef;

    public void PopulateBuildingArrays()
    {
        buildingArrays = new Dictionary<string, GameObject[]>
        {
            { "BASIC", new GameObject[0] },
            { "PROCESSED", new GameObject[0] },
            { "ENHANCED", new GameObject[0] },
            { "ASSEMBLED", new GameObject[0] }
        };
    }
    void OnEnable()
    {
        ShowItems(BuildingManager.ShowBuildingTypes);
    }

    public void AddToItemArray(string itemType, GameObject item)
    {
        item.transform.SetParent(transform);
        // Check if the item type already exists in the buildingArrays dictionary
        if (buildingArrays.ContainsKey(itemType))
        {
            // Update the existing item

            // Get the existing array of items for the item type
            GameObject[] itemArray = buildingArrays[itemType];

            // Create a new array with increased length to accommodate the new item
            GameObject[] newArray = new GameObject[itemArray.Length + 1];

            // Copy the existing items to the new array
            itemArray.CopyTo(newArray, 0);

            // Add the new item to the new array
            newArray[newArray.Length - 1] = item;

            // Update the itemArray reference in the dictionary
            buildingArrays[itemType] = newArray;
        }
        else
        {
            // Create a new array with a single item
            GameObject[] itemArray = new GameObject[] { item };

            // Add the item array to the dictionary with the item type as the key
            buildingArrays.Add(itemType, itemArray);
        }
    }

    public void RemoveFromItemArray(string itemType, GameObject item)
    {
        // Check if the item type exists in the buildingArrays dictionary
        if (buildingArrays.ContainsKey(itemType))
        {
            // Get the array of items for the item type
            GameObject[] itemArray = buildingArrays[itemType];

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
                buildingArrays[itemType] = newArray;
            }
        }
    }
    public void ShowFilteredItems()
    {
        ShowItems(BuildingManager.ShowBuildingTypes);
    }
    public void ShowItems(string itemType)
    {
        bool showAllTypes = itemType == "ALL";

        foreach (var kvp in buildingArrays)
        {
            string product = kvp.Key;
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                BuildingItemData itemData = item.GetComponent<BuildingItemData>();

                bool showItem =
                    (showAllTypes || itemType == itemData.itemType);

                item.SetActive(showItem);
            }
        }
    }
}
