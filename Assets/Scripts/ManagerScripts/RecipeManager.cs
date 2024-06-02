using RecipeManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public Dictionary<string, GameObject[]> itemRecipeArrays;
    public RecipeCreator recipeCreator;
    public EquipmentManager equipmentManager;

    public static string ShowRecipeProducts = "ALLBLUEPRINTS";
    public static string ShowRecipeTypes = "ALLTYPES";
    public static string ShowRecipeClass = "ALLCLASSES";

    public void PopulateInventoryArrays()
    {
        itemRecipeArrays = new Dictionary<string, GameObject[]>
        {
            { "BASIC", new GameObject[0] },
            { "PROCESSED", new GameObject[0] },
            { "ENHANCED", new GameObject[0] },
            { "ASSEMBLED", new GameObject[0] },
            { "BUILDINGS", new GameObject[0] }
        };
    }

    /// <summary>
    /// Iterates all player knows recipes and pulls out one with matching 'name'.
    /// </summary>
    /// <param name="recipeName"></param>
    /// <param name="recipe"></param>
    /// <returns></returns>
    public bool TryGetRecipe(string recipeName, out GameObject recipe)
    {
        recipe = null;

        foreach (var recipeType in itemRecipeArrays)
        {
            foreach (var gameObject in recipeType.Value)
            {
                if (gameObject.name == recipeName)
                {
                    recipe = gameObject;
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Iterates all player knows recipes and pulls out one with matching 'Guid'.
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="recipe"></param>
    /// <returns></returns>
    public bool TryGetRecipe(Guid guid, out GameObject recipe)
    {
        recipe = null;

        foreach (var recipeType in itemRecipeArrays)
        {
            foreach (var gameObject in recipeType.Value)
            {
                RecipeItemData recipeItemData = gameObject.GetComponent<RecipeItemData>();

                if (recipeItemData.guid == guid)
                {
                    recipe = gameObject;
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Iterates all player knows recipes and pulls out one with matching 'index'.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="recipe"></param>
    /// <returns></returns>
    public bool TryGetRecipe(int index, out GameObject recipe)
    {
        recipe = null;

        foreach (var recipeType in itemRecipeArrays)
        {
            foreach (var gameObject in recipeType.Value)
            {
                RecipeItemData recipeItemData = gameObject.GetComponent<RecipeItemData>();

                if (recipeItemData.index == index)
                {
                    recipe = gameObject;
                    return true;
                }
            }
        }

        return false;
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
        RecipeItemData recipeData1 = t1.GetComponent<RecipeItemData>();
        RecipeItemData recipeData2 = t2.GetComponent<RecipeItemData>();

        if (recipeData1 != null && recipeData2 != null)
        {
            return recipeData1.orderAdded.CompareTo(recipeData2.orderAdded);
        }
        return 0;
    }
    void OnEnable()
    {
        ShowItems(ShowRecipeProducts, ShowRecipeTypes, ShowRecipeClass);
    }

    public void AddToItemArray(string recipeProduct, GameObject item)
    {
        item.transform.SetParent(transform);
        // Check if the item type already exists in the itemRecipeArrays dictionary
        if (itemRecipeArrays.ContainsKey(recipeProduct))
        {
            // Update the existing item

            // Get the existing array of items for the item type
            GameObject[] itemArray = itemRecipeArrays[recipeProduct];

            // Create a new array with increased length to accommodate the new item
            GameObject[] newArray = new GameObject[itemArray.Length + 1];

            // Copy the existing items to the new array
            itemArray.CopyTo(newArray, 0);

            // Add the new item to the new array
            newArray[newArray.Length - 1] = item;

            // Update the itemArray reference in the dictionary
            itemRecipeArrays[recipeProduct] = newArray;
        }
        else
        {
            // Add the item to the itemRecipeArrays dictionary

            // Create a new array with a single item
            GameObject[] itemArray = new GameObject[] { item };

            // Add the item array to the dictionary with the item type as the key
            itemRecipeArrays.Add(recipeProduct, itemArray);
        }

        // Get or add the ItemData component to the item
        _ = item.GetComponent<RecipeItemData>() ?? item.AddComponent<RecipeItemData>();
    }

    public void RemoveFromItemArray(string recipeProduct, GameObject item)
    {
        // Check if the item type exists in the itemRecipeArrays dictionary
        if (itemRecipeArrays.ContainsKey(recipeProduct))
        {
            // Get the array of items for the item type
            GameObject[] itemArray = itemRecipeArrays[recipeProduct];

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
                itemRecipeArrays[recipeProduct] = newArray;
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
        ShowItems(ShowRecipeProducts, ShowRecipeTypes, ShowRecipeClass);
    }
    public void ShowItems(string recipeProduct, string recipeType, string itemClass)
    {
        bool showAllProducts = recipeProduct == "ALLBLUEPRINTS";
        bool showAllTypes = recipeType == "ALLTYPES";
        bool showAllClasses = itemClass == "ALLCLASSES";

        foreach (var kvp in itemRecipeArrays)
        {
            string product = kvp.Key;
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                RecipeItemData itemData = item.GetComponent<RecipeItemData>();

                bool showItem =
                    (showAllProducts || recipeProduct == itemData.recipeProduct) &&
                    (showAllTypes || recipeType == itemData.recipeType) &&
                    (showAllClasses || itemClass == itemData.itemClass);

                item.SetActive(showItem);
            }
        }
    }
}
