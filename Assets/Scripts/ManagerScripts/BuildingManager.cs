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

    public static string ShowBuildingProducts = "ALL";
    public static string ShowBuildingTypes = "ALL";
    public static string ShowBuildingClass = "ALL";

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
        ShowItems(BuildingManager.ShowBuildingProducts, BuildingManager.ShowBuildingTypes, BuildingManager.ShowBuildingClass);
    }

    public void AddToItemArray(string itemProduct, GameObject item)
    {
        item.transform.SetParent(transform);
        // Check if the item type already exists in the buildingArrays dictionary
        if (buildingArrays.ContainsKey(itemProduct))
        {
            // Update the existing item

            // Get the existing array of items for the item type
            GameObject[] itemArray = buildingArrays[itemProduct];

            // Create a new array with increased length to accommodate the new item
            GameObject[] newArray = new GameObject[itemArray.Length + 1];

            // Copy the existing items to the new array
            itemArray.CopyTo(newArray, 0);

            // Add the new item to the new array
            newArray[newArray.Length - 1] = item;

            // Update the itemArray reference in the dictionary
            buildingArrays[itemProduct] = newArray;
        }
        else
        {
            // Add the item to the buildingArrays dictionary

            // Create a new array with a single item
            GameObject[] itemArray = new GameObject[] { item };

            // Add the item array to the dictionary with the item type as the key
            buildingArrays.Add(itemProduct, itemArray);
        }

        // Get or add the ItemData component to the item
        BuildingItemData itemData = item.GetComponent<BuildingItemData>();
        if (itemData == null)
        {
            itemData = item.AddComponent<BuildingItemData>();
        }
    }

    public void RemoveFromItemArray(string itemProduct, GameObject item)
    {
        // Check if the item type exists in the buildingArrays dictionary
        if (buildingArrays.ContainsKey(itemProduct))
        {
            // Get the array of items for the item type
            GameObject[] itemArray = buildingArrays[itemProduct];

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
                buildingArrays[itemProduct] = newArray;
            }
        }
    }
    public void ShowFilteredItems()
    {
        ShowItems(BuildingManager.ShowBuildingProducts, BuildingManager.ShowBuildingTypes, BuildingManager.ShowBuildingClass);
    }
    public void ShowItems(string itemProduct, string itemType, string itemClass)
    {
        bool showAllProducts = itemProduct == "ALL";
        bool showAllTypes = itemType == "ALL";
        bool showAllClasses = itemClass == "ALL";

        foreach (var kvp in buildingArrays)
        {
            string product = kvp.Key;
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                BuildingItemData itemData = item.GetComponent<BuildingItemData>();

                bool showItem =
                    (showAllProducts || itemProduct == itemData.itemProduct) &&
                    (showAllTypes || itemType == itemData.itemType) &&
                    (showAllClasses || itemClass == itemData.itemClass);

                item.SetActive(showItem);
            }
        }
    }
}
