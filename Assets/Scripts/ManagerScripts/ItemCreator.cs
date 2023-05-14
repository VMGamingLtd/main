using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ItemManagement
{
    public class ItemData : MonoBehaviour
        {
            public int itemQuantity;
            public string itemType;
            public string itemQuality;
            public int OxygenTimer;
        }
    public class ItemCreator : MonoBehaviour
    {
        public GameObject[] itemPrefabs;
        public InventoryManager inventoryManager;
        public void CreatePlants(int quantity)
        {
            CreateItem(quantity, itemPrefabs[0], "RAW", "RESOURCE", itemPrefabs[0].name);
        }

        public void CreateWater(int quantity)
        {
            CreateItem(quantity, itemPrefabs[1], "RAW", "RESOURCE", itemPrefabs[1].name);
        }

        public void CreateBiofuel(int quantity)
        {
            CreateItem(quantity, itemPrefabs[2], "INTERMEDIATE", "CONSUMABLE", itemPrefabs[2].name);
        }
        public void CreateWaterBottle(int quantity)
        {
            CreateItem(quantity, itemPrefabs[3], "INTERMEDIATE", "CONSUMABLE", itemPrefabs[3].name);
        }

        private void CreateItem(int quantity, GameObject prefab, string itemType, string itemQuality, string prefabName)
        {
            bool itemFound = false;
            if (inventoryManager.itemArrays.TryGetValue(itemType, out GameObject[] itemArray) && itemArray.Length > 0)
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
                newItemData.itemType = itemType;
                newItemData.itemQuality = itemQuality;

                // Add the new item to the itemArrays dictionary
                inventoryManager.AddToItemArray(itemType, newItem);

                // Update the CountInventory text
                TextMeshProUGUI newCountText = newItem.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                if (newCountText != null)
                {
                    newCountText.text = newItemData.itemQuantity.ToString();
                }
            }
        }
        public void CreateBattery(int quantity)
        {
            GameObject thisPrefab = itemPrefabs[4];
            string prefabName = thisPrefab.name;

            if (inventoryManager.itemArrays.TryGetValue(prefabName, out GameObject[] itemArray))
            {
                ItemData itemData = itemArray[0].GetComponent<ItemData>();
                itemData.itemQuantity += quantity;

                TextMeshProUGUI countText = itemArray[0].transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                if (countText != null)
                {
                    countText.text = itemData.itemQuantity.ToString();
                }
            }
            else
            {
                GameObject newPlant = Instantiate(thisPrefab);
                newPlant.transform.position = new Vector3(newPlant.transform.position.x, newPlant.transform.position.y, 0f);
                string itemType = "ASSEMBLED";

                ItemData itemData = newPlant.GetComponent<ItemData>();
                if (itemData == null)
                {
                    itemData = newPlant.AddComponent<ItemData>();
                }

                if (quantity > 1)
                {
                    itemData.itemQuantity = quantity;
                }
                else
                {
                    itemData.itemQuantity = 1;
                }

                itemData.itemType = itemType;
                itemData.itemQuality = "COMMON";

                inventoryManager.AddToItemArray(itemType, newPlant);

                TextMeshProUGUI countText = newPlant.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                if (countText != null)
                {
                    countText.text = itemData.itemQuantity.ToString();
                }
            }
        }

        public void CreateOxygenTanks(int quantity)
        {
            GameObject thisPrefab = itemPrefabs[5];
            string prefabName = thisPrefab.name;

            if (inventoryManager.itemArrays.TryGetValue(prefabName, out GameObject[] itemArray))
            {
                ItemData itemData = itemArray[0].GetComponent<ItemData>();
                itemData.itemQuantity += quantity;

                TextMeshProUGUI countText = itemArray[0].transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                if (countText != null)
                {
                    countText.text = itemData.itemQuantity.ToString();
                }
            }
            else
            {
                GameObject newPlant = Instantiate(thisPrefab);
                newPlant.transform.position = new Vector3(newPlant.transform.position.x, newPlant.transform.position.y, 0f);
                string itemType = "UTILITY";

                ItemData itemData = newPlant.GetComponent<ItemData>();
                if (itemData == null)
                {
                    itemData = newPlant.AddComponent<ItemData>();
                }

                if (quantity > 1)
                {
                    itemData.itemQuantity = quantity;
                }
                else
                {
                    itemData.itemQuantity = 1;
                }

                itemData.itemType = itemType;
                itemData.itemQuality = "COMMON";
                itemData.OxygenTimer = 1800;

                inventoryManager.AddToItemArray(itemType, newPlant);

                TextMeshProUGUI countText = newPlant.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                if (countText != null)
                {
                    countText.text = itemData.itemQuantity.ToString();
                }
            }
        }
    }
}
