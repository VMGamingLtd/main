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
      ""minQuantityRange"": 10000,
      ""maxQuantityRange"": 10000000,
      ""productionTime"": 2,
      ""outputValue"": 5,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 1,
      ""recipeName"": ""Water"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""LIQUID"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 2,
      ""outputValue"": 10,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 2,
      ""recipeName"": ""Biofuel"",
      ""recipeProduct"": ""ENHANCED"",
      ""recipeType"": ""LIQUID"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 4,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 8,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 15,
          ""name"": ""LatexFoam"",
          ""product"": ""BASIC"",
          ""type"": ""FOAM"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 20
        },
        {
          ""index"": 20,
          ""name"": ""BioOil"",
          ""product"": ""PROCESSED"",
          ""type"": ""LIQUID"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 1
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
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 3,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 1,
          ""name"": ""Water"",
          ""product"": ""BASIC"",
          ""type"": ""LIQUID"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 60
        }
      ]
    },
    {
      ""index"": 4,
      ""recipeName"": ""Battery"",
      ""recipeProduct"": ""ASSEMBLED"",
      ""recipeType"": ""ENERGY"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 8,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 12,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 2,
          ""name"": ""Biofuel"",
          ""product"": ""ENHANCED"",
          ""type"": ""LIQUID"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 1
        },
        {
          ""index"": 6,
          ""name"": ""BatteryCore"",
          ""product"": ""ENHANCED"",
          ""type"": ""ENERGY"",
          ""itemClass"": ""CLASS-F"",
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
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 8,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true
    },
    {
      ""index"": 6,
      ""recipeName"": ""BatteryCore"",
      ""recipeProduct"": ""ENHANCED"",
      ""recipeType"": ""ENERGY"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 3,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 6,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 2,
          ""name"": ""Biofuel"",
          ""product"": ""ENHANCED"",
          ""type"": ""LIQUID"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 4
        }
      ]
    },
    {
      ""index"": 7,
      ""recipeName"": ""Steam"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""GAS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 2,
      ""outputValue"": 2,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 8,
      ""recipeName"": ""IronOre"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERALS"",
      ""itemClass"": ""CLASS-D"",
      ""experience"": 2,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 10000,
      ""productionTime"": 6,
      ""outputValue"": 4,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 9,
      ""recipeName"": ""Wood"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""PLANTS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 100000,
      ""productionTime"": 4,
      ""outputValue"": 2,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 10,
      ""recipeName"": ""Coal"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERALS"",
      ""itemClass"": ""CLASS-D"",
      ""experience"": 2,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 10000,
      ""productionTime"": 4,
      ""outputValue"": 2,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 11,
      ""recipeName"": ""IronBeam"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""METALS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 4,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 6,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 8,
          ""name"": ""IronOre"",
          ""product"": ""BASIC"",
          ""type"": ""MINERALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 6
        },
        {
          ""index"": 10,
          ""name"": ""Coal"",
          ""product"": ""BASIC"",
          ""type"": ""MINERALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 2
        }
      ]
    },
    {
      ""index"": 12,
      ""recipeName"": ""BiofuelGenerator"",
      ""recipeProduct"": ""BUILDINGS"",
      ""recipeType"": ""POWERPLANT"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 16,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 24,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 11,
          ""name"": ""IronBeam"",
          ""product"": ""PROCESSED"",
          ""type"": ""METALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 4
        },
        {
          ""index"": 9,
          ""name"": ""Wood"",
          ""product"": ""BASIC"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 4
        },
        {
          ""index"": 14,
          ""name"": ""IronRod"",
          ""product"": ""PROCESSED"",
          ""type"": ""METALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 4
        }
      ]
    },
    {
      ""index"": 13,
      ""recipeName"": ""IronSheet"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""METALS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 4,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 8,
          ""name"": ""IronOre"",
          ""product"": ""BASIC"",
          ""type"": ""MINERALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 4
        },
        {
          ""index"": 10,
          ""name"": ""Coal"",
          ""product"": ""BASIC"",
          ""type"": ""MINERALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 2
        }
      ]
    },
    {
      ""index"": 14,
      ""recipeName"": ""IronRod"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""METALS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 4,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 6,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 13,
          ""name"": ""IronSheet"",
          ""product"": ""BASIC"",
          ""type"": ""METALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 1
        },
        {
          ""index"": 10,
          ""name"": ""Coal"",
          ""product"": ""BASIC"",
          ""type"": ""MINERALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 1
        }
      ]
    },
    {
      ""index"": 15,
      ""recipeName"": ""LatexFoam"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""FOAM"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 2,
      ""outputValue"": 4,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 16,
      ""recipeName"": ""ProteinBeans"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""PLANTS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 100000,
      ""productionTime"": 2,
      ""outputValue"": 8,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 17,
      ""recipeName"": ""BiomassLeaves"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""PLANTS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 3,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 5,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 1,
          ""name"": ""FibrousLeaves"",
          ""product"": ""BASIC"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 30
        }
      ]
    },
    {
      ""index"": 18,
      ""recipeName"": ""BiomassWood"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""PLANTS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 3,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 5,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 1,
          ""name"": ""Wood"",
          ""product"": ""BASIC"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 2
        }
      ]
    },
    {
      ""index"": 19,
      ""recipeName"": ""ProteinPowder"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""POWDER"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 3,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 5,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 0,
          ""name"": ""FibrousLeaves"",
          ""product"": ""BASIC"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 35
        },
        {
          ""index"": 16,
          ""name"": ""ProteinBeans"",
          ""product"": ""BASIC"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 60
        }
      ]
    },
    {
      ""index"": 20,
      ""recipeName"": ""BioOil"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""LIQUID"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 3,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 5,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 17,
          ""name"": ""BiomassLeaves"",
          ""product"": ""PROCESSED"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 1
        },
        {
          ""index"": 1,
          ""name"": ""Water"",
          ""product"": ""BASIC"",
          ""type"": ""LIQUID"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 60
        }
      ]
    },
    {
      ""index"": 21,
      ""recipeName"": ""IronTube"",
      ""recipeProduct"": ""PROCESSED"",
      ""recipeType"": ""METALS"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 4,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 6,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 13,
          ""name"": ""IronSheet"",
          ""product"": ""BASIC"",
          ""type"": ""METALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 1
        },
        {
          ""index"": 10,
          ""name"": ""Coal"",
          ""product"": ""BASIC"",
          ""type"": ""MINERALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 1
        }
      ]
    },
    {
      ""index"": 22,
      ""recipeName"": ""WaterPump"",
      ""recipeProduct"": ""BUILDINGS"",
      ""recipeType"": ""PUMPINGFACILITY"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 12,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 18,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 21,
          ""name"": ""IronTube"",
          ""product"": ""PROCESSED"",
          ""type"": ""METALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 4
        },
        {
          ""index"": 13,
          ""name"": ""IronSheet"",
          ""product"": ""BASIC"",
          ""type"": ""METALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 2
        },
        {
          ""index"": 9,
          ""name"": ""Wood"",
          ""product"": ""BASIC"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 2
        }
      ]
    },
    {
      ""index"": 23,
      ""recipeName"": ""FibrousPlantField"",
      ""recipeProduct"": ""BUILDINGS"",
      ""recipeType"": ""AGRICULTURE"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 8,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 30,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 0,
          ""name"": ""FibrousLeaves"",
          ""product"": ""BASIC"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 100
        },
        {
          ""index"": 9,
          ""name"": ""Wood"",
          ""product"": ""BASIC"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 10
        }
      ]
    },
    {
      ""index"": 24,
      ""recipeName"": ""ResearchDevice"",
      ""recipeProduct"": ""BUILDINGS"",
      ""recipeType"": ""LABORATORY"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 16,
      ""minQuantityRange"": -1,
      ""maxQuantityRange"": -1,
      ""productionTime"": 60,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": true,
      ""childDataList"": [
        {
          ""index"": 11,
          ""name"": ""IronBeam"",
          ""product"": ""PROCESSED"",
          ""type"": ""METALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 4
        },      
        {
          ""index"": 13,
          ""name"": ""IronSheet"",
          ""product"": ""BASIC"",
          ""type"": ""METALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 8
        },
        {
          ""index"": 21,
          ""name"": ""IronTube"",
          ""product"": ""PROCESSED"",
          ""type"": ""METALS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 8
        },
        {
          ""index"": 9,
          ""name"": ""Wood"",
          ""product"": ""BASIC"",
          ""type"": ""PLANTS"",
          ""itemClass"": ""CLASS-F"",
          ""quantity"": 12
        }
      ]
    },
    {
      ""index"": 25,
      ""recipeName"": ""FishMeat"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MEAT"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""minQuantityRange"": 100,
      ""maxQuantityRange"": 1000,
      ""productionTime"": 22,
      ""outputValue"": 2,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 26,
      ""recipeName"": ""AnimalMeat"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MEAT"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""minQuantityRange"": 100,
      ""maxQuantityRange"": 1000,
      ""productionTime"": 24,
      ""outputValue"": 4,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 27,
      ""recipeName"": ""AnimalSkin"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""FABRIC"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""minQuantityRange"": 100,
      ""maxQuantityRange"": 1000,
      ""productionTime"": 18,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 28,
      ""recipeName"": ""Milk"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""LIQUID"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""minQuantityRange"": 100,
      ""maxQuantityRange"": 1000,
      ""productionTime"": 22,
      ""outputValue"": 1,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 29,
      ""recipeName"": ""SilicaSand"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERAL"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 1,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 10000,
      ""productionTime"": 4,
      ""outputValue"": 4,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 30,
      ""recipeName"": ""Limestone"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERAL"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 10000,
      ""productionTime"": 6,
      ""outputValue"": 4,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 31,
      ""recipeName"": ""Clay"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERAL"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 10000,
      ""productionTime"": 4,
      ""outputValue"": 2,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 32,
      ""recipeName"": ""Stone"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERAL"",
      ""itemClass"": ""CLASS-F"",
      ""experience"": 2,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 10000,
      ""productionTime"": 6,
      ""outputValue"": 4,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 33,
      ""recipeName"": ""CopperOre"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERAL"",
      ""itemClass"": ""CLASS-D"",
      ""experience"": 2,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 10000,
      ""productionTime"": 6,
      ""outputValue"": 4,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 34,
      ""recipeName"": ""SilverOre"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERAL"",
      ""itemClass"": ""CLASS-C"",
      ""experience"": 4,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 5000,
      ""productionTime"": 8,
      ""outputValue"": 2,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    },
    {
      ""index"": 35,
      ""recipeName"": ""GoldOre"",
      ""recipeProduct"": ""BASIC"",
      ""recipeType"": ""MINERAL"",
      ""itemClass"": ""CLASS-C"",
      ""experience"": 4,
      ""minQuantityRange"": 1000,
      ""maxQuantityRange"": 5000,
      ""productionTime"": 8,
      ""outputValue"": 2,
      ""currentQuantity"": -1,
      ""hasRequirements"": false
    }
  ]
}
";
    }
}
