using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
          ""itemQuality"": ""PROCESSED"",
          ""quantity"": 1.0
        }
      ],
      ""producedSlotCount"": 0,
      ""producedItems"": [],
      ""totalTime"": 10.0,
      ""basePowerOutput"": 10000,
      ""powerConsumption"": 0
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
          ""quantity"": 4.0
        }
      ],
      ""totalTime"": 10.0,
      ""basePowerOutput"": 0,
      ""powerConsumption"": 2000
    },
    {
      ""index"": 2,
      ""buildingName"": ""PlantField"",
      ""buildingType"": ""AGRICULTURE"",
      ""buildingClass"": ""CLASS-F"",
      ""consumedSlotCount"": 1,
      ""consumedItems"": [
        {
          ""index"": 1,
          ""itemName"": ""Water"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 1.0
        }
      ],
      ""producedSlotCount"": 1,
      ""producedItems"": [
        {
          ""index"": 0,
          ""itemName"": ""FibrousLeaves"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 4.0
        }
      ],
      ""totalTime"": 20.0,
      ""basePowerOutput"": 0,
      ""powerConsumption"": 2000
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
          ""quantity"": 1.0
        }
      ],
      ""producedSlotCount"": 1,
      ""producedItems"": [
        {
          ""index"": 7,
          ""itemName"": ""Steam"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 2.0
        }
      ],
      ""totalTime"": 20.0,
      ""basePowerOutput"": 0,
      ""powerConsumption"": 2000
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
          ""quantity"": 1.0
        }
      ],
      ""producedSlotCount"": 0,
      ""producedItems"": [],
      ""totalTime"": 10.0,
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
          ""quantity"": 1.0
        }
      ],
      ""producedSlotCount"": 1,
      ""producedItems"": [
        {
          ""index"": 7,
          ""itemName"": ""Steam"",
          ""itemQuality"": ""BASIC"",
          ""quantity"": 1.0
        }
      ],
      ""totalTime"": 10.0,
      ""basePowerOutput"": 30000,
      ""powerConsumption"": 0
    }
  ]
}
";
    }
}
