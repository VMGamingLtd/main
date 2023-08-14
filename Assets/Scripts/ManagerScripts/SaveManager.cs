using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using ItemManagement;
using RecipeManagement;
using System.Linq;
using System;

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
        public int planet0WaterPump;
        public int planet0PlantField;
        public int planet0Boiler;
        public int planet0SteamGenerator;
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


    public void SaveToJsonFile()
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
        currentSaveData.planet0WaterPump = Planet0Buildings.Planet0WaterPump;
        currentSaveData.planet0PlantField = Planet0Buildings.Planet0PlantField;
        currentSaveData.planet0Boiler = Planet0Buildings.Planet0Boiler;
        currentSaveData.planet0SteamGenerator = Planet0Buildings.Planet0SteamGenerator;
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
        currentSaveData.ItemCreationID = ItemCreator.ItemCreationID;

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
        int slotId = 1;

        // fill in game data

        Gaos.Routes.Model.GameDataJson.UserGameDataSaveRequest userGameDataSaveRequest= new Gaos.Routes.Model.GameDataJson.UserGameDataSaveRequest();

        userGameDataSaveRequest.UserId = Gaos.Context.Authentication.GetUserId();
        userGameDataSaveRequest.SlotId = slotId;
        userGameDataSaveRequest.GameData = new Gaos.Dbo.Model.GameData();
        userGameDataSaveRequest.GameData.Username = UserName.userName;
        userGameDataSaveRequest.GameData.Password = Password.password;
        userGameDataSaveRequest.GameData.Email = Email.email;
        userGameDataSaveRequest.GameData.ShowItemProducts = InventoryManager.ShowItemProducts;
        userGameDataSaveRequest.GameData.ShowRecipeProducts = RecipeManager.ShowRecipeProducts;
        userGameDataSaveRequest.GameData.ShowItemTypes = InventoryManager.ShowItemTypes;
        userGameDataSaveRequest.GameData.ShowRecipeTypes = RecipeManager.ShowRecipeTypes;
        userGameDataSaveRequest.GameData.ShowItemClass = InventoryManager.ShowItemClass;
        userGameDataSaveRequest.GameData.ShowRecipeClass = RecipeManager.ShowRecipeClass;
        userGameDataSaveRequest.GameData.Planet0Name = Planet0Name.planet0Name;
        userGameDataSaveRequest.GameData.AtmospherePlanet0 = Planet0Buildings.AtmospherePlanet0;
        userGameDataSaveRequest.GameData.Planet0WindStatus = WindManager.Planet0WindStatus; // string
        userGameDataSaveRequest.GameData.Planet0UV = WeatherManager.planet0UV; // int
        userGameDataSaveRequest.GameData.Planet0Weather = WeatherManager.planet0Weather; // string
        userGameDataSaveRequest.GameData.AgriLandPlanet0 = Planet0Buildings.AgriLandPlanet0;
        userGameDataSaveRequest.GameData.ForestsPlanet0 = Planet0Buildings.ForestsPlanet0;
        userGameDataSaveRequest.GameData.WaterPlanet0 = Planet0Buildings.WaterPlanet0;
        userGameDataSaveRequest.GameData.FisheriesPlanet0 = Planet0Buildings.FisheriesPlanet0;
        userGameDataSaveRequest.GameData.MineralsPlanet0 = Planet0Buildings.MineralsPlanet0;
        userGameDataSaveRequest.GameData.RocksPlanet0 = Planet0Buildings.RocksPlanet0;
        userGameDataSaveRequest.GameData.FossilFuelsPlanet0 = Planet0Buildings.FossilFuelsPlanet0;
        userGameDataSaveRequest.GameData.RareElementsPlanet0 = Planet0Buildings.RareElementsPlanet0;
        userGameDataSaveRequest.GameData.GemstonesPlanet0 = Planet0Buildings.GemstonesPlanet0;
        userGameDataSaveRequest.GameData.PlayerLevel = Level.PlayerLevel;
        userGameDataSaveRequest.GameData.PlayerCurrentExp = Level.PlayerCurrentExp;
        userGameDataSaveRequest.GameData.PlayerMaxExp = Level.PlayerMaxExp;
        userGameDataSaveRequest.GameData.SkillPoints = Level.SkillPoints;
        userGameDataSaveRequest.GameData.StatPoints = Level.StatPoints;
        userGameDataSaveRequest.GameData.Hours = GlobalCalculator.hours;
        userGameDataSaveRequest.GameData.Minutes = GlobalCalculator.minutes;
        userGameDataSaveRequest.GameData.Seconds = GlobalCalculator.seconds;
        userGameDataSaveRequest.GameData.PlayerOxygen = PlayerResources.PlayerOxygen;
        userGameDataSaveRequest.GameData.PlayerWater = PlayerResources.PlayerWater;
        userGameDataSaveRequest.GameData.PlayerEnergy = PlayerResources.PlayerEnergy;
        userGameDataSaveRequest.GameData.PlayerHunger = PlayerResources.PlayerHunger;
        userGameDataSaveRequest.GameData.RegisteredUser = CoroutineManager.registeredUser;
        userGameDataSaveRequest.GameData.FirstGoal = GoalManager.firstGoal;
        userGameDataSaveRequest.GameData.SecondGoal = GoalManager.secondGoal;
        userGameDataSaveRequest.GameData.ThirdGoal = GoalManager.thirdGoal;
        userGameDataSaveRequest.GameData.IsPlayerInBiologicalBiome = GlobalCalculator.isPlayerInBiologicalBiome;
        userGameDataSaveRequest.GameData.Credits = Credits.credits;
        userGameDataSaveRequest.GameData.MenuButtonTypeOn = ButtonManager.MenuButtonTypeOn;
        userGameDataSaveRequest.GameData.isDraggingBuilding = BuildingManager.isDraggingBuilding;
        userGameDataSaveRequest.GameData.Planet0CurrentElectricity = Planet0Buildings.Planet0CurrentElectricity;
        userGameDataSaveRequest.GameData.Planet0CurrentConsumption = Planet0Buildings.Planet0CurrentConsumption;
        userGameDataSaveRequest.GameData.Planet0MaxElectricity = Planet0Buildings.Planet0MaxElectricity;
        userGameDataSaveRequest.GameData.Planet0BiofuelGenerator = Planet0Buildings.Planet0BiofuelGenerator;
        userGameDataSaveRequest.GameData.Planet0WaterPump = Planet0Buildings.Planet0WaterPump;
        userGameDataSaveRequest.GameData.Planet0PlantField = Planet0Buildings.Planet0PlantField;
        userGameDataSaveRequest.GameData.Planet0Boiler = Planet0Buildings.Planet0Boiler;
        userGameDataSaveRequest.GameData.Planet0SteamGenerator = Planet0Buildings.Planet0SteamGenerator;
        userGameDataSaveRequest.GameData.BuildingStatisticProcess = BuildingStatisticsManager.BuildingStatisticProcess;
        userGameDataSaveRequest.GameData.BuildingStatisticType = BuildingStatisticsManager.BuildingStatisticType;
        userGameDataSaveRequest.GameData.BuildingStatisticInterval = BuildingStatisticsManager.BuildingStatisticInterval;
        userGameDataSaveRequest.GameData.BuildingIntervalTypeChanged = BuildingIntervalTypes.BuildingIntervalTypeChanged;
        userGameDataSaveRequest.GameData.BuildingStatisticTypeChanged = BuildingStatisticTypes.BuildingStatisticTypeChanged;
        userGameDataSaveRequest.GameData.ItemCreationID = ItemCreator.ItemCreationID;

        // store the equipment bool array
        userGameDataSaveRequest.GameData.slotEquipped = EquipmentManager.slotEquipped;

        // fill in inventories

        Dictionary<string, GameObject[]> itemArrays = inventoryManager.itemArrays;

        userGameDataSaveRequest.BasicInventoryObjects = new Gaos.Dbo.Model.InventoryItemData[itemArrays["BASIC"].Length];

        for (int i = 0; i < itemArrays["BASIC"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["BASIC"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            string itemName = itemGameObject.name.Replace("(Clone)", "");


            userGameDataSaveRequest.BasicInventoryObjects[i] = new Gaos.Dbo.Model.InventoryItemData();

            userGameDataSaveRequest.BasicInventoryObjects[i].ItemName = itemName;
            userGameDataSaveRequest.BasicInventoryObjects[i].ID = itemDataComponent.ID;
            userGameDataSaveRequest.BasicInventoryObjects[i].stackLimit = itemDataComponent.stackLimit;
            userGameDataSaveRequest.BasicInventoryObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.BasicInventoryObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.BasicInventoryObjects[i].ItemClass = itemDataComponent.itemClass;
            userGameDataSaveRequest.BasicInventoryObjects[i].ItemQuantity =itemDataComponent.itemQuantity;
        }

        userGameDataSaveRequest.ProcessedInventoryObjects = new Gaos.Dbo.Model.InventoryItemData[itemArrays["PROCESSED"].Length];

        for (int i = 0; i < itemArrays["PROCESSED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["PROCESSED"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            userGameDataSaveRequest.ProcessedInventoryObjects[i] = new Gaos.Dbo.Model.InventoryItemData();

            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemName = itemName;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ID = itemDataComponent.ID;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].stackLimit = itemDataComponent.stackLimit;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemClass = itemDataComponent.itemClass;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemQuantity =itemDataComponent.itemQuantity;
        }

        userGameDataSaveRequest.EnhancedInventoryObjects = new Gaos.Dbo.Model.InventoryItemData[itemArrays["ENHANCED"].Length];

        for (int i = 0; i < itemArrays["ENHANCED"].Length; i++)
        {

            GameObject itemGameObject = itemArrays["ENHANCED"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            userGameDataSaveRequest.EnhancedInventoryObjects[i] = new Gaos.Dbo.Model.InventoryItemData();

            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemName = itemName;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ID = itemDataComponent.ID;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].stackLimit = itemDataComponent.stackLimit;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemClass = itemDataComponent.itemClass;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemQuantity =itemDataComponent.itemQuantity;
        }

        userGameDataSaveRequest.AssembledInventoryObjects = new Gaos.Dbo.Model.InventoryItemData[itemArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemArrays["ASSEMBLED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["ASSEMBLED"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            userGameDataSaveRequest.AssembledInventoryObjects[i] = new Gaos.Dbo.Model.InventoryItemData();

            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemName = itemName;
            userGameDataSaveRequest.AssembledInventoryObjects[i].ID = itemDataComponent.ID;
            userGameDataSaveRequest.AssembledInventoryObjects[i].stackLimit = itemDataComponent.stackLimit;
            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemClass = itemDataComponent.itemClass;
            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemQuantity =itemDataComponent.itemQuantity;
        }
        // recipeTitle treba vyhodit!
        //userGameDataSaveRequest.GameData.RecipeTitle = saveDataModel.recipeTitle;


        // fill in recipes

        Dictionary<string, GameObject[]> itemRecipeArrays = recipeManager.itemRecipeArrays;

        userGameDataSaveRequest.BasicRecipeObjects = new Gaos.Dbo.Model.RecipeData[itemRecipeArrays["BASIC"].Length];

        for (int i = 0; i < itemRecipeArrays["BASIC"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["BASIC"][i];
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            userGameDataSaveRequest.BasicRecipeObjects[i] = new Gaos.Dbo.Model.RecipeData();

            userGameDataSaveRequest.BasicRecipeObjects[i].ItemName = itemName;
            userGameDataSaveRequest.BasicRecipeObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.BasicRecipeObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.BasicRecipeObjects[i].ItemClass = itemDataComponent.itemClass;
        }

        userGameDataSaveRequest.ProcessedRecipeObjects = new Gaos.Dbo.Model.RecipeData[itemRecipeArrays["PROCESSED"].Length];

        for (int i = 0; i < itemRecipeArrays["PROCESSED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["PROCESSED"][i];
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            userGameDataSaveRequest.ProcessedRecipeObjects[i] = new Gaos.Dbo.Model.RecipeData();

            userGameDataSaveRequest.ProcessedRecipeObjects[i].ItemName = itemName;
            userGameDataSaveRequest.ProcessedRecipeObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.ProcessedRecipeObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.ProcessedRecipeObjects[i].ItemClass = itemDataComponent.itemClass;
        }

        userGameDataSaveRequest.EnhancedRecipeObjects = new Gaos.Dbo.Model.RecipeData[itemRecipeArrays["ENHANCED"].Length];

        for (int i = 0; i < itemRecipeArrays["ENHANCED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["ENHANCED"][i];
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            userGameDataSaveRequest.EnhancedRecipeObjects[i] = new Gaos.Dbo.Model.RecipeData();

            userGameDataSaveRequest.EnhancedRecipeObjects[i].ItemName = itemName;
            userGameDataSaveRequest.EnhancedRecipeObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.EnhancedRecipeObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.EnhancedRecipeObjects[i].ItemClass = itemDataComponent.itemClass;
        }

        userGameDataSaveRequest.AssembledRecipeObjects = new Gaos.Dbo.Model.RecipeData[itemRecipeArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemRecipeArrays["ASSEMBLED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["ASSEMBLED"][i];
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            userGameDataSaveRequest.AssembledRecipeObjects[i] = new Gaos.Dbo.Model.RecipeData();

            userGameDataSaveRequest.AssembledRecipeObjects[i].ItemName = itemName;
            userGameDataSaveRequest.AssembledRecipeObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.AssembledRecipeObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.AssembledRecipeObjects[i].ItemClass = itemDataComponent.itemClass;
        }

        StartCoroutine(Gaos.GameData.UserGameDataSave.Save(slotId, userGameDataSaveRequest, OnUserGameDataSaveComplete));

    }

    public  void OnUserGameDataSaveComplete(Gaos.Routes.Model.GameDataJson.UserGameDataSaveResponse response)
    {
        string responseString = JsonConvert.SerializeObject(response);
        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 3000: OnUserGameDataSaveComplete(): {responseString}");


        StartCoroutine(Gaos.GameData.UserGameDataGet.Get(1,OnUserGameDataGetComplete));
    }
    public static void OnUserGameDataGetComplete(Gaos.Routes.Model.GameDataJson.UserGameDataGetResponse response)
    {
        string responseString = JsonConvert.SerializeObject(response);
        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 3100: OnUserGameDataGetComplete(): {responseString}");

        // DON'T FORGET TO SWITCH 'GlobalCalculator.GameStarted' bool to true when game is loaded!!!
    }
}
