using System.Collections;
using System.Collections.Generic;
using RecipeManagement;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class RecipeManager : MonoBehaviour
{
    public Dictionary<string, GameObject[]> itemRecipeArrays;
    public RecipeCreator recipeCreator;
    public EquipmentManager equipmentManager;

    public static string ShowRecipeProducts = "ALL";
    public static string ShowRecipeTypes = "ALL";
    public static string ShowRecipeClass = "ALL";

    public void PopulateInventoryArrays()
    {
        itemRecipeArrays = new Dictionary<string, GameObject[]>
        {
            { "BASIC", new GameObject[0] },
            { "PROCESSED", new GameObject[0] },
            { "REFINED", new GameObject[0] },
            { "ASSEMBLED", new GameObject[0] }
        };
    }
    void OnEnable()
    {
        ShowItems(RecipeManager.ShowRecipeProducts, RecipeManager.ShowRecipeTypes, RecipeManager.ShowRecipeClass);
    }

    public void AddToItemArray(string itemProduct, GameObject item)
    {
        item.transform.SetParent(transform);
        // Check if the item type already exists in the itemRecipeArrays dictionary
        if (itemRecipeArrays.ContainsKey(itemProduct))
        {
            // Update the existing item

            // Get the existing array of items for the item type
            GameObject[] itemArray = itemRecipeArrays[itemProduct];

            // Create a new array with increased length to accommodate the new item
            GameObject[] newArray = new GameObject[itemArray.Length + 1];

            // Copy the existing items to the new array
            itemArray.CopyTo(newArray, 0);

            // Add the new item to the new array
            newArray[newArray.Length - 1] = item;

            // Update the itemArray reference in the dictionary
            itemRecipeArrays[itemProduct] = newArray;
        }
        else
        {
            // Add the item to the itemRecipeArrays dictionary

            // Create a new array with a single item
            GameObject[] itemArray = new GameObject[] { item };

            // Add the item array to the dictionary with the item type as the key
            itemRecipeArrays.Add(itemProduct, itemArray);
        }

        // Get or add the ItemData component to the item
        RecipeItemData itemData = item.GetComponent<RecipeItemData>();
        if (itemData == null)
        {
            itemData = item.AddComponent<RecipeItemData>();
        }
    }

    public void RemoveFromItemArray(string itemProduct, GameObject item)
    {
        // Check if the item type exists in the itemRecipeArrays dictionary
        if (itemRecipeArrays.ContainsKey(itemProduct))
        {
            // Get the array of items for the item type
            GameObject[] itemArray = itemRecipeArrays[itemProduct];

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
                itemRecipeArrays[itemProduct] = newArray;
            }
            else
            {
                Debug.LogWarning("Item not found in the item array for the specified item type.");
            }
        }
        else
        {
            Debug.LogWarning("Item type not found in the itemRecipeArrays dictionary.");
        }
    }
    public void ShowFilteredItems()
    {
        ShowItems(RecipeManager.ShowRecipeProducts, RecipeManager.ShowRecipeTypes, RecipeManager.ShowRecipeClass);
    }
    public void ShowItems(string itemProduct, string itemType, string itemClass)
    {
        bool showAllProducts = itemProduct == "ALL";
        bool showAllTypes = itemType == "ALL";
        bool showAllClasses = itemClass == "ALL";

        foreach (var kvp in itemRecipeArrays)
        {
            string product = kvp.Key;
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                RecipeItemData itemData = item.GetComponent<RecipeItemData>();

                bool showItem =
                    (showAllProducts || itemProduct == itemData.itemProduct) &&
                    (showAllTypes || itemType == itemData.itemType) &&
                    (showAllClasses || itemClass == itemData.itemClass);

                item.SetActive(showItem);
            }
        }
    }
}
