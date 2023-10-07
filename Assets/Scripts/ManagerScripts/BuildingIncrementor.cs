using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingIncrementor : MonoBehaviour
{
    public TextMeshProUGUI[] buildingCounts;
    public GameObject[] buildings;

    public void InitializeAvailableBuildings()
    {
        if (Planet0Buildings.PlantFieldUnlocked) buildings[1].SetActive(true);
        if (Planet0Buildings.WaterPumpUnlocked) buildings[2].SetActive(true);
        if (Planet0Buildings.BoilerUnlocked) buildings[3].SetActive(true);
        if (Planet0Buildings.SteamGeneratorUnlocked) buildings[4].SetActive(true);
        if (Planet0Buildings.FurnaceUnlocked) buildings[5].SetActive(true);
    }
    public void InitializeBuildingCounts()
    { 
        buildingCounts[0].text = Planet0Buildings.Planet0BiofuelGeneratorBlueprint.ToString();
        buildingCounts[1].text = Planet0Buildings.Planet0PlantFieldBlueprint.ToString();
        buildingCounts[2].text = Planet0Buildings.Planet0WaterPumpBlueprint.ToString();
        buildingCounts[3].text = Planet0Buildings.Planet0BoilerBlueprint.ToString();
        buildingCounts[4].text = Planet0Buildings.Planet0SteamGeneratorBlueprint.ToString();
        buildingCounts[5].text = Planet0Buildings.Planet0FurnaceBlueprint.ToString();
    } 
}
