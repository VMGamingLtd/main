#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class GameData
    {
        public int Id;

        public int? UserSlotId;
        public UserSlot? UserSlot;

        public string? Title;
        public string? Username;
        public string? Password;
        public string? Email;
        public string? ShowItemProducts;
        public string? ShowRecipeProducts;
        public string? ShowItemTypes;
        public string? ShowRecipeTypes;
        public string? ShowItemClass;
        public string? ShowRecipeClass;
        public string? Planet0Name;
        public int AtmospherePlanet0;
        public string? Planet0WindStatus;
        public int Planet0UV;
        public string? Planet0Weather;
        public int AgriLandPlanet0;
        public int ForestsPlanet0;
        public int WaterPlanet0;
        public int FisheriesPlanet0;
        public int MineralsPlanet0;
        public int RocksPlanet0;
        public int FossilFuelsPlanet0;
        public int RareElementsPlanet0;
        public int GemstonesPlanet0;
        public float? PlayerOxygen;
        public float? PlayerWater;
        public float? PlayerEnergy;
        public float? PlayerHunger;
        public int PlayerLevel;
        public int PlayerCurrentExp;
        public int PlayerMaxExp;
        public int SkillPoints;
        public int StatPoints;
        public int Hours;
        public int Minutes;
        public int Seconds;
        public bool RegisteredUser;
        public bool FirstGoal;
        public bool SecondGoal;
        public bool ThirdGoal;
        public bool IsPlayerInBiologicalBiome;
        public float Credits;
        public string? MenuButtonTypeOn;
        public bool isDraggingBuilding;
        public int Planet0CurrentElectricity;
        public int Planet0CurrentConsumption;
        public int Planet0MaxElectricity;
        public int Planet0BiofuelGenerator;
        public int Planet0WaterPump;
        public int Planet0PlantField;
        public int Planet0Boiler;
        public int Planet0SteamGenerator;
        public string? BuildingStatisticProcess;
        public string? BuildingStatisticType;
        public string? BuildingStatisticInterval;
        public bool BuildingIntervalTypeChanged;
        public bool BuildingStatisticTypeChanged;
        public int ItemCreationID;
        public string? slotEquipped;
    }
}
