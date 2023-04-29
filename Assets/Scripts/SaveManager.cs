using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{

    private const string FileName = "PlayerPrefsData.json";
    private string filePath;

    private class SaveDataModel
    {
        public string username;
        public string password;
        public string email;
        public string showItemTypes;
        public string planet0Name;
        public int hours;
        public int minutes;
        public int seconds;
        public int plants;
        public int water;
        public int biofuel;
        public int waterbottle;
        public int battery;
        public int achievementPoints;
        public bool registeredUser;
        public float credits;

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
        currentSaveData.showItemTypes = InventoryManager.ShowItemTypes;
        currentSaveData.planet0Name = Planet0Name.planet0Name;
        currentSaveData.hours = GlobalCalculator.hours;
        currentSaveData.minutes = GlobalCalculator.minutes;
        currentSaveData.seconds = GlobalCalculator.seconds;
        currentSaveData.plants = PlayerResources.Plants;
        currentSaveData.water = PlayerResources.Water;
        currentSaveData.biofuel = PlayerResources.Biofuel;
        currentSaveData.waterbottle = PlayerResources.WaterBottle;
        currentSaveData.battery = PlayerResources.Battery;
        currentSaveData.achievementPoints = AchievementPoints.achievementPoints;
        currentSaveData.registeredUser = CoroutineManager.registeredUser;
        currentSaveData.credits = Credits.credits;



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
