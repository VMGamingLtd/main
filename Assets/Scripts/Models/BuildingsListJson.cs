namespace Assets.Scripts.Models
{
    public class BuildingsListJson
    {
        public static string json = @"
{
  ""header"": ""Available buildings"",
  ""buildings"": [
    {
      ""index"": 0,
      ""buildingName"": ""BiofuelGenerator"",
      ""buildingType"": ""POWERPLANT"",
      ""buildingClass"": ""CLASS-F"",
      ""consumedSlotCount"": 1,
      ""consumedItems"": [
        {
          ""index"": 2,
          ""itemName"": ""Biofuel"",
          ""itemQuality"": ""ENHANCED"",
          ""quantity"": 1.0
        }
      ],
      ""producedSlotCount"": 0,
      ""producedItems"": [],
      ""totalTime"": 300.0,
      ""basePowerOutput"": 10000,
      ""powerConsumption"": 0,
      ""researchPoints"": 0
    },
    {
      ""index"": 1,
      ""buildingName"": ""WaterPump"",
      ""buildingType"": ""PUMPINGFACILITY"",
      ""buildingClass"": ""CLASS-F"",
      ""consumedSlotCount"": 0,
      ""consumedItems"": [],
      ""producedSlotCount"": 1,
      ""producedItems"": [
        {
          ""index"": 1,
          ""itemName"": ""Water"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 10.0
        }
      ],
      ""totalTime"": 12.0,
      ""basePowerOutput"": 0,
      ""powerConsumption"": 2000,
      ""researchPoints"": 0
    },
    {
      ""index"": 2,
      ""buildingName"": ""FibrousPlantField"",
      ""buildingType"": ""AGRICULTURE"",
      ""buildingClass"": ""CLASS-F"",
      ""consumedSlotCount"": 1,
      ""consumedItems"": [
        {
          ""index"": 1,
          ""itemName"": ""Water"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 5.0
        }
      ],
      ""producedSlotCount"": 1,
      ""producedItems"": [
        {
          ""index"": 0,
          ""itemName"": ""FibrousLeaves"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 10.0
        }
      ],
      ""totalTime"": 30.0,
      ""basePowerOutput"": 0,
      ""powerConsumption"": 2000,
      ""researchPoints"": 0
    },
    {
      ""index"": 3,
      ""buildingName"": ""Boiler"",
      ""buildingType"": ""HEATINGFACILITY"",
      ""buildingClass"": ""CLASS-F"",
      ""consumedSlotCount"": 1,
      ""consumedItems"": [
        {
          ""index"": 1,
          ""itemName"": ""Water"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 2.0
        }
      ],
      ""producedSlotCount"": 1,
      ""producedItems"": [
        {
          ""index"": 7,
          ""itemName"": ""Steam"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 6.0
        }
      ],
      ""totalTime"": 20.0,
      ""basePowerOutput"": 0,
      ""powerConsumption"": 2000,
      ""researchPoints"": 0
    },
    {
      ""index"": 4,
      ""buildingName"": ""SteamGenerator"",
      ""buildingType"": ""POWERPLANT"",
      ""buildingClass"": ""CLASS-F"",
      ""consumedSlotCount"": 1,
      ""consumedItems"": [
        {
          ""index"": 7,
          ""itemName"": ""Steam"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 60.0
        }
      ],
      ""producedSlotCount"": 0,
      ""producedItems"": [],
      ""totalTime"": 300.0,
      ""basePowerOutput"": 30000,
      ""powerConsumption"": 0
    },
    {
      ""index"": 5,
      ""buildingName"": ""Furnace"",
      ""buildingType"": ""FACTORY"",
      ""buildingClass"": ""CLASS-F"",
      ""consumedSlotCount"": 1,
      ""consumedItems"": [
        {
          ""index"": 7,
          ""itemName"": ""Steam"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 5.0
        }
      ],
      ""producedSlotCount"": 1,
      ""producedItems"": [
        {
          ""index"": 7,
          ""itemName"": ""Steam"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 5.0
        }
      ],
      ""totalTime"": 10.0,
      ""basePowerOutput"": 30000,
      ""powerConsumption"": 0,
      ""researchPoints"": 0
    },
    {
      ""index"": 6,
      ""buildingName"": ""ResearchDevice"",
      ""buildingType"": ""LABORATORY"",
      ""buildingClass"": ""CLASS-F"",
      ""consumedSlotCount"": 0,
      ""consumedItems"": [],
      ""producedSlotCount"": 0,
      ""producedItems"": [],
      ""totalTime"": 60.0,
      ""basePowerOutput"": 0,
      ""powerConsumption"": 10000,
      ""researchPoints"": 1.0
    },
    {
      ""index"": 7,
      ""buildingName"": ""SmallPowerGrid"",
      ""buildingType"": ""ENERGY"",
      ""buildingClass"": ""CLASS-F"",
      ""consumedSlotCount"": 0,
      ""consumedItems"": [],
      ""producedSlotCount"": 0,
      ""producedItems"": [],
      ""totalTime"": 60.0,
      ""basePowerOutput"": 0,
      ""powerConsumption"": 0,
      ""researchPoints"": 0
    }
  ]
}
";
    }
}
