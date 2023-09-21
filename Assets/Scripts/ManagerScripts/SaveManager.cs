using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using ItemManagement;
using RecipeManagement;
using System.Linq;
using System;
using Assets.Scripts.ItemFactory;

public class SaveManager : MonoBehaviour
{

    private const string FileName = "Savegame.json";
    private string filePath;
    public InventoryManager inventoryManager;
    public RecipeManager recipeManager;
    private SaveDataModel currentSaveData;

    public class SaveDataModel
    {
        public string title;
        public string username;
        public string password;
        public string email;
        public string showItemProducts;
        public string showRecipeProducts;
        public string showItemTypes;
        public string showRecipeTypes;
        public string showItemClass;
        public string showRecipeClass;
        public string planet0Name;
        public string planet0Weather;
        public int planet0UV;
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
        public int planet0BiofuelGenerator;
        public int planet0BiofuelGeneratorBlueprint;
        public int planet0WaterPump;
        public int planet0WaterPumpBlueprint;
        public int planet0PlantField;
        public int planet0PlantFieldBlueprint;
        public int planet0Boiler;
        public int planet0BoilerBlueprint;
        public int planet0SteamGenerator;
        public int planet0SteamGeneratorBlueprint;
        public int planet0Furnace;
        public int planet0FurnaceBlueprint;
        public string planet0WindStatus;
        public float playerOxygen;
        public float playerWater;
        public float playerEnergy;
        public float playerHunger;
        public string MenuButtonTypeOn;
        public int playerLevel;
        public int playerCurrentExp;
        public int playerMaxExp;
        public int skillPoints;
        public int statPoints;
        public int hours;
        public int minutes;
        public int seconds;
        public int Planet0CurrentElectricity;
        public int Planet0CurrentConsumption;
        public int Planet0MaxElectricity;
        public bool registeredUser;
        public bool firstGoal;
        public bool secondGoal;
        public bool thirdGoal;
        public bool isPlayerInBiologicalBiome;
        public bool isDraggingBuilding;
        public bool autoConsumption;
        public float credits;
        public string inventoryTitle;
        public InventoryItemData[] basicInventoryObjects;
        public InventoryItemData[] processedInventoryObjects;
        public InventoryItemData[] enhancedInventoryObjects;
        public InventoryItemData[] assembledInventoryObjects;
        public string recipeTitle;
        public RecipeData[] basicRecipeObjects;
        public RecipeData[] processedRecipeObjects;
        public RecipeData[] enhancedRecipeObjects;
        public RecipeData[] assembledRecipeObjects;
        public string BuildingStatisticProcess;
        public string BuildingStatisticType;
        public string BuildingStatisticInterval;
        public bool BuildingStatisticTypeChanged;
        public bool BuildingIntervalTypeChanged;
        public bool[] slotEquipped;
        public int ItemCreationID;
    }

    public class InventoryItemData
    {
        public int ID;
        public float stackLimit;
        public string itemName;
        public string itemType;
        public string itemClass;
        public string itemProduct;
        public float itemQuantity;
    }
    public class RecipeData
    {
        public string itemName;
        public string itemType;
        public string itemClass;
        public string itemProduct;
    }

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

