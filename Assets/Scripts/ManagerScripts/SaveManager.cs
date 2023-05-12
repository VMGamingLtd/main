using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

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
        public int plants;
        public int water;
        public int biofuel;
        public int waterbottle;
        public int battery;
        public int expPoints;
        public bool registeredUser;
        public float credits;
        public string inventoryTitle;
        public InventoryItemData[] inventoryObjects;

    }

    [System.Serializable]
    public class InventoryItemData
    {
        public string itemName;
        public int itemQuantity;
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
        currentSaveData.plants = PlayerResources.Plants;
        currentSaveData.water = PlayerResources.Water;
        currentSaveData.biofuel = PlayerResources.Biofuel;
        currentSaveData.waterbottle = PlayerResources.WaterBottle;
        currentSaveData.battery = PlayerResources.Battery;
        currentSaveData.expPoints = EXPPoints.expPoints;
        currentSaveData.registeredUser = CoroutineManager.registeredUser;
        currentSaveData.credits = Credits.credits;

        currentSaveData.inventoryTitle = "Inventory";

        // Create an array to store the inventory objects
        currentSaveData.inventoryObjects = new InventoryItemData[inventoryManager.RawItems.Length];

        // Populate the inventory objects
        for (int i = 0; i < inventoryManager.RawItems.Length; i++)
        {
            GameObject itemGameObject = inventoryManager.RawItems[i];

            // Create a new InventoryItemData object
            InventoryItemData itemData = new InventoryItemData();

            // Set the specific attributes of the item
            itemData.itemName = itemGameObject.name;

            // Retrieve the child GameObject with the TextMeshProUGUI component representing the quantity
            GameObject quantityObject = itemGameObject.transform.Find("CountInventory").gameObject;

            // Get the TextMeshProUGUI component from the child GameObject
            TMPro.TextMeshProUGUI quantityText = quantityObject.GetComponent<TMPro.TextMeshProUGUI>();

            // Parse the quantity text value to an integer and assign it to itemData.itemQuantity
            int quantity;
            if (int.TryParse(quantityText.text, out quantity))
            {
                itemData.itemQuantity = quantity;
            }
            else
            {
                itemData.itemQuantity = 0;
            }

            // Add the item data to the array
            currentSaveData.inventoryObjects[i] = itemData;
        }

        // Serialize the data to JSON
        string jsonString = JsonConvert.SerializeObject(currentSaveData, Formatting.Indented);

        // Write the JSON string to a file in the persistentDataPath
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
            PlayerResources.Plants = loadedData.plants;
            CoroutineManager.registeredUser = loadedData.registeredUser;
        }
        else
        {
            Debug.LogError("JsonManager: Failed to load user data. File not found.");
        }

    }
}
