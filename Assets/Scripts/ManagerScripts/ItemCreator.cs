using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

namespace ItemManagement
{
    public class ItemData : MonoBehaviour
        {
            public int ID;
            public float stackLimit;
            public float itemQuantity;
            public string itemProduct;
            public string itemType;
            public string itemClass;
        }
    public class ItemCreator : MonoBehaviour
    {
        public GameObject[] itemPrefabs;
        public InventoryManager inventoryManager;
        public static int ItemCreationID = 0;
        public void CreatePlants(float quantity)
        {
            CreateItem(quantity, itemPrefabs[0], "BASIC", "PLANTS", "CLASS-F", itemPrefabs[0].name);
        }
        public void CreateWater(float quantity)
        {
            CreateItem(quantity, itemPrefabs[1], "BASIC", "LIQUID", "CLASS-F", itemPrefabs[1].name);
        }
        public void CreateBiofuel(float quantity)
        {
            CreateItem(quantity, itemPrefabs[2], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[2].name);
        }
        public void CreateDistilledWater(float quantity)
        {
            CreateItem(quantity, itemPrefabs[3], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[3].name);
        }
        public void CreateBattery(float quantity)
        {
            CreateItem(quantity, itemPrefabs[4], "ASSEMBLED", "ENERGY", "CLASS-F", itemPrefabs[4].name);
        }
        public void CreateOxygenTanks(float quantity)
        {
            CreateItem(quantity, itemPrefabs[5], "ASSEMBLED", "OXYGEN", "CLASS-F", itemPrefabs[5].name);
        }
        public void CreateBatteryCore(float quantity)
        {
            CreateItem(quantity, itemPrefabs[6], "ENHANCED", "ENERGY", "CLASS-F", itemPrefabs[6].name);
        }
        public void CreateSteam(float quantity)
        {
            CreateItem(quantity, itemPrefabs[7], "BASIC", "GAS", "CLASS-F", itemPrefabs[7].name);
        }

        public void SplitPlants(float quantity)
        {
            SplitItem(quantity, itemPrefabs[0], "BASIC", "PLANTS", "CLASS-F", itemPrefabs[0].name);
        }
        public void SplitWater(float quantity)
        {
            SplitItem(quantity, itemPrefabs[1], "BASIC", "LIQUID", "CLASS-F", itemPrefabs[1].name);
        }
        public void SplitBiofuel(float quantity)
        {
            SplitItem(quantity, itemPrefabs[2], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[2].name);
        }
        public void SplitDistilledWater(float quantity)
        {
            SplitItem(quantity, itemPrefabs[3], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[3].name);
        }
        public void SplitBattery(float quantity)
        {
            SplitItem(quantity, itemPrefabs[4], "ASSEMBLED", "ENERGY", "CLASS-F", itemPrefabs[4].name);
        }
        public void SplitOxygenTanks(float quantity)
        {
            SplitItem(quantity, itemPrefabs[5], "ASSEMBLED", "OXYGEN", "CLASS-F", itemPrefabs[5].name);
        }
        public void SplitBatteryCore(float quantity)
        {
            SplitItem(quantity, itemPrefabs[6], "ENHANCED", "ENERGY", "CLASS-F", itemPrefabs[6].name);
        }
        public void SplitSteam(float quantity)
        {
            SplitItem(quantity, itemPrefabs[7], "BASIC", "GAS", "CLASS-F", itemPrefabs[7].name);
        }

        private void SplitItem(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName)
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
                newItemData.stackLimit = 10f;

                // Add the new item to the itemArrays dictionary
                inventoryManager.AddToItemArray(itemProduct, newItem);

                // Update the CountInventory text
                TextMeshProUGUI newCountText = newItem.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                if (newCountText != null)
                {
                    newCountText.text = newItemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
                }
                newItem.AddComponent<ItemUse>();
        }

        private void CreateItem(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName)
        {
            bool itemFound = false;
            if (inventoryManager.itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray) && itemArray.Length > 0)
            {
                foreach (GameObject item in itemArray)
                {
                    if (item.name == prefabName + "(Clone)")
                    {
                        // The item already exists, increment the quantity for the rest of the quantity order
                        ItemData existingItemData = item.GetComponent<ItemData>();
                        existingItemData.itemQuantity += quantity;

                        if (existingItemData.itemQuantity > existingItemData.stackLimit)
                        {
                            float remainingQuantity = existingItemData.itemQuantity - existingItemData.stackLimit;
                            SplitItem(remainingQuantity, prefab, itemProduct, itemType, itemClass, prefabName);
                            existingItemData.itemQuantity -= remainingQuantity;
                        }

                        // Update the CountInventory text
                        TextMeshProUGUI existingCountText = item.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                        if (existingCountText != null)
                        {
                            existingCountText.text = existingItemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
                        }
                        itemFound = true;
                        break;
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
                newItemData.stackLimit = 10f;

                // Add the new item to the itemArrays dictionary
                inventoryManager.AddToItemArray(itemProduct, newItem);

                // Update the CountInventory text
                TextMeshProUGUI newCountText = newItem.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                if (newCountText != null)
                {
                    newCountText.text = newItemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
                }
                newItem.AddComponent<ItemUse>();
            }
        }
    }
}
