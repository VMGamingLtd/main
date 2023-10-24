using BuildingManagement;
using Cysharp.Threading.Tasks; // Import UniTask namespace
using TMPro;
using UnityEngine;

public class ElectricityUpdater : MonoBehaviour
{
    public TextMeshProUGUI[] textFields; // An array of TextMeshProUGUI for multiple text fields
    private NumberFormater formatter;
    public BuildingManager buildingManager;

    private async UniTaskVoid Start()
    {
        formatter = new NumberFormater();
        await RefreshCurrentElectricityLoop(); // Start the electricity refresh loop
    }

    private async UniTask RefreshCurrentElectricityLoop()
    {
        await UniTask.Delay(1000); // Use UniTask.Delay to wait for 1 second (1000 milliseconds)
        UpdateCurrentElectricityText();
    }

    private async void UpdateCurrentElectricityText()
    {
        if (GlobalCalculator.GameStarted)
        {
            int totalElectricity = CalculateTotalElectricity();
            int totalConsumption = CalculateTotalConsumption();
            totalElectricity -= totalConsumption;
            Planet0Buildings.Planet0CurrentElectricity = totalElectricity;
            Planet0Buildings.Planet0CurrentConsumption = totalConsumption;
            string formattedPower = formatter.FormatEnergyInThousands(totalElectricity);
            string formattedConsumption = formatter.FormatEnergyInThousands(totalConsumption);

            // Update all TextMeshProUGUI elements in the textFields array
            foreach (TextMeshProUGUI textField in textFields)
            {
                textField.text = formattedPower;
                if (formattedPower.Contains("-"))
                {
                    textField.color = Color.red;
                }
                else
                {
                    textField.color = Color.white;
                }
            }
        }

        await RefreshCurrentElectricityLoop();

    }

    private int CalculateTotalElectricity()
    {
        int totalElectricity = 0;
        // Check if "POWERPLANT" exists in buildingArrays
        if (buildingManager.buildingArrays.TryGetValue("POWERPLANT", out GameObject[] powerPlants))
        {
            // Iterate through each POWERPLANT building and get the BuildingItemData component
            foreach (GameObject powerPlant in powerPlants)
            {
                EnergyBuildingItemData itemData = powerPlant.GetComponent<EnergyBuildingItemData>();

                if (itemData.efficiency > 0)
                {
                    // Add the basePowerOutput of each POWERPLANT to the totalElectricity
                    totalElectricity += itemData.powerOutput;
                }
            }
        }
        return totalElectricity;
    }
    private int CalculateTotalConsumption()
    {
        int totalConsumption = 0;
        // Iterate through all buildingArrays except "POWERPLANT"
        foreach (var buildingType in buildingManager.buildingArrays.Keys)
        {
            if (buildingType != "POWERPLANT")
            {
                if (buildingManager.buildingArrays.TryGetValue(buildingType, out GameObject[] buildings))
                {
                    // Iterate through each building of the current type and get the BuildingItemData component
                    foreach (GameObject building in buildings)
                    {
                        BuildingItemData itemData = building.GetComponent<BuildingItemData>();

                        if (!itemData.isPaused)
                        {
                            // Add the powerConsumption of each building to the totalConsumption
                            totalConsumption += itemData.powerConsumption;
                        }
                    }
                }
            }
        }
        return totalConsumption;
    }
}
