using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using ItemManagement;
using System.Linq;
using System;

public class SaveManager : MonoBehaviour
{

    private const string FileName = "Savegame.json";
    private string filePath;
    public InventoryManager inventoryManager;

    private class SaveDataModel
    {
        public string title;
        public string username;
        public string password;
        public string email;
        public string showItemTypes;
        public string planet0Name;
        public int atmospherePlanet0;
        public int agriLandPlanet0;
        public int forestsPlanet0;
        public int waterPlanet0;
        public int fisheriesPlanet0;
        public int mineralsPlanet0;
        public int rocksPlanet0;
        public int fossilFuelsPlanet0;
        public int rareElementsPlanet0;
        public int gemstonesPlanet0;
        public int playerOxygen;
        public int playerWater;
        public int playerEnergy;
        public int playerHunger;
        public int playerLevel;
        public int playerCurrentExp;
        public int playerMaxExp;
        public int skillPoints;
        public int statPoints;
        public int hours;
        public int minutes;
        public int seconds;
        public int expPoints;
        public bool registeredUser;
        public float credits;
        public string inventoryTitle;
        public InventoryItemData[] rawInventoryObjects;
        public InventoryItemData[] intermediateInventoryObjects;
        public InventoryItemData[] assembledInventoryObjects;
        public InventoryItemData[] utilityInventoryObjects;
    }

    [System.Serializable]
    public class InventoryItemData
    {
        public string itemName;
        public string itemType;
        public string itemQuality;
        public int itemQuantity;
        public int OxygenTimer;

        public bool ShouldSerializeOxygenTimer()
        {
            string[] excludedTypes = { "RAW", "INTERMEDIATE", "ASSEMBLED" };
            return !excludedTypes.Any(type => itemType.Equals(type, StringComparison.OrdinalIgnoreCase));
        }
    }
    // Current save data
    private SaveDataModel currentSaveData;

    private void Start()
    {
        // Set the file path to the persistent data path with the file name
        filePath = Path.Combine(Application.persistentDataPath, FileName);

        // Load the save data from the file
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            currentSaveData = JsonConvert.DeserializeObject<SaveDataModel>(jsonString);
        }
        else
        {
            currentSaveData = new SaveDataModel();
        }
    }


    public void SaveToJsonFile()
    {
        currentSaveData.title = "Player info";
        currentSaveData.username = UserName.userName;
        currentSaveData.password = Password.password;
        currentSaveData.email = Email.email;
        currentSaveData.showItemTypes = InventoryManager.ShowItemTypes;
        currentSaveData.planet0Name = Planet0Name.planet0Name;
        currentSaveData.atmospherePlanet0 = Planet0Buildings.AtmospherePlanet0;
        currentSaveData.agriLandPlanet0 = Planet0Buildings.AgriLandPlanet0;
        currentSaveData.forestsPlanet0 = Planet0Buildings.ForestsPlanet0;
        currentSaveData.waterPlanet0 = Planet0Buildings.WaterPlanet0;
        currentSaveData.fisheriesPlanet0 = Planet0Buildings.FisheriesPlanet0;
        currentSaveData.mineralsPlanet0 = Planet0Buildings.MineralsPlanet0;
        currentSaveData.rocksPlanet0 = Planet0Buildings.RocksPlanet0;
        currentSaveData.fossilFuelsPlanet0 = Planet0Buildings.FossilFuelsPlanet0;
        currentSaveData.rareElementsPlanet0 = Planet0Buildings.RareElementsPlanet0;
        currentSaveData.gemstonesPlanet0 = Planet0Buildings.GemstonesPlanet0;
        currentSaveData.playerLevel = Level.PlayerLevel;
        currentSaveData.playerCurrentExp = Level.PlayerCurrentExp;
        currentSaveData.playerMaxExp = Level.PlayerMaxExp;
        currentSaveData.skillPoints = Level.SkillPoints;
        currentSaveData.statPoints = Level.StatPoints;
        currentSaveData.hours = GlobalCalculator.hours;
        currentSaveData.minutes = GlobalCalculator.minutes;
        currentSaveData.seconds = GlobalCalculator.seconds;
        currentSaveData.playerOxygen = PlayerResources.PlayerOxygen;
        currentSaveData.playerWater = PlayerResources.PlayerWater;
        currentSaveData.playerEnergy = PlayerResources.PlayerEnergy;
        currentSaveData.playerHunger = PlayerResources.PlayerHunger;
        currentSaveData.expPoints = EXPPoints.expPoints;
        currentSaveData.registeredUser = CoroutineManager.registeredUser;
        currentSaveData.credits = Credits.credits;

        currentSaveData.inventoryTitle = "Inventory";

        // Access the itemArrays dictionary through the inventoryManager reference
        Dictionary<string, GameObject[]> itemArrays = inventoryManager.itemArrays;

        currentSaveData.rawInventoryObjects = new InventoryItemData[itemArrays["RAW"].Length];

        for (int i = 0; i < itemArrays["RAW"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["RAW"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            InventoryItemData itemData = new InventoryItemData();
            itemData.itemName = itemName;
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemQuantity = itemDataComponent.itemQuantity;
            itemData.itemQuality = itemDataComponent.itemQuality;

            currentSaveData.rawInventoryObjects[i] = itemData;
        }

        currentSaveData.intermediateInventoryObjects = new InventoryItemData[itemArrays["INTERMEDIATE"].Length];

        for (int i = 0; i < itemArrays["INTERMEDIATE"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["INTERMEDIATE"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            InventoryItemData itemData = new InventoryItemData();
            itemData.itemName = itemName;
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemQuantity = itemDataComponent.itemQuantity;
            itemData.itemQuality = itemDataComponent.itemQuality;

            currentSaveData.intermediateInventoryObjects[i] = itemData;
        }
        currentSaveData.assembledInventoryObjects = new InventoryItemData[itemArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemArrays["ASSEMBLED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["ASSEMBLED"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            InventoryItemData itemData = new InventoryItemData();
            itemData.itemName = itemName;
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemQuantity = itemDataComponent.itemQuantity;
            itemData.itemQuality = itemDataComponent.itemQuality;

            currentSaveData.assembledInventoryObjects[i] = itemData;
        }
        currentSaveData.utilityInventoryObjects = new InventoryItemData[itemArrays["UTILITY"].Length];

        for (int i = 0; i < itemArrays["UTILITY"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["UTILITY"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            InventoryItemData itemData = new InventoryItemData();
            itemData.itemName = itemName;
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemQuantity = itemDataComponent.itemQuantity;
            itemData.itemQuality = itemDataComponent.itemQuality;
            itemData.OxygenTimer = itemDataComponent.OxygenTimer;

            currentSaveData.utilityInventoryObjects[i] = itemData;
        }

        string jsonString = JsonConvert.SerializeObject(currentSaveData, Formatting.Indented);
        File.WriteAllText(filePath, jsonString);
    }

    public void LoadFromJsonFile()
    {
        // Check if the file exists at the specified path
        if (File.Exists(filePath))
        {
            // Read the JSON data from the file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON data back into the UserData object
            SaveDataModel loadedData = JsonConvert.DeserializeObject<SaveDataModel>(json);

            // Update the variable values with the loaded data
            UserName.userName = loadedData.username;
            CoroutineManager.registeredUser = loadedData.registeredUser;
        }
        else
        {
            Debug.LogError("JsonManager: Failed to load user data. File not found.");
        }

    }
}
