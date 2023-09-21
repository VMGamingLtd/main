using ItemManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.ItemFactory
{
    public class ItemFactory : MonoBehaviour
    {
        public InventoryManager inventoryManager;
        public static int ItemCreationID = 0;
        private float remainingQuantity;
        public void CreateItem(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName)
        {
            bool itemFound = false;
            bool spliRemainingQuantity = true;
            if (inventoryManager.itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray) && itemArray.Length > 0)
            {
                foreach (GameObject item in itemArray)
                {
                    ItemData existingItemData = item.GetComponent<ItemData>();
                    if (item.name == prefabName + "(Clone)" && existingItemData.itemQuantity < existingItemData.stackLimit)
                    {
                        // The item already exists, increment the quantity for the rest of the quantity order
                        existingItemData.itemQuantity += quantity;
                        spliRemainingQuantity = false;
                        if (existingItemData.itemQuantity > existingItemData.stackLimit)
                        {
                            spliRemainingQuantity = true;
                            remainingQuantity = existingItemData.itemQuantity - existingItemData.stackLimit;
                            existingItemData.itemQuantity -= remainingQuantity;
                        }
                        UpdateItemCountText(item, existingItemData);
                        itemFound = true;
                        break;
                    }
                }
                if (spliRemainingQuantity)
                {
                    foreach (GameObject item in itemArray)
                    {
                        ItemData existingItemData = item.GetComponent<ItemData>();
                        if (item.name == prefabName + "(Clone)" && existingItemData.itemQuantity < existingItemData.stackLimit)
                        {
                            existingItemData.itemQuantity += remainingQuantity;
                            if (existingItemData.itemQuantity > existingItemData.stackLimit)
                            {
                                float newRemainingQuantity = existingItemData.itemQuantity - existingItemData.stackLimit;
                                SplitItem(newRemainingQuantity, prefab, itemProduct, itemType, itemClass, prefabName);
                                existingItemData.itemQuantity -= remainingQuantity;
                            }
                            UpdateItemCountText(item, existingItemData);
                            break;
                        }
                    }
                }
            }
            if (!itemFound)
            {
                // Create the item once and set the initial quantity
                GameObject newItem = Instantiate(prefab);
                newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);

                // Get or add the ItemData component to the new item
                ItemData newItemData = newItem.GetComponent<ItemData>();
                if (newItemData == null)
                {
                    newItemData = newItem.AddComponent<ItemData>();
                }

                newItemData.itemQuantity = quantity;
                newItemData.itemProduct = itemProduct;
                newItemData.itemType = itemType;
                newItemData.itemClass = itemClass;
                newItemData.ID = ItemCreationID++;
                newItemData.stackLimit = StackLimits.MediumStackLimit;

                // Add the new item to the itemArrays dictionary
                inventoryManager.AddToItemArray(itemProduct, newItem);

                // Update the CountInventory text
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();
                newItem.transform.localScale = Vector3.one;
            }
        }
        public void SplitItem(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName)
        {
            // Split the item, meaning that it will be duplicated
            GameObject newItem = Instantiate(prefab);
            newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);

            // Get or add the ItemData component to the new item
            ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();

            newItemData.itemQuantity = quantity;
            newItemData.itemProduct = itemProduct;
            newItemData.itemType = itemType;
            newItemData.itemClass = itemClass;
            newItemData.ID = ItemCreationID++;
            newItemData.stackLimit = StackLimits.MediumStackLimit;

            // Add the new item to the itemArrays dictionary
            inventoryManager.AddToItemArray(itemProduct, newItem);

            // Update the CountInventory text
            UpdateItemCountText(newItem, newItemData);
            newItem.AddComponent<ItemUse>();
        }

        private void UpdateItemCountText(GameObject item, ItemData itemData)
        {
            TextMeshProUGUI existingCountText = item.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
            if (existingCountText != null)
            {
                existingCountText.text = itemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
            }
        }
    }
}
