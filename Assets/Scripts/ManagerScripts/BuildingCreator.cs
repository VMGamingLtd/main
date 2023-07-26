using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BuildingManagement
{
    [System.Serializable]
    public class BuildingItemData : MonoBehaviour
    {
        public string itemType;
        public Vector3 buildingPosition;
        public List<SlotItemData> consumedItems;
        public List<SlotItemData> producedItems;
        public int consumedSlotCount;
        public float timer;
        public float totalTime;
        public int powerOutput;
        public int basePowerOutput;
        public int powerConsumption;
        public int producedSlotCount;
        public int efficiency;
        public int efficiencySetting;
        public bool isPaused;

        public BuildingItemData()
        {
            consumedItems = new List<SlotItemData>();
            producedItems = new List<SlotItemData>();
        }
    }

    [System.Serializable]
    public class SlotItemData
    {
        public string itemName;
        public string itemQuality;
        public int quantity;
        public int slotCount;
    }

    public class BuildingCreator : MonoBehaviour
    {
        public GameObject[] buildingPrefabs;
        public GameObject newItemParent;
        public BuildingManager buildingManager;
        private BuildingItemData itemData;

        public void CreateBuilding(GameObject draggedObject, string itemType)
        {
            itemData = draggedObject.AddComponent<BuildingItemData>();
            itemData.itemType = itemType;
            itemData.buildingPosition = draggedObject.GetComponent<RectTransform>().anchoredPosition3D;

            if (draggedObject.name == "BiofuelGenerator(Clone)")
            {
                SlotItemData consumedItem1 = new SlotItemData
                {
                    itemName = "Biofuel",
                    itemQuality = "PROCESSED",
                    quantity = 1,
                    slotCount = 1
                };
                itemData.consumedItems.Add(consumedItem1);

                itemData.efficiencySetting = 100;
                itemData.totalTime = 10;
                itemData.basePowerOutput = 10000;
                itemData.powerOutput = itemData.basePowerOutput;
                itemData.powerConsumption = 0;
                itemData.consumedSlotCount = 1;
                itemData.isPaused = false;
            }
            else if (draggedObject.name == "WaterPump(Clone)")
            {
                SlotItemData producedItem1 = new SlotItemData
                {
                    itemName = "Water",
                    itemQuality = "BASIC",
                    quantity = 4,
                    slotCount = 1
                };
                itemData.producedItems.Add(producedItem1);

                itemData.efficiencySetting = 100;
                itemData.totalTime = 10;
                itemData.powerConsumption = 2000;
                itemData.producedSlotCount = 1;
                itemData.isPaused = false;
            }

            // Add the item to the building manager's item array
            buildingManager.AddToItemArray(itemType, draggedObject);

            // Adding this class will start corotuine on the building object itself
            draggedObject.AddComponent<BuildingCycles>();
        }
    }
}
