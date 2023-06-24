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
        public string planet0WindStatus;
        public string playerOxygen;
        public string playerWater;
        public string playerEnergy;
        public string playerHunger;
        public int playerLevel;
        public int playerCurrentExp;
        public int playerMaxExp;
        public int skillPoints;
        public int statPoints;
        public int hours;
        public int minutes;
        public int seconds;
        public bool registeredUser;
        public bool firstGoal;
        public bool secondGoal;
        public bool thirdGoal;
        public bool isPlayerInBiologicalBiome;
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
    }

    [System.Serializable]
    public class InventoryItemData
    {
        public string itemName;
        public string itemType;
        public string itemClass;
        public string itemProduct;
        public int itemQuantity;
        public string OxygenTimer;
        public string EnergyTimer;
        public string WaterTimer;

        public bool ShouldSerializeOxygenTimer()
        {
            string[] excludedTypes = { "BASIC", "PROCESSED", "ENHANCED" };
            return !excludedTypes.Any(type => itemType.Equals(type, StringComparison.OrdinalIgnoreCase));
        }
    }
    public class RecipeData
    {
        public string itemName;
        public string itemType;
        public string itemClass;
        public string itemProduct;
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
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;
            itemData.itemQuantity = itemDataComponent.itemQuantity;
            itemData.WaterTimer = itemDataComponent.WaterTimer;

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
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;
            itemData.itemQuantity = itemDataComponent.itemQuantity;
            itemData.OxygenTimer = itemDataComponent.OxygenTimer;
            itemData.EnergyTimer = itemDataComponent.EnergyTimer;
            itemData.WaterTimer = itemDataComponent.WaterTimer;

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

    /*public void TestSaveGameDataOnServer()
    {
        Gaos.Routes.Model.GameDataJson.UserGameDataSaveRequest userGameDataSaveRequest= new Gaos.Routes.Model.GameDataJson.UserGameDataSaveRequest();

        userGameDataSaveRequest.UserId = Gaos.Context.Authentication.GetUserId();
        userGameDataSaveRequest.SlotId = 1;
        userGameDataSaveRequest.GameData = new Gaos.Dbo.Model.GameData();
        userGameDataSaveRequest.GameData.Username = UserName.userName;
        userGameDataSaveRequest.GameData.Password = Password.password;
        userGameDataSaveRequest.GameData.Email = Email.email;
        userGameDataSaveRequest.GameData.showItemProducts = InventoryManager.ShowItemProducts;
        userGameDataSaveRequest.GameData.showRecipeProducts = RecipeManager.ShowRecipeProducts;
        userGameDataSaveRequest.GameData.showItemTypes = InventoryManager.ShowItemTypes;
        userGameDataSaveRequest.GameData.showRecipeTypes = RecipeManager.ShowRecipeTypes;
        userGameDataSaveRequest.GameData.showItemClass = InventoryManager.ShowItemClass;
        userGameDataSaveRequest.GameData.showRecipeClass = RecipeManager.ShowRecipeClass;
        userGameDataSaveRequest.GameData.planet0Name = Planet0Name.planet0Name;
        userGameDataSaveRequest.GameData.atmospherePlanet0 = Planet0Buildings.AtmospherePlanet0;
        userGameDataSaveRequest.GameData.planet0WindStatus = WindManager.Planet0WindStatus;
        userGameDataSaveRequest.GameData.planet0UV = WeatherManager.planet0UV;
        userGameDataSaveRequest.GameData.planet0Weather = WeatherManager.planet0Weather;
        userGameDataSaveRequest.GameData.agriLandPlanet0 = Planet0Buildings.AgriLandPlanet0;
        userGameDataSaveRequest.GameData.forestsPlanet0 = Planet0Buildings.ForestsPlanet0;
        userGameDataSaveRequest.GameData.waterPlanet0 = Planet0Buildings.WaterPlanet0;
        userGameDataSaveRequest.GameData.fisheriesPlanet0 = Planet0Buildings.FisheriesPlanet0;
        userGameDataSaveRequest.GameData.mineralsPlanet0 = Planet0Buildings.MineralsPlanet0;
        userGameDataSaveRequest.GameData.rocksPlanet0 = Planet0Buildings.RocksPlanet0;
        userGameDataSaveRequest.GameData.fossilFuelsPlanet0 = Planet0Buildings.FossilFuelsPlanet0;
        userGameDataSaveRequest.GameData.rareElementsPlanet0 = Planet0Buildings.RareElementsPlanet0;
        userGameDataSaveRequest.GameData.gemstonesPlanet0 = Planet0Buildings.GemstonesPlanet0;
        userGameDataSaveRequest.GameData.playerLevel = Level.PlayerLevel;
        userGameDataSaveRequest.GameData.playerCurrentExp = Level.PlayerCurrentExp;
        userGameDataSaveRequest.GameData.playerMaxExp = Level.PlayerMaxExp;
        userGameDataSaveRequest.GameData.skillPoints = Level.SkillPoints;
        userGameDataSaveRequest.GameData.statPoints = Level.StatPoints;
        userGameDataSaveRequest.GameData.hours = GlobalCalculator.hours;
        userGameDataSaveRequest.GameData.minutes = GlobalCalculator.minutes;
        userGameDataSaveRequest.GameData.seconds = GlobalCalculator.seconds;
        userGameDataSaveRequest.GameData.playerOxygen = PlayerResources.PlayerOxygen;
        userGameDataSaveRequest.GameData.playerWater = PlayerResources.PlayerWater;
        userGameDataSaveRequest.GameData.playerEnergy = PlayerResources.PlayerEnergy;
        userGameDataSaveRequest.GameData.playerHunger = PlayerResources.PlayerHunger;
        userGameDataSaveRequest.GameData.registeredUser = CoroutineManager.registeredUser;
        userGameDataSaveRequest.GameData.firstGoal = GoalManager.firstGoal;
        userGameDataSaveRequest.GameData.firstGoal = GoalManager.secondGoal;
        userGameDataSaveRequest.GameData.firstGoal = GoalManager.thirdGoal;
        userGameDataSaveRequest.GameData.isPlayerInBiologicalBiome = GlobalCalculator.isPlayerInBiologicalBiome;
        userGameDataSaveRequest.GameData.credits = Credits.credits;

        // inventoryTitle treba vyhodit!
        //userGameDataSaveRequest.GameData.InventoryTitle = saveDataModel.inventoryTitle;

        userGameDataSaveRequest.BasicInventoryObjects = new Gaos.Dbo.Model.InventoryItemData[itemArrays["BASIC"].Length];

        for (int i = 0; i < itemArrays["BASIC"].Length; i++)
        {
            userGameDataSaveRequest.BasicInventoryObjects[i] = new Gaos.Dbo.Model.InventoryItemData();
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            userGameDataSaveRequest.BasicInventoryObjects[i].ItemName = itemDataComponent.itemName;
            userGameDataSaveRequest.BasicInventoryObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.BasicInventoryObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.BasicInventoryObjects[i].ItemClass = itemDataComponent.itemClass;
            userGameDataSaveRequest.BasicInventoryObjects[i].ItemQuantity =itemDataComponent.itemQuantity;
        }

        userGameDataSaveRequest.ProcessedInventoryObjects = new Gaos.Dbo.Model.InventoryItemData[itemArrays["PROCESSED"].Length];

        for (int i = 0; i < itemArrays["PROCESSED"].Length; i++)
        {
            userGameDataSaveRequest.ProcessedInventoryObjects[i] = new Gaos.Dbo.Model.InventoryItemData();
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemName = itemDataComponent.itemName;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemClass = itemDataComponent.itemClass;
            userGameDataSaveRequest.ProcessedInventoryObjects[i].ItemQuantity =itemDataComponent.itemQuantity;
            //userGameDataSaveRequest.BasicInventoryObjects[i].OxygenTimer = itemDataComponent.OxygenTimer;
        }
        // RefinedInventoryObjects sa menia na EnhancedInventoryObjects!
        userGameDataSaveRequest.EnhancedInventoryObjects = new Gaos.Dbo.Model.InventoryItemData[itemArrays["ENHANCED"].Length];

        for (int i = 0; i < itemArrays["ENHANCED"].Length; i++)
        {
            userGameDataSaveRequest.EnhancedInventoryObjects[i] = new Gaos.Dbo.Model.InventoryItemData();
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemName = itemDataComponent.itemName;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemClass = itemDataComponent.itemClass;
            userGameDataSaveRequest.EnhancedInventoryObjects[i].ItemQuantity =itemDataComponent.itemQuantity;
        }

        userGameDataSaveRequest.AssembledInventoryObjects = new Gaos.Dbo.Model.InventoryItemData[itemArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemArrays["ASSEMBLED"].Length; i++)
        {
            userGameDataSaveRequest.AssembledInventoryObjects[i] = new Gaos.Dbo.Model.InventoryItemData();
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemName = itemDataComponent.itemName;
            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemType = itemDataComponent.itemType;
            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemProduct = itemDataComponent.itemProduct;
            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemClass = itemDataComponent.itemClass;
            userGameDataSaveRequest.AssembledInventoryObjects[i].ItemQuantity =itemDataComponent.itemQuantity;
            //userGameDataSaveRequest.AssembledInventoryObjects[i].OxygenTimer = itemDataComponent.OxygenTimer;
            //userGameDataSaveRequest.AssembledInventoryObjects[i].WaterTimer = itemDataComponent.WaterTimer;
            //userGameDataSaveRequest.AssembledInventoryObjects[i].EnergyTimer = itemDataComponent.EnergyTimer;
        }
        // recipeTitle treba vyhodit!
        //userGameDataSaveRequest.GameData.RecipeTitle = saveDataModel.recipeTitle;

        userGameDataSaveRequest.BasicRecipeObjects = new Gaos.Dbo.Model.RecipeData[itemArrays["BASIC"].Length];

        for (int i = 0; i < itemRecipeArrays["BASIC"].Length; i++)
        {
            userGameDataSaveRequest.BasicRecipeObjects[i] = new Gaos.Dbo.Model.RecipeData();
            RecipeData itemData = itemGameObject.GetComponent<RecipeData>();
            userGameDataSaveRequest.BasicRecipeObjects[i].ItemName = itemData.itemName;
            userGameDataSaveRequest.BasicRecipeObjects[i].ItemType = itemData.itemType;
            userGameDataSaveRequest.BasicRecipeObjects[i].ItemProduct = itemData.itemProduct;
            userGameDataSaveRequest.BasicRecipeObjects[i].ItemClass = itemData.itemClass;
        }

        userGameDataSaveRequest.ProcessedRecipeObjects = new Gaos.Dbo.Model.RecipeData[itemArrays["PROCESSED"].Length];

        for (int i = 0; i < itemRecipeArrays["PROCESSED"].Length; i++)
        {
            userGameDataSaveRequest.ProcessedRecipeObjects[i] = new Gaos.Dbo.Model.RecipeData();
            RecipeData itemData = itemGameObject.GetComponent<RecipeData>();
            userGameDataSaveRequest.ProcessedRecipeObjects[i].ItemName = itemData.itemName;
            userGameDataSaveRequest.ProcessedRecipeObjects[i].ItemType = itemData.itemType;
            userGameDataSaveRequest.ProcessedRecipeObjects[i].ItemProduct = itemData.itemProduct;
            userGameDataSaveRequest.ProcessedRecipeObjects[i].ItemClass = itemData.itemClass;
        }

        // RefinedRecipeObjects sa menia na EnhancedRecipeObjects!
        //userGameDataSaveRequest.RefinedRecipeObjects = new Gaos.Dbo.Model.RecipeData[1];
        userGameDataSaveRequest.EnahncedRecipeObjects = new Gaos.Dbo.Model.RecipeData[itemArrays["ENHANCED"].Length];

        for (int i = 0; i < itemRecipeArrays["ENHANCED"].Length; i++)
        {
            userGameDataSaveRequest.EnahncedRecipeObjects[i] = new Gaos.Dbo.Model.RecipeData();
            RecipeData itemData = itemGameObject.GetComponent<RecipeData>();
            userGameDataSaveRequest.EnahncedRecipeObjects[i].ItemName = itemData.itemName;
            userGameDataSaveRequest.EnahncedRecipeObjects[i].ItemType = itemData.itemType;
            userGameDataSaveRequest.EnahncedRecipeObjects[i].ItemProduct = itemData.itemProduct;
            userGameDataSaveRequest.EnahncedRecipeObjects[i].ItemClass = itemData.itemClass;
        }

        userGameDataSaveRequest.EnahncedRecipeObjects = new Gaos.Dbo.Model.RecipeData[itemArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemRecipeArrays["ASSEMBLED"].Length; i++)
        {
            userGameDataSaveRequest.AssembledRecipeObjects[i] = new Gaos.Dbo.Model.RecipeData();
            RecipeData itemData = itemGameObject.GetComponent<RecipeData>();
            userGameDataSaveRequest.AssembledRecipeObjects[i].ItemName = itemData.itemName;
            userGameDataSaveRequest.AssembledRecipeObjects[i].ItemType = itemData.itemType;
            userGameDataSaveRequest.AssembledRecipeObjects[i].ItemProduct = itemData.itemProduct;
            userGameDataSaveRequest.AssembledRecipeObjects[i].ItemClass = itemData.itemClass;
        }

        mb.StartCoroutine(Gaos.GameData.UserGameDataSave.Save(1,userGameDataSaveRequest, OnUserGameDataSaveComplete, mb));

    }*/

    public static void OnUserGameDataSaveComplete(Gaos.Routes.Model.GameDataJson.UserGameDataSaveResponse response, object mb)
    {
        string responseString = JsonConvert.SerializeObject(response);
        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 3000: OnUserGameDataSaveComplete(): {responseString}");


        ((MonoBehaviour)mb).StartCoroutine(Gaos.GameData.UserGameDataGet.Get(1,OnUserGameDataGetComplete, mb));
    }
    public static void OnUserGameDataGetComplete(Gaos.Routes.Model.GameDataJson.UserGameDataGetResponse response, object obj)
    {
        string responseString = JsonConvert.SerializeObject(response);
        Debug.Log($"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 3100: OnUserGameDataGetComplete(): {responseString}");

    }
}