    public string SerializeGameData()
    {
        currentSaveData.username = UserName.userName;
        currentSaveData.password = Password.password;
        currentSaveData.email = Email.email;
        currentSaveData.showItemProducts = InventoryManager.ShowItemProducts;
        currentSaveData.showRecipeProducts = RecipeManager.ShowRecipeProducts;
        currentSaveData.showItemTypes = InventoryManager.ShowItemTypes;
        currentSaveData.showRecipeTypes = RecipeManager.ShowRecipeTypes;
        currentSaveData.showItemClass = InventoryManager.ShowItemClass;
        currentSaveData.showRecipeClass = RecipeManager.ShowRecipeClass;
        currentSaveData.planet0Name = Planet0Name.planet0Name;
        currentSaveData.atmospherePlanet0 = Planet0Buildings.AtmospherePlanet0;
        currentSaveData.planet0WindStatus = WindManager.Planet0WindStatus;
        currentSaveData.planet0UV = WeatherManager.planet0UV;
        currentSaveData.planet0Weather = WeatherManager.planet0Weather;
        currentSaveData.agriLandPlanet0 = Planet0Buildings.AgriLandPlanet0;
        currentSaveData.forestsPlanet0 = Planet0Buildings.ForestsPlanet0;
        currentSaveData.waterPlanet0 = Planet0Buildings.WaterPlanet0;
        currentSaveData.fisheriesPlanet0 = Planet0Buildings.FisheriesPlanet0;
        currentSaveData.mineralsPlanet0 = Planet0Buildings.MineralsPlanet0;
        currentSaveData.rocksPlanet0 = Planet0Buildings.RocksPlanet0;
        currentSaveData.fossilFuelsPlanet0 = Planet0Buildings.FossilFuelsPlanet0;
        currentSaveData.rareElementsPlanet0 = Planet0Buildings.RareElementsPlanet0;
        currentSaveData.gemstonesPlanet0 = Planet0Buildings.GemstonesPlanet0;
        currentSaveData.planet0BiofuelGenerator = Planet0Buildings.Planet0BiofuelGenerator;
        currentSaveData.planet0BiofuelGeneratorBlueprint = Planet0Buildings.Planet0BiofuelGeneratorBlueprint;
        currentSaveData.planet0WaterPump = Planet0Buildings.Planet0WaterPump;
        currentSaveData.planet0WaterPumpBlueprint = Planet0Buildings.Planet0WaterPumpBlueprint;
        currentSaveData.planet0PlantField = Planet0Buildings.Planet0PlantField;
        currentSaveData.planet0PlantFieldBlueprint = Planet0Buildings.Planet0PlantFieldBlueprint;
        currentSaveData.planet0Boiler = Planet0Buildings.Planet0Boiler;
        currentSaveData.planet0BoilerBlueprint = Planet0Buildings.Planet0BoilerBlueprint;
        currentSaveData.planet0SteamGenerator = Planet0Buildings.Planet0SteamGenerator;
        currentSaveData.planet0SteamGeneratorBlueprint = Planet0Buildings.Planet0SteamGeneratorBlueprint;
        currentSaveData.planet0Furnace = Planet0Buildings.Planet0Furnace;
        currentSaveData.planet0FurnaceBlueprint = Planet0Buildings.Planet0FurnaceBlueprint;
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
        currentSaveData.registeredUser = CoroutineManager.registeredUser;
        currentSaveData.firstGoal = GoalManager.firstGoal;
        currentSaveData.firstGoal = GoalManager.secondGoal;
        currentSaveData.firstGoal = GoalManager.thirdGoal;
        currentSaveData.isPlayerInBiologicalBiome = GlobalCalculator.isPlayerInBiologicalBiome;
        currentSaveData.credits = Credits.credits;
        currentSaveData.MenuButtonTypeOn = ButtonManager.MenuButtonTypeOn;
        currentSaveData.isDraggingBuilding = BuildingManager.isDraggingBuilding;
        currentSaveData.Planet0MaxElectricity = Planet0Buildings.Planet0CurrentElectricity;
        currentSaveData.Planet0CurrentConsumption = Planet0Buildings.Planet0CurrentConsumption;
        currentSaveData.Planet0MaxElectricity = Planet0Buildings.Planet0MaxElectricity;
        currentSaveData.BuildingStatisticProcess = BuildingStatisticsManager.BuildingStatisticProcess;
        currentSaveData.BuildingStatisticType = BuildingStatisticsManager.BuildingStatisticType;
        currentSaveData.BuildingStatisticInterval = BuildingStatisticsManager.BuildingStatisticInterval;
        currentSaveData.BuildingIntervalTypeChanged = BuildingIntervalTypes.BuildingIntervalTypeChanged;
        currentSaveData.BuildingStatisticTypeChanged = BuildingStatisticTypes.BuildingStatisticTypeChanged;
        currentSaveData.ItemCreationID = ItemFactory.ItemCreationID;
        currentSaveData.autoConsumption = EquipmentManager.autoConsumption;

        // slot equip array
        currentSaveData.slotEquipped = EquipmentManager.slotEquipped;

        // Access the itemArrays dictionary through the inventoryManager reference
        Dictionary<string, GameObject[]> itemArrays = inventoryManager.itemArrays;

        currentSaveData.basicInventoryObjects = new InventoryItemData[itemArrays["BASIC"].Length];

        for (int i = 0; i < itemArrays["BASIC"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["BASIC"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            InventoryItemData itemData = new InventoryItemData();
            itemData.itemName = itemName;
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemData.ID = itemDataComponent.ID;
            itemData.stackLimit = itemDataComponent.stackLimit;
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;
            itemData.itemQuantity = itemDataComponent.itemQuantity;

            currentSaveData.basicInventoryObjects[i] = itemData;
        }

        currentSaveData.processedInventoryObjects = new InventoryItemData[itemArrays["PROCESSED"].Length];

        for (int i = 0; i < itemArrays["PROCESSED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["PROCESSED"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            InventoryItemData itemData = new InventoryItemData();
            itemData.itemName = itemName;
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemData.ID = itemDataComponent.ID;
            itemData.stackLimit = itemDataComponent.stackLimit;
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;
            itemData.itemQuantity = itemDataComponent.itemQuantity;

            currentSaveData.processedInventoryObjects[i] = itemData;
        }
        currentSaveData.enhancedInventoryObjects = new InventoryItemData[itemArrays["ENHANCED"].Length];

        for (int i = 0; i < itemArrays["ENHANCED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["ENHANCED"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            InventoryItemData itemData = new InventoryItemData();
            itemData.itemName = itemName;
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemData.ID = itemDataComponent.ID;
            itemData.stackLimit = itemDataComponent.stackLimit;
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;
            itemData.itemQuantity = itemDataComponent.itemQuantity;

            currentSaveData.enhancedInventoryObjects[i] = itemData;
        }
        currentSaveData.assembledInventoryObjects = new InventoryItemData[itemArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemArrays["ASSEMBLED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["ASSEMBLED"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            InventoryItemData itemData = new InventoryItemData();
            itemData.itemName = itemName;
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemData.ID = itemDataComponent.ID;
            itemData.stackLimit = itemDataComponent.stackLimit;
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;
            itemData.itemQuantity = itemDataComponent.itemQuantity;

            currentSaveData.assembledInventoryObjects[i] = itemData;
        }

        // Access the itemArrays dictionary through the inventoryManager reference
        Dictionary<string, GameObject[]> itemRecipeArrays = recipeManager.itemRecipeArrays;

        currentSaveData.basicRecipeObjects = new RecipeData[itemRecipeArrays["BASIC"].Length];

        for (int i = 0; i < itemRecipeArrays["BASIC"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["BASIC"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            RecipeData itemData = new RecipeData();
            itemData.itemName = itemName;
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;

            currentSaveData.basicRecipeObjects[i] = itemData;
        }

        currentSaveData.processedRecipeObjects = new RecipeData[itemRecipeArrays["PROCESSED"].Length];

        for (int i = 0; i < itemRecipeArrays["PROCESSED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["PROCESSED"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            RecipeData itemData = new RecipeData();
            itemData.itemName = itemName;
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;

            currentSaveData.processedRecipeObjects[i] = itemData;
        }
        currentSaveData.enhancedRecipeObjects = new RecipeData[itemRecipeArrays["ENHANCED"].Length];

        for (int i = 0; i < itemRecipeArrays["ENHANCED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["ENHANCED"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            RecipeData itemData = new RecipeData();
            itemData.itemName = itemName;
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;

            currentSaveData.enhancedRecipeObjects[i] = itemData;
        }
        currentSaveData.assembledRecipeObjects = new RecipeData[itemRecipeArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemRecipeArrays["ASSEMBLED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["ASSEMBLED"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            RecipeData itemData = new RecipeData();
            itemData.itemName = itemName;
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;

            currentSaveData.assembledRecipeObjects[i] = itemData;
        }

        string jsonString = JsonConvert.SerializeObject(currentSaveData, Formatting.Indented);
        return jsonString;

    }


    public void SaveToJsonFile()
    {
        string jsonString = SerializeGameData();
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

    public void TestSaveGameDataOnServer()
    {
        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 3000: TestSaveGameDataOnServer()");
        int slotId = 1;

        // fill in game data

        Gaos.Routes.Model.GameDataJson.UserGameDataSaveRequest userGameDataSaveRequest= new Gaos.Routes.Model.GameDataJson.UserGameDataSaveRequest();

        userGameDataSaveRequest.UserId = Gaos.Context.Authentication.GetUserId();
        userGameDataSaveRequest.SlotId = slotId;

        userGameDataSaveRequest.GameDataJson = SerializeGameData();

        StartCoroutine(Gaos.GameData.UserGameDataSave.Save(slotId, userGameDataSaveRequest, OnUserGameDataSaveComplete));

    }

    public  void OnUserGameDataSaveComplete(Gaos.Routes.Model.GameDataJson.UserGameDataSaveResponse response)
    {
        string responseString = JsonConvert.SerializeObject(response);
        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 3100: OnUserGameDataSaveComplete(): {responseString}");


        StartCoroutine(Gaos.GameData.UserGameDataGet.Get(1,OnUserGameDataGetComplete));
    }
    public static void OnUserGameDataGetComplete(Gaos.Routes.Model.GameDataJson.UserGameDataGetResponse response)
    {
        SaveDataModel gameData = JsonConvert.DeserializeObject<SaveDataModel>(response.GameDataJson);
        string gameDataStr = JsonConvert.SerializeObject(gameData);
        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 3300: saveDataModel: {gameDataStr}");



        // DON'T FORGET TO SWITCH 'GlobalCalculator.GameStarted' bool to true when game is loaded!!!
        // Also BuildingIncrementor has to be re-initialized!
    }
}
