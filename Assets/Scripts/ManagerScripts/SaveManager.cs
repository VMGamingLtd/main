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

    private class SaveDataModel
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
        public bool isPlayerInBiologicalBiome;
        public float credits;
        public string inventoryTitle;
        public InventoryItemData[] basicInventoryObjects;
        public InventoryItemData[] processedInventoryObjects;
        public InventoryItemData[] refinedInventoryObjects;
        public InventoryItemData[] assembledInventoryObjects;
        public string recipeTitle;
        public RecipeData[] basicRecipeObjects;
        public RecipeData[] processedRecipeObjects;
        public RecipeData[] refinedRecipeObjects;
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
            string[] excludedTypes = { "BASIC", "PROCESSED", "REFINED" };
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
        currentSaveData.title = "Player info";
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
        currentSaveData.isPlayerInBiologicalBiome = GlobalCalculator.isPlayerInBiologicalBiome;
        currentSaveData.credits = Credits.credits;

        currentSaveData.inventoryTitle = "Inventory";

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
            itemData.OxygenTimer = itemDataComponent.WaterTimer;

            currentSaveData.processedInventoryObjects[i] = itemData;
        }
        currentSaveData.refinedInventoryObjects = new InventoryItemData[itemArrays["REFINED"].Length];

        for (int i = 0; i < itemArrays["REFINED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["REFINED"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            InventoryItemData itemData = new InventoryItemData();
            itemData.itemName = itemName;
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;
            itemData.itemQuantity = itemDataComponent.itemQuantity;

            currentSaveData.refinedInventoryObjects[i] = itemData;
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
            itemData.OxygenTimer = itemDataComponent.EnergyTimer;
            itemData.OxygenTimer = itemDataComponent.WaterTimer;

            currentSaveData.assembledInventoryObjects[i] = itemData;
        }
        currentSaveData.recipeTitle = "Recipes";

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
        currentSaveData.refinedRecipeObjects = new RecipeData[itemRecipeArrays["REFINED"].Length];

        for (int i = 0; i < itemRecipeArrays["REFINED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["REFINED"][i];
            string itemName = itemGameObject.name.Replace("(Clone)", "");

            RecipeData itemData = new RecipeData();
            itemData.itemName = itemName;
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemData.itemType = itemDataComponent.itemType;
            itemData.itemProduct = itemDataComponent.itemProduct;
            itemData.itemClass = itemDataComponent.itemClass;

            currentSaveData.refinedRecipeObjects[i] = itemData;
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
}
