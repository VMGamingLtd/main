using System;

[Serializable]
public static class Planet0Buildings
{
    public static float Planet0Index = 28.18f;
    public static int AtmospherePlanet0 = 90;
    public static int AgriLandPlanet0 = 23;
    public static int ForestsPlanet0 = 41;
    public static int WaterPlanet0 = 51;
    public static int FisheriesPlanet0 = 28;
    public static int MineralsPlanet0 = 18;
    public static int RocksPlanet0 = 35;
    public static int FossilFuelsPlanet0 = 13;
    public static int RareElementsPlanet0 = 6;
    public static int GemstonesPlanet0 = 11;
    public static int HousingUnityPlanet0 = 0;
    public static int OxygenGeneratorPlanet0 = 0;
    public static int AtmosphericCondenserPlanet0 = 0;
    public static int Planet0CurrentElectricity = 0;
    public static int Planet0CurrentConsumption = 0;
    public static int Planet0MaxElectricity = 0;

    // available buildings unlocks
    public static bool PlantFieldUnlocked;
    public static bool WaterPumpUnlocked;
    public static bool BoilerUnlocked;
    public static bool SteamGeneratorUnlocked;
    public static bool FurnaceUnlocked;

    // available buildings to place
    public static int Planet0BiofuelGeneratorBlueprint = 0;
    public static int Planet0PlantFieldBlueprint = 0;
    public static int Planet0WaterPumpBlueprint = 0;
    public static int Planet0BoilerBlueprint = 0;
    public static int Planet0SteamGeneratorBlueprint = 0;
    public static int Planet0FurnaceBlueprint = 0;


    // building counter
    public static int Planet0BiofuelGenerator = 0;
    public static int Planet0PlantField = 0;
    public static int Planet0WaterPump = 0;
    public static int Planet0Boiler = 0;
    public static int Planet0SteamGenerator = 0;
    public static int Planet0Furnace = 0;

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

    public static float CalculatePlanet0Index()
    {
        int condition1 = AtmospherePlanet0;
        int condition2 = AgriLandPlanet0;
        int condition3 = ForestsPlanet0;
        int condition4 = WaterPlanet0;
        int condition5 = FisheriesPlanet0;
        int condition6 = MineralsPlanet0;
        int condition7 = RocksPlanet0;
        int condition8 = FossilFuelsPlanet0;
        int condition9 = RareElementsPlanet0;
        int condition10 = GemstonesPlanet0;

        float result = (float)Math.Round((condition1 + condition2 + condition3 + condition4 + condition5 + condition6 + condition7 + condition8 + condition9 + condition10) / 11.0f, 1);
        Planet0Index = result;
        return result;
    }
}
