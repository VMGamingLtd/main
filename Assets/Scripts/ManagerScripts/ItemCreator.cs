using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ItemManagement
{
    public class ItemData : MonoBehaviour
        {
            public int itemQuantity;
            public string itemProduct;
            public string itemType;
            public string itemClass;
            public string OxygenTimer;
            public string EnergyTimer;
            public string WaterTimer;
        }
    public class ItemCreator : MonoBehaviour
    {
        public GameObject[] itemPrefabs;
        public InventoryManager inventoryManager;
        public void CreatePlants(int quantity)
        {
            CreateItem(quantity, itemPrefabs[0], "BASIC", "PLANTS", "CLASS-F", itemPrefabs[0].name);
        }
        public void CreateWater(int quantity)
        {
            CreateItem(quantity, itemPrefabs[1], "BASIC", "LIQUID", "CLASS-F", itemPrefabs[1].name);
        }
        public void CreateBiofuel(int quantity)
        {
            CreateItem(quantity, itemPrefabs[2], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[2].name);
        }
        public void CreatePurifiedWater(int quantity)
        {
            CreateItem(quantity, itemPrefabs[3], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[3].name);
        }
        public void CreateBattery(int quantity)
        {
            CreateItem(quantity, itemPrefabs[4], "ASSEMBLED", "ENERGY", "CLASS-F", itemPrefabs[4].name);
        }
        public void CreateOxygenTanks(int quantity)
        {
            CreateItem(quantity, itemPrefabs[5], "ASSEMBLED", "OXYGEN", "CLASS-F", itemPrefabs[5].name);
        }
        public void CreateBatteryCore(int quantity)
        {
            CreateItem(quantity, itemPrefabs[6], "ENHANCED", "ENERGY", "CLASS-F", itemPrefabs[6].name);
        }

        private void CreateItem(int quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName)
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

                        // Update the CountInventory text
                        TextMeshProUGUI existingCountText = item.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                        if (existingCountText != null)
                        {
                            existingCountText.text = existingItemData.itemQuantity.ToString();
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

                if (prefabName == "OxygenTank")
                {
                    newItemData.OxygenTimer = "00:00:10:00";
                }
                else if (prefabName == "PurifiedWater")
                {
                    newItemData.WaterTimer = "00:00:10:00";
                }
                else if (prefabName == "Battery")
                {
                    newItemData.EnergyTimer = "00:00:05:00";
                }

                // Add the new item to the itemArrays dictionary
                inventoryManager.AddToItemArray(itemProduct, newItem);

                // Update the CountInventory text
                TextMeshProUGUI newCountText = newItem.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                if (newCountText != null)
                {
                    newCountText.text = newItemData.itemQuantity.ToString();
                }
            }
        }
    }
}
