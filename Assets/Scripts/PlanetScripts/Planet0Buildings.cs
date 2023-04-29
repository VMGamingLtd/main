using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class Planet0Buildings
{
    public static int Planet0Index = 64;
    public static int HousingUnityPlanet0 = 0;
    public static int OxygenGeneratorPlanet0 = 0;
    public static int AtmosphericCondenserPlanet0 = 0;

    public static string AddBuildingCount(ref int buildingName, int amount)
    {
        buildingName += amount;
        return buildingName.ToString();
    }

    public static string ReduceBuildingCount(ref int buildingName, int amount)
    {
        buildingName -= amount;
        return buildingName.ToString();
    }

    public static string ResetBuildingCount(ref int buildingName)
    {
        buildingName = 0;
        return buildingName.ToString();
    }
}
