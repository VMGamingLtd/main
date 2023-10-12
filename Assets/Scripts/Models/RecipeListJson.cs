using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    internal class RecipeListJson
    {
        public static string json = @"
{
  ""header"": ""Available recipes"",
  ""recipes"": [
    {
      ""index"": 0,
      ""recipeName"": ""FibrousLeaves"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""PLANTS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""productionTime"": 2,
      ""outputValue"": 5,
      ""hasRequirements"": false
    },
    {
      ""index"": 1,
      ""recipeName"": ""Water"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""LIQUID"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""productionTime"": 2,
      ""outputValue"": 10,
      ""hasRequirements"": false
    },
    {
      ""index"": 2,
      ""recipeName"": ""Biofuel"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""LIQUID"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""productionTime"": 3,
      ""outputValue"": 1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 0,
          ""name"": ""FibrousLeaves"",
          ""product"": ""BASIC"",
          ""quantity"": 20
        }
      ]
    },
    {
      ""index"": 3,
      ""recipeName"": ""DistilledWater"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""LIQUID"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""productionTime"": 3,
      ""outputValue"": 1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 1,
          ""name"": ""Water"",
          ""product"": ""BASIC"",
          ""quantity"": 50
        }
      ]
    },
    {
      ""index"": 4,
      ""recipeName"": ""Battery"",
      ""recipeProduct"": ""ASSEMBLED"",
      ""recipeType"": ""ENERGY"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 4,
      ""productionTime"": 8,
      ""outputValue"": 1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 2,
          ""name"": ""Biofuel"",
          ""product"": ""PROCESSED"",
          ""quantity"": 1
        },
        {
          ""index"": 6,
          ""name"": ""BatteryCore"",
          ""product"": ""ENHANCED"",
          ""quantity"": 1
        }
      ]
    },
    {
      ""index"": 5,
      ""recipeName"": ""OxygenTank"",
      ""recipeProduct"": ""ASSEMBLED"",
      ""recipeType"": ""OXYGEN"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 4,
      ""productionTime"": 8,
      ""outputValue"": 1,
      ""hasRequirements"": true
    },
    {
      ""index"": 6,
      ""recipeName"": ""BatteryCore"",
      ""recipeProduct"": ""ENHANCED"",
      ""recipeType"": ""ENERGY"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 3,
      ""productionTime"": 6,
      ""outputValue"": 1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 2,
          ""name"": ""Biofuel"",
          ""product"": ""PROCESSED"",
          ""quantity"": 4
        }
      ]
    },
    {
      ""index"": 7,
      ""recipeName"": ""Wood"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""PLANTS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""productionTime"": 4,
      ""outputValue"": 2,
      ""hasRequirements"": false
    },
    {
      ""index"": 8,
      ""recipeName"": ""IronOre"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERALS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""productionTime"": 4,
      ""outputValue"": 4,
      ""hasRequirements"": false
    },
    {
      ""index"": 9,
      ""recipeName"": ""Coal"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERALS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""productionTime"": 4,
      ""outputValue"": 2,
      ""hasRequirements"": false
    },
    {
      ""index"": 10,
      ""recipeName"": ""IronBeam"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""METALS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 4,
      ""productionTime"": 6,
      ""outputValue"": 1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 8,
          ""name"": ""IronOre"",
          ""product"": ""BASIC"",
          ""quantity"": 6
        },
        {
          ""index"": 9,
          ""name"": ""Coal"",
          ""product"": ""BASIC"",
          ""quantity"": 2
        }
      ]
    },
    {
      ""index"": 11,
      ""recipeName"": ""BiofuelGenerator"",
      ""recipeProduct"": ""BUILDINGS"",
      ""recipeType"": ""POWERPLANT"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 16,
      ""productionTime"": 24,
      ""outputValue"": 1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 10,
          ""name"": ""IronBeam"",
          ""product"": ""PROCESSED"",
          ""quantity"": 4
        },
        {
          ""index"": 7,
          ""name"": ""Wood"",
          ""product"": ""BASIC"",
          ""quantity"": 4
        }
      ]
    }
  ]
}
";
    }
}
