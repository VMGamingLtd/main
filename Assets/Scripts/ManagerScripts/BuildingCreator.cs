using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BuildingManagement;

namespace BuildingManagement
{
    [System.Serializable]
    public class BuildingItemData : MonoBehaviour
    {
        public string itemType;
        public string buildingName;
        public Vector3 buildingPosition;
        public List<SlotItemData> consumedItems;
        public List<SlotItemData> producedItems;
        public PowerCycle powerCycleData = new PowerCycle();
        public PowerConsumptionCycle powerConsumptionCycleData = new PowerConsumptionCycle();
        public ProductionCycle productionCycleData = new ProductionCycle();
        public int consumedSlotCount;
        public float timer;
        public float totalTime;
        public int secondCycleCount;
        public int minuteCycleCount;
        public int hourCycleCount;
        public int powerOutput;
        public int basePowerOutput;
        public int actualPowerOutput;
        public int powerConsumption;
        public int actualPowerConsumption;
        public int producedSlotCount;
        public int efficiency;
        public int efficiencySetting;
        public int buildingCount;
        public bool isPaused;

        public BuildingItemData()
        {
            consumedItems = new List<SlotItemData>();
            producedItems = new List<SlotItemData>();
        }
    }
    [System.Serializable]
    public class PowerCycle
    {
        public float[] secondCycle = new float[120];
        public float[] tenSecondCycle = new float[120];
        public float[] thirtySecondCycle = new float[120];
        public float[] minuteCycle = new float[120];
        public float[] tenMinuteCycle = new float[120];
        public float[] thirtyMinuteCycle = new float[120];
        public float[] hourCycle = new float[120];
        public float[] sixHourCycle = new float[120];
    }
    public class PowerConsumptionCycle
    {
        public float[] secondCycle = new float[120];
        public float[] tenSecondCycle = new float[120];
        public float[] thirtySecondCycle = new float[120];
        public float[] minuteCycle = new float[120];
        public float[] tenMinuteCycle = new float[120];
        public float[] thirtyMinuteCycle = new float[120];
        public float[] hourCycle = new float[120];
        public float[] sixHourCycle = new float[120];
    }
    public class ProductionCycle
    {
        public float[] secondCycle = new float[120];
        public float[] tenSecondCycle = new float[120];
        public float[] thirtySecondCycle = new float[120];
        public float[] minuteCycle = new float[120];
        public float[] tenMinuteCycle = new float[120];
        public float[] thirtyMinuteCycle = new float[120];
        public float[] hourCycle = new float[120];
        public float[] sixHourCycle = new float[120];
    }


    [System.Serializable]
    public class SlotItemData
    {
        public string itemName;
        public string itemQuality;
        public float quantity;
    }
}

public class BuildingCreator : MonoBehaviour
{
    public GameObject[] buildingPrefabs;
    public GameObject newItemParent;
    public BuildingManager buildingManager;
    private BuildingItemData itemData;
    private Transform titleTransform;
    private TextMeshProUGUI title;

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
                quantity = 1f
            };
            itemData.consumedItems.Add(consumedItem1);

            itemData.efficiencySetting = 100;
            itemData.totalTime = 10;
            itemData.basePowerOutput = 10000;
            itemData.powerOutput = itemData.basePowerOutput;
            itemData.powerConsumption = 0;
            itemData.consumedSlotCount = 1;
            itemData.isPaused = false;
            Planet0Buildings.Planet0BiofuelGenerator++;
            itemData.buildingCount = Planet0Buildings.Planet0BiofuelGenerator;
        }
        else if (draggedObject.name == "WaterPump(Clone)")
        {
            SlotItemData producedItem1 = new SlotItemData
            {
                itemName = "Water",
                itemQuality = "BASIC",
                quantity = 4f
            };
            itemData.producedItems.Add(producedItem1);

            itemData.efficiencySetting = 100;
            itemData.totalTime = 10;
            itemData.powerConsumption = 2000;
            itemData.producedSlotCount = 1;
            itemData.isPaused = false;
            Planet0Buildings.Planet0WaterPump++;
            itemData.buildingCount = Planet0Buildings.Planet0WaterPump;
        }
        else if (draggedObject.name == "PlantField(Clone)")
        {
            SlotItemData consumedItem1 = new SlotItemData
            {
                itemName = "Water",
                itemQuality = "BASIC",
                quantity = 1f
            };
            itemData.consumedItems.Add(consumedItem1);

            SlotItemData producedItem1 = new SlotItemData
            {
                itemName = "FibrousLeaves",
                itemQuality = "BASIC",
                quantity = 4f
            };
            itemData.producedItems.Add(producedItem1);

            itemData.efficiencySetting = 100;
            itemData.totalTime = 20;
            itemData.powerConsumption = 0;
            itemData.consumedSlotCount = 1;
            itemData.producedSlotCount = 1;
            itemData.isPaused = false;
            Planet0Buildings.Planet0PlantField++;
            itemData.buildingCount = Planet0Buildings.Planet0PlantField;
        }
        else if (draggedObject.name == "Boiler(Clone)")
        {
            SlotItemData consumedItem1 = new SlotItemData
            {
                itemName = "Water",
                itemQuality = "BASIC",
                quantity = 1f
            };
            itemData.consumedItems.Add(consumedItem1);

            SlotItemData producedItem1 = new SlotItemData
            {
                itemName = "Steam",
                itemQuality = "BASIC",
                quantity = 2f
            };
            itemData.producedItems.Add(producedItem1);

            itemData.efficiencySetting = 100;
            itemData.totalTime = 20;
            itemData.powerConsumption = 2000;
            itemData.consumedSlotCount = 1;
            itemData.producedSlotCount = 1;
            itemData.isPaused = false;
            Planet0Buildings.Planet0Boiler++;
            itemData.buildingCount = Planet0Buildings.Planet0Boiler;
        }
        else if (draggedObject.name == "SteamGenerator(Clone)")
        {
            SlotItemData consumedItem1 = new SlotItemData
            {
                itemName = "Steam",
                itemQuality = "BASIC",
                quantity = 1f
            };
            itemData.consumedItems.Add(consumedItem1);

            itemData.efficiencySetting = 100;
            itemData.totalTime = 10;
            itemData.basePowerOutput = 30000;
            itemData.powerOutput = itemData.basePowerOutput;
            itemData.powerConsumption = 0;
            itemData.consumedSlotCount = 1;
            itemData.isPaused = false;
            Planet0Buildings.Planet0SteamGenerator++;
            itemData.buildingCount = Planet0Buildings.Planet0SteamGenerator;
        }
        titleTransform = draggedObject.transform.Find("Title");
        title = titleTransform.GetComponent<TextMeshProUGUI>();
        itemData.buildingName = $"{title.text} #{itemData.buildingCount}";

        // Add the item to the building manager's item array
        buildingManager.AddToItemArray(itemType, draggedObject);

        // Adding this class will start corotuine on the building object itself
        draggedObject.AddComponent<BuildingCycles>();
    }
}
