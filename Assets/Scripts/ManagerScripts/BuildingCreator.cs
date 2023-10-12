using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BuildingManagement;
using System;
using System.IO;

namespace BuildingManagement
{
    [Serializable]
    public class BuildingDataJson
    {
        public int index;
        public string buildingName;
        public string buildingType;
        public string buildingClass;
        public int consumedSlotCount;
        public List<SlotItemData> consumedItems;
        public int producedSlotCount;
        public List<SlotItemData> producedItems;
        public float totalTime;
        public int basePowerOutput;
        public int powerConsumption;
    }

    [System.Serializable]
    public class BuildingItemData : MonoBehaviour
    {
        public int index;
        public string buildingType;
        public string buildingName;
        public string buildingClass;
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
        public int index;
        public string itemName;
        public string itemQuality;
        public float quantity;
    }
}

public class BuildingCreator : MonoBehaviour
{
    public GameObject buildingTemplate;
    public GameObject newItemParent;
    public BuildingManager buildingManager;
    public TranslationManager translationManager;
    private BuildingItemData itemData;
    private List<BuildingDataJson> buildingDataList;

    [Serializable]
    private class JsonArray
    {
        public List<BuildingDataJson> buildings;
    }
    private void Awake()
    {
        /*
        string filePath = Path.Combine(Application.dataPath, "Scripts/Models/BuildingsList.json");

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            JsonArray jsonArray = JsonUtility.FromJson<JsonArray>(jsonText);
            if (jsonArray != null)
            {
                buildingDataList = jsonArray.buildings;
            }
        }
        else
        {
            Debug.LogError("BuildingsList.json not found at: " + filePath);
        }
        */

        string jsonText = Assets.Scripts.Models.BuildingsListJson.json;
        JsonArray jsonArray = JsonUtility.FromJson<JsonArray>(jsonText);
        if (jsonArray != null)
        {
            buildingDataList = jsonArray.buildings;
        }
    }
    public void CreateBuilding(GameObject draggedObject, int buildingIndex)
    {
        var itemData = buildingDataList[buildingIndex];

        CreateBuilding(draggedObject,
                    itemData.index,
                    itemData.buildingName,
                    itemData.buildingType,
                    itemData.buildingClass,
                    itemData.consumedSlotCount,
                    itemData.consumedItems.ToArray(),
                    itemData.producedSlotCount,
                    itemData.producedItems.ToArray(),
                    itemData.totalTime,
                    itemData.basePowerOutput,
                    itemData.powerConsumption);
    }

    private void CreateBuilding(GameObject draggedObject, 
                                int index, 
                                string buildingName, 
                                string buildingType,
                                string buildingClass,
                                int consumedSlotCount, 
                                SlotItemData[] slotItemData,
                                int producedSlotCount,
                                SlotItemData[] producedItemData,
                                float totalTime,
                                int powerOutput,
                                int powerConsumption)
    {
        itemData = draggedObject.AddComponent<BuildingItemData>();
        itemData.buildingType = buildingType;
        itemData.buildingPosition = draggedObject.GetComponent<RectTransform>().anchoredPosition3D;
        itemData.index = index;
        draggedObject.name = buildingName;
        itemData.buildingClass = buildingClass;
        itemData.consumedSlotCount = consumedSlotCount;
        itemData.producedSlotCount = producedSlotCount;
        itemData.consumedItems = new List<SlotItemData>(slotItemData);
        itemData.producedItems = new List<SlotItemData>(producedItemData);
        itemData.totalTime = totalTime;
        itemData.powerOutput = powerOutput;
        itemData.powerConsumption = powerConsumption;
        itemData.efficiencySetting = 100;
        itemData.isPaused = false;

        if (draggedObject.name == "BiofuelGenerator")
        {
            Planet0Buildings.Planet0BiofuelGenerator++;
            Planet0Buildings.Planet0BiofuelGeneratorBlueprint--;
            itemData.buildingCount = Planet0Buildings.Planet0BiofuelGenerator;
        }
        else if (draggedObject.name == "WaterPump")
        {
            Planet0Buildings.Planet0WaterPump++;
            Planet0Buildings.Planet0WaterPumpBlueprint--;
            itemData.buildingCount = Planet0Buildings.Planet0WaterPump;
        }
        else if (draggedObject.name == "PlantField")
        {
            Planet0Buildings.Planet0PlantField++;
            Planet0Buildings.Planet0PlantFieldBlueprint--;
            itemData.buildingCount = Planet0Buildings.Planet0PlantField;
        }
        else if (draggedObject.name == "Boiler")
        {
            Planet0Buildings.Planet0Boiler++;
            Planet0Buildings.Planet0BoilerBlueprint--;
            itemData.buildingCount = Planet0Buildings.Planet0Boiler;
        }
        else if (draggedObject.name == "SteamGenerator")
        {
            Planet0Buildings.Planet0SteamGenerator++;
            Planet0Buildings.Planet0SteamGeneratorBlueprint--;
            itemData.buildingCount = Planet0Buildings.Planet0SteamGenerator;
        }
        else if (draggedObject.name == "Furnace")
        {
            Planet0Buildings.Planet0Furnace++;
            Planet0Buildings.Planet0FurnaceBlueprint--;
            itemData.buildingCount = Planet0Buildings.Planet0Furnace;
        }

        translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
        string titleText = draggedObject.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(buildingName);
        itemData.buildingName = $"{titleText} #{itemData.buildingCount}";

        // Add the item to the building manager's item array
        buildingManager.AddToItemArray(buildingType, draggedObject);

        // Adding this class will start corotuine on the building object itself
        draggedObject.AddComponent<BuildingCycles>();

        if (GoalManager.thirdGoal == false)
        {
            GoalManager goalManager = GameObject.Find("GOALMANAGER").GetComponent<GoalManager>();
            _ = goalManager.SetFourthGoal();
        }
    }
}
