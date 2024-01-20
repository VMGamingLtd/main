using Assets.Scripts.ItemFactory;
using BuildingManagement;
using ItemManagement;
using Newtonsoft.Json;
using RecipeManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    private const string FileName = "Savegame.json";
    private string filePath;
    public InventoryManager inventoryManager;
    public RecipeManager recipeManager;
    public BuildingManager buildingManager;
    private SaveDataModel currentSaveData;
    public class SaveDataModel
    {
        public string title;
        public string username;
        public string password;
        public string email;
        public string showItemProducts;
        public string showRecipeProducts;
        public string showItemTypes;
        public string showRecipeTypes;
        public string showItemClass;
        public string showRecipeClass;
        public string planet0Name;
        public string planet0Weather;
        public int planet0UV;
        public string planet0WindStatus;
        public string MenuButtonTypeOn;
        public int hours;
        public int minutes;
        public int seconds;
        public bool registeredUser;
        public bool firstGoal;
        public bool secondGoal;
        public bool thirdGoal;
        public bool fourthGoal;
        public bool isPlayerInBiologicalBiome;
        public bool isDraggingBuilding;
        public bool autoConsumption;
        public bool fibrousPlantFieldUnlocked;
        public bool waterPumpUnlocked;
        public float credits;
        public float playerMovementSpeed;
        public float AchievementPoints;
        public string inventoryTitle;
        public ItemDataModel[] basicInventoryObjects;
        public ItemDataModel[] processedInventoryObjects;
        public ItemDataModel[] enhancedInventoryObjects;
        public ItemDataModel[] assembledInventoryObjects;
        public SuitDataModel[] suitInventoryObjects;
        public HelmetDataModel[] helmetInventoryObjects;
        public ToolDataModel[] toolInventoryObjects;
        public string recipeTitle;
        public RecipeItemDataModel[] basicRecipeObjects;
        public RecipeItemDataModel[] processedRecipeObjects;
        public RecipeItemDataModel[] enhancedRecipeObjects;
        public RecipeItemDataModel[] assembledRecipeObjects;
        public RecipeItemDataModel[] buildingRecipeObjects;
        public string BuildingStatisticProcess;
        public string BuildingStatisticType;
        public string BuildingStatisticInterval;
        public string[] slotEquippedName;
        public bool BuildingStatisticTypeChanged;
        public bool BuildingIntervalTypeChanged;
        public bool[] slotEquipped;
        public int ItemCreationID;
        public int BuildingUniqueID;
        public int RecipeOrderAdded;
        public BuildingItemDataModel[] agriculture;
        public BuildingItemDataModel[] pumpingFacility;
        public BuildingItemDataModel[] factory;
        public BuildingItemDataModel[] commFacility;
        public BuildingItemDataModel[] storageHouse;
        public BuildingItemDataModel[] navalFacility;
        public BuildingItemDataModel[] oxygenFacility;
        public BuildingItemDataModel[] aviationFacility;
        public BuildingItemDataModel[] heatingFacility;
        public BuildingItemDataModel[] coolingFacility;
        public EnergyBuildingItemDataModel[] powerplant;
        public BuildingItemDataModel[] oxygenStation;
        public BuildingItemDataModel[] miningRig;
        public Dictionary<string, object> Planet0StaticVariables = new();
        public Dictionary<string, object> PlayerStaticVariables = new();
        public Dictionary<string, object> PlayerResourcesStaticVariables = new();
        public List<EventIconModel> EventObjects = new();
    }

    [Serializable]
    public class EventIconModel
    {
        public string Name;
        public Vector3 position;
    }

    [Serializable]
    public class SuitDataModel
    {
        public int ID;
        public int index;
        public float stackLimit;
        public float itemQuantity;
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
        public bool equipable;
        public bool isEquipped;
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int gasProtection;
        public int explosionProtection;
        public int shieldPoints;
        public int hitPoints;
        public int energyCapacity;
        public int durability;
        public int maxDurability;
        public int inventorySlots;
        public int strength;
        public int perception;
        public int intelligence;
        public int agility;
        public int charisma;
        public int willpower;
    }

    [Serializable]
    public class HelmetDataModel
    {
        public int ID;
        public int index;
        public float stackLimit;
        public float itemQuantity;
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
        public bool equipable;
        public bool isEquipped;
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int gasProtection;
        public int explosionProtection;
        public int shieldPoints;
        public int hitPoints;
        public int durability;
        public int maxDurability;
        public int strength;
        public int perception;
        public int intelligence;
        public int agility;
        public int charisma;
        public int willpower;
        public int visibilityRadius;
        public int explorationRadius;
        public int pickupRadius;
    }

    [Serializable]
    public class ToolDataModel
    {
        public int ID;
        public int index;
        public float stackLimit;
        public float itemQuantity;
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
        public bool equipable;
        public bool isEquipped;
        public int durability;
        public int maxDurability;
        public int strength;
        public int perception;
        public int intelligence;
        public int agility;
        public int charisma;
        public int willpower;
        public float productionSpeed;
        public float materialCost;
        public float outcomeRate;
    }

    [Serializable]
    public class ItemDataModel
    {
        public int ID;
        public int index;
        public float stackLimit;
        public float itemQuantity;
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
        public bool equipable;
        public bool isEquipped;
    }
    [Serializable]
    public class RecipeItemDataModel
    {
        public int index;
        public int orderAdded;
        public string recipeName;
        public string recipeProduct;
        public string recipeType;
        public string itemClass;
        public int experience;
        public float productionTime;
        public float outputValue;
        public bool hasRequirements;
        public List<ChildData> childData;
    }
    [System.Serializable]
    public class EnergyBuildingItemDataModel
    {
        public int index;
        public int ID;
        public string spriteIconName;
        public string buildingType;
        public string buildingName;
        public string buildingClass;
        public Vector3 buildingPosition;
        public List<SlotItemData> consumedItems;
        public PowerCycle powerCycleData = new();
        public int consumedSlotCount;
        public float timer;
        public float totalTime;
        public int secondCycleCount;
        public int minuteCycleCount;
        public int hourCycleCount;
        public int powerOutput;
        public int basePowerOutput;
        public int actualPowerOutput;
        public int producedSlotCount;
        public int efficiency;
        public int efficiencySetting;
        public int buildingCount;
        public bool isPaused;
        public bool enlistedProduction;
    }
    [System.Serializable]
    public class BuildingItemDataModel
    {
        public int index;
        public int ID;
        public string spriteIconName;
        public string buildingType;
        public string buildingName;
        public string buildingClass;
        public Vector3 buildingPosition;
        public List<SlotItemData> consumedItems;
        public List<SlotItemData> producedItems;
        public PowerConsumptionCycle powerConsumptionCycleData = new();
        public ProductionCycle productionCycleData = new();
        public int consumedSlotCount;
        public float timer;
        public float totalTime;
        public int secondCycleCount;
        public int minuteCycleCount;
        public int hourCycleCount;
        public int powerConsumption;
        public int actualPowerConsumption;
        public int producedSlotCount;
        public int efficiency;
        public int efficiencySetting;
        public int buildingCount;
        public bool isPaused;
        public bool enlistedProduction;
    }

    /// <summary>
    /// Searches a class and stores all variables from it into an object.
    /// </summary>
    /// <param name="staticClassType"></param>
    public void AssignFromStaticClass(Type staticClassType)
    {
        FieldInfo[] fields = staticClassType.GetFields(BindingFlags.Public | BindingFlags.Static);
        FieldInfo[] thisFields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            // only PUBLIC STATIC variables
            if (field.IsPublic && field.IsStatic)
            {
                FieldInfo correspondingField = Array.Find(thisFields, f => f.Name == field.Name);
                correspondingField?.SetValue(this, field.GetValue(null));
            }
        }
    }
    public static void LoadStaticVariablesFromModel(SaveDataModel saveDataModel)
    {
        foreach (var kvp in saveDataModel.Planet0StaticVariables)
        {
            FieldInfo field = typeof(Planet0Buildings).GetField(kvp.Key, BindingFlags.Public | BindingFlags.Static);
            if (field != null)
            {
                //Debug.Log($"{kvp.Key} / {kvp.Value} / {kvp.Value.GetType()}");
                // Convert the value to float before setting
                if (kvp.Value is double doubleValue)
                {
                    field.SetValue(null, (float)doubleValue);
                }
                else if (kvp.Value is float floatValue)
                {
                    field.SetValue(null, floatValue);
                }
                else if (kvp.Value is int intValue)
                {
                    field.SetValue(null, intValue);
                }
                else if (kvp.Value is long longValue)
                {
                    field.SetValue(null, (int)longValue);
                }
                else if (kvp.Value is bool boolValue)
                {
                    field.SetValue(null, boolValue);
                }
            }
        }
        foreach (var kvp in saveDataModel.PlayerResourcesStaticVariables)
        {
            FieldInfo field = typeof(PlayerResources).GetField(kvp.Key, BindingFlags.Public | BindingFlags.Static);
            if (field != null)
            {
                // Convert the value to float before setting
                if (kvp.Value is double doubleValue)
                {
                    field.SetValue(null, (float)doubleValue);
                }
                else if (kvp.Value is float floatValue)
                {
                    field.SetValue(null, floatValue);
                }
                else if (kvp.Value is int intValue)
                {
                    field.SetValue(null, intValue);
                }
                else if (kvp.Value is bool boolValue)
                {
                    field.SetValue(null, boolValue);
                }
            }
        }
        foreach (var kvp in saveDataModel.PlayerStaticVariables)
        {
            FieldInfo field = typeof(Player).GetField(kvp.Key, BindingFlags.Public | BindingFlags.Static);
            if (field != null)
            {
                // Convert the value to float before setting
                if (kvp.Value is double doubleValue)
                {
                    field.SetValue(null, (float)doubleValue);
                }
                else if (kvp.Value is float floatValue)
                {
                    field.SetValue(null, floatValue);
                }
                // since we use mainly Integers, JSON saves them as longs
                else if (kvp.Value is long intValue)
                {
                    field.SetValue(null, (int)intValue);
                }
                else if (kvp.Value is bool boolValue)
                {
                    field.SetValue(null, boolValue);
                }
            }
        }
    }

    private void Start()
    {
        // Set the file path to the persistent data path with the file name
        filePath = Path.Combine(Application.persistentDataPath, FileName);

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

    public string SerializeGameData()
    {
        // Create the custom settings with the custom contract resolver
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CustomContractResolver()
        };

        currentSaveData.AchievementPoints = Achievements.AchievementPoints;
        currentSaveData.username = UserName.userName;
        currentSaveData.password = Password.password;
        currentSaveData.email = Email.email;
        currentSaveData.showItemProducts = InventoryManager.ShowItemProducts;
        currentSaveData.showRecipeProducts = RecipeManager.ShowRecipeProducts;
        currentSaveData.showItemTypes = InventoryManager.ShowItemTypes;
        currentSaveData.showRecipeTypes = RecipeManager.ShowRecipeTypes;
        currentSaveData.showItemClass = InventoryManager.ShowItemClass;
        currentSaveData.showRecipeClass = RecipeManager.ShowRecipeClass;
        currentSaveData.planet0Name = Planet0Name.planet0Name;
        currentSaveData.planet0WindStatus = WindManager.Planet0WindStatus;
        currentSaveData.planet0UV = WeatherManager.planet0UV;
        currentSaveData.planet0Weather = WeatherManager.planet0Weather;
        currentSaveData.hours = GlobalCalculator.hours;
        currentSaveData.minutes = GlobalCalculator.minutes;
        currentSaveData.seconds = GlobalCalculator.seconds;
        currentSaveData.registeredUser = CoroutineManager.registeredUser;
        currentSaveData.firstGoal = GoalManager.firstGoal;
        currentSaveData.secondGoal = GoalManager.secondGoal;
        currentSaveData.thirdGoal = GoalManager.thirdGoal;
        currentSaveData.fourthGoal = GoalManager.fourthGoal;
        currentSaveData.isPlayerInBiologicalBiome = GlobalCalculator.isPlayerInBiologicalBiome;
        currentSaveData.credits = Credits.credits;
        currentSaveData.MenuButtonTypeOn = ButtonManager.MenuButtonTypeOn;
        currentSaveData.isDraggingBuilding = BuildingManager.isDraggingBuilding;
        currentSaveData.BuildingStatisticProcess = BuildingStatisticsManager.BuildingStatisticProcess;
        currentSaveData.BuildingStatisticType = BuildingStatisticsManager.BuildingStatisticType;
        currentSaveData.BuildingStatisticInterval = BuildingStatisticsManager.BuildingStatisticInterval;
        currentSaveData.BuildingIntervalTypeChanged = BuildingIntervalTypes.BuildingIntervalTypeChanged;
        currentSaveData.BuildingStatisticTypeChanged = BuildingStatisticTypes.BuildingStatisticTypeChanged;
        currentSaveData.ItemCreationID = ItemFactory.ItemCreationID;
        currentSaveData.BuildingUniqueID = BuildingCreator.BuildingUniqueID;
        currentSaveData.RecipeOrderAdded = RecipeCreator.recipeOrderAdded;
        currentSaveData.autoConsumption = EquipmentManager.autoConsumption;

        // Planet0Building class storage
        FieldInfo[] fields = typeof(Planet0Buildings).GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo field in fields)
        {
            currentSaveData.Planet0StaticVariables[field.Name] = field.GetValue(null);
        }

        // PlayerResources class storage
        FieldInfo[] fields2 = typeof(PlayerResources).GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo field in fields2)
        {
            currentSaveData.PlayerResourcesStaticVariables[field.Name] = field.GetValue(null);
        }

        // Player class storage
        // ATTENTION, THIS CLASS USES ONLY INTEGERS, BUT JSON CONVERTS THEM AS INT64, SO DURING DESERIALIZATION
        // WE HAVE TO CONVERT THEM FROM LONG(INT64 TO INT - SEE ROW #301)!
        FieldInfo[] fields3 = typeof(Player).GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (FieldInfo field in fields3)
        {
            currentSaveData.PlayerStaticVariables[field.Name] = field.GetValue(null);
        }

        // slot equip array
        currentSaveData.slotEquipped = EquipmentManager.slotEquipped;
        currentSaveData.slotEquippedName = EquipmentManager.slotEquippedName;

        // store all Planet event objects with their coordinates
        currentSaveData.EventObjects.Clear();
        Planet planet = GameObject.Find("PlanetParent/StartPlanet").GetComponent<Planet>();
        foreach (GameObject eventObject in planet.eventObjects)
        {
            EventIconModel itemData = new()
            {
                Name = eventObject.name,
                position = eventObject.transform.position,
            };

            currentSaveData.EventObjects.Add(itemData);
        }

        // Access the itemArrays dictionary through the inventoryManager reference
        Dictionary<string, GameObject[]> itemArrays = inventoryManager.itemArrays;

        currentSaveData.basicInventoryObjects = new ItemDataModel[itemArrays["BASIC"].Length];

        for (int i = 0; i < itemArrays["BASIC"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["BASIC"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemDataComponent.itemName = itemDataComponent.itemName.Replace("(Clone)", "");
            ItemDataModel itemData = new()
            {
                ID = itemDataComponent.ID,
                index = itemDataComponent.index,
                stackLimit = itemDataComponent.stackLimit,
                itemQuantity = itemDataComponent.quantity,
                itemProduct = itemDataComponent.itemProduct,
                itemType = itemDataComponent.itemType,
                itemClass = itemDataComponent.itemClass,
                itemName = itemDataComponent.itemName,
                equipable = itemDataComponent.equipable,
                isEquipped = itemDataComponent.isEquipped
            };

            currentSaveData.basicInventoryObjects[i] = itemData;
        }

        currentSaveData.processedInventoryObjects = new ItemDataModel[itemArrays["PROCESSED"].Length];

        for (int i = 0; i < itemArrays["PROCESSED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["PROCESSED"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemDataComponent.itemName = itemDataComponent.itemName.Replace("(Clone)", "");
            ItemDataModel itemData = new()
            {
                ID = itemDataComponent.ID,
                index = itemDataComponent.index,
                stackLimit = itemDataComponent.stackLimit,
                itemQuantity = itemDataComponent.quantity,
                itemProduct = itemDataComponent.itemProduct,
                itemType = itemDataComponent.itemType,
                itemClass = itemDataComponent.itemClass,
                itemName = itemDataComponent.itemName,
                equipable = itemDataComponent.equipable,
                isEquipped = itemDataComponent.isEquipped
            };

            currentSaveData.processedInventoryObjects[i] = itemData;
        }
        currentSaveData.enhancedInventoryObjects = new ItemDataModel[itemArrays["ENHANCED"].Length];

        for (int i = 0; i < itemArrays["ENHANCED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["ENHANCED"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemDataComponent.itemName = itemDataComponent.itemName.Replace("(Clone)", "");
            ItemDataModel itemData = new()
            {
                ID = itemDataComponent.ID,
                index = itemDataComponent.index,
                stackLimit = itemDataComponent.stackLimit,
                itemQuantity = itemDataComponent.quantity,
                itemProduct = itemDataComponent.itemProduct,
                itemType = itemDataComponent.itemType,
                itemClass = itemDataComponent.itemClass,
                itemName = itemDataComponent.itemName,
                equipable = itemDataComponent.equipable,
                isEquipped = itemDataComponent.isEquipped
            };

            currentSaveData.enhancedInventoryObjects[i] = itemData;
        }

        // assembled general inventory objects
        currentSaveData.assembledInventoryObjects = new ItemDataModel[itemArrays["ASSEMBLED"].Length];
        currentSaveData.suitInventoryObjects = new SuitDataModel[itemArrays["ASSEMBLED"].Length];
        currentSaveData.helmetInventoryObjects = new HelmetDataModel[itemArrays["ASSEMBLED"].Length];
        currentSaveData.toolInventoryObjects = new ToolDataModel[itemArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemArrays["ASSEMBLED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["ASSEMBLED"][i];
            if (itemGameObject.TryGetComponent(out ItemData itemDataComponent))
            {
                itemDataComponent.itemName = itemDataComponent.itemName.Replace("(Clone)", "");
                if (itemDataComponent.itemType == "SUIT")
                {
                    SuitData suitDataComponent = itemGameObject.GetComponent<SuitData>();
                    SuitDataModel suitData = new()
                    {
                        ID = suitDataComponent.ID,
                        index = suitDataComponent.index,
                        stackLimit = suitDataComponent.stackLimit,
                        itemQuantity = suitDataComponent.quantity,
                        itemProduct = suitDataComponent.itemProduct,
                        itemType = suitDataComponent.itemType,
                        itemClass = suitDataComponent.itemClass,
                        itemName = suitDataComponent.itemName,
                        equipable = suitDataComponent.equipable,
                        isEquipped = suitDataComponent.isEquipped,
                        physicalProtection = suitDataComponent.physicalProtection,
                        fireProtection = suitDataComponent.fireProtection,
                        coldProtection = suitDataComponent.coldProtection,
                        gasProtection = suitDataComponent.gasProtection,
                        explosionProtection = suitDataComponent.explosionProtection,
                        shieldPoints = suitDataComponent.shieldPoints,
                        hitPoints = suitDataComponent.hitPoints,
                        energyCapacity = suitDataComponent.energyCapacity,
                        durability = suitDataComponent.durability,
                        maxDurability = suitDataComponent.maxDurability,
                        inventorySlots = suitDataComponent.inventorySlots,
                        strength = suitDataComponent.strength,
                        perception = suitDataComponent.perception,
                        intelligence = suitDataComponent.intelligence,
                        agility = suitDataComponent.agility,
                        charisma = suitDataComponent.charisma,
                        willpower = suitDataComponent.willpower
                    };
                    currentSaveData.suitInventoryObjects[i] = suitData;
                }
                else if (itemDataComponent.itemType == "HELMET")
                {
                    HelmetData helmetDataComponent = itemGameObject.GetComponent<HelmetData>();
                    HelmetDataModel helmetData = new()
                    {
                        ID = helmetDataComponent.ID,
                        index = helmetDataComponent.index,
                        stackLimit = helmetDataComponent.stackLimit,
                        itemQuantity = helmetDataComponent.quantity,
                        itemProduct = helmetDataComponent.itemProduct,
                        itemType = helmetDataComponent.itemType,
                        itemClass = helmetDataComponent.itemClass,
                        itemName = helmetDataComponent.itemName,
                        equipable = helmetDataComponent.equipable,
                        isEquipped = helmetDataComponent.isEquipped,
                        physicalProtection = helmetDataComponent.physicalProtection,
                        fireProtection = helmetDataComponent.fireProtection,
                        coldProtection = helmetDataComponent.coldProtection,
                        gasProtection = helmetDataComponent.gasProtection,
                        explosionProtection = helmetDataComponent.explosionProtection,
                        shieldPoints = helmetDataComponent.shieldPoints,
                        hitPoints = helmetDataComponent.hitPoints,
                        durability = helmetDataComponent.durability,
                        maxDurability = helmetDataComponent.maxDurability,
                        strength = helmetDataComponent.strength,
                        perception = helmetDataComponent.perception,
                        intelligence = helmetDataComponent.intelligence,
                        agility = helmetDataComponent.agility,
                        charisma = helmetDataComponent.charisma,
                        willpower = helmetDataComponent.willpower,
                        visibilityRadius = helmetDataComponent.visibilityRadius,
                        explorationRadius = helmetDataComponent.explorationRadius,
                        pickupRadius = helmetDataComponent.pickupRadius
                    };
                    currentSaveData.helmetInventoryObjects[i] = helmetData;
                }
                else if (itemDataComponent.itemType == "FABRICATOR")
                {
                    ToolData toolDataComponent = itemGameObject.GetComponent<ToolData>();
                    ToolDataModel toolData = new()
                    {
                        ID = toolDataComponent.ID,
                        index = toolDataComponent.index,
                        stackLimit = toolDataComponent.stackLimit,
                        itemQuantity = toolDataComponent.quantity,
                        itemProduct = toolDataComponent.itemProduct,
                        itemType = toolDataComponent.itemType,
                        itemClass = toolDataComponent.itemClass,
                        itemName = toolDataComponent.itemName,
                        equipable = toolDataComponent.equipable,
                        isEquipped = toolDataComponent.isEquipped,
                        durability = toolDataComponent.durability,
                        maxDurability = toolDataComponent.maxDurability,
                        strength = toolDataComponent.strength,
                        perception = toolDataComponent.perception,
                        intelligence = toolDataComponent.intelligence,
                        agility = toolDataComponent.agility,
                        charisma = toolDataComponent.charisma,
                        willpower = toolDataComponent.willpower,
                        productionSpeed = toolDataComponent.productionSpeed,
                        materialCost = toolDataComponent.materialCost,
                        outcomeRate = toolDataComponent.outcomeRate
                    };
                    currentSaveData.toolInventoryObjects[i] = toolData;
                }
                else
                {
                    ItemDataModel itemData = new()
                    {
                        ID = itemDataComponent.ID,
                        index = itemDataComponent.index,
                        stackLimit = itemDataComponent.stackLimit,
                        itemQuantity = itemDataComponent.quantity,
                        itemProduct = itemDataComponent.itemProduct,
                        itemType = itemDataComponent.itemType,
                        itemClass = itemDataComponent.itemClass,
                        itemName = itemDataComponent.itemName,
                        equipable = itemDataComponent.equipable,
                        isEquipped = itemDataComponent.isEquipped
                    };
                    currentSaveData.assembledInventoryObjects[i] = itemData;
                }

            }
        }

        // Access the itemRecipeArrays dictionary through the RecipeManager reference
        Dictionary<string, GameObject[]> itemRecipeArrays = recipeManager.itemRecipeArrays;

        currentSaveData.basicRecipeObjects = new RecipeItemDataModel[itemRecipeArrays["BASIC"].Length];

        for (int i = 0; i < itemRecipeArrays["BASIC"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["BASIC"][i];
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemDataComponent.recipeName = itemDataComponent.recipeName.Replace("(Clone)", "");
            RecipeItemDataModel recipeData = new()
            {
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                hasRequirements = itemDataComponent.hasRequirements,
                childData = itemDataComponent.childData
            };

            currentSaveData.basicRecipeObjects[i] = recipeData;
        }

        currentSaveData.processedRecipeObjects = new RecipeItemDataModel[itemRecipeArrays["PROCESSED"].Length];

        for (int i = 0; i < itemRecipeArrays["PROCESSED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["PROCESSED"][i];
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemDataComponent.recipeName = itemDataComponent.recipeName.Replace("(Clone)", "");
            RecipeItemDataModel recipeData = new()
            {
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                hasRequirements = itemDataComponent.hasRequirements,
                childData = itemDataComponent.childData
            };

            currentSaveData.processedRecipeObjects[i] = recipeData;
        }
        currentSaveData.enhancedRecipeObjects = new RecipeItemDataModel[itemRecipeArrays["ENHANCED"].Length];

        for (int i = 0; i < itemRecipeArrays["ENHANCED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["ENHANCED"][i];
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemDataComponent.recipeName = itemDataComponent.recipeName.Replace("(Clone)", "");
            RecipeItemDataModel recipeData = new()
            {
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                hasRequirements = itemDataComponent.hasRequirements,
                childData = itemDataComponent.childData
            };

            currentSaveData.enhancedRecipeObjects[i] = recipeData;
        }
        currentSaveData.assembledRecipeObjects = new RecipeItemDataModel[itemRecipeArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemRecipeArrays["ASSEMBLED"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["ASSEMBLED"][i];
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemDataComponent.recipeName = itemDataComponent.recipeName.Replace("(Clone)", "");
            RecipeItemDataModel recipeData = new()
            {
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                hasRequirements = itemDataComponent.hasRequirements,
                childData = itemDataComponent.childData
            };

            currentSaveData.assembledRecipeObjects[i] = recipeData;
        }
        currentSaveData.buildingRecipeObjects = new RecipeItemDataModel[itemRecipeArrays["BUILDINGS"].Length];

        for (int i = 0; i < itemRecipeArrays["BUILDINGS"].Length; i++)
        {
            GameObject itemGameObject = itemRecipeArrays["BUILDINGS"][i];
            RecipeItemData itemDataComponent = itemGameObject.GetComponent<RecipeItemData>();
            itemDataComponent.recipeName = itemDataComponent.recipeName.Replace("(Clone)", "");
            RecipeItemDataModel recipeData = new()
            {
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                hasRequirements = itemDataComponent.hasRequirements,
                childData = itemDataComponent.childData
            };

            currentSaveData.buildingRecipeObjects[i] = recipeData;
        }

        // Access the buildingArrays dictionary through the BuildingManager reference
        Dictionary<string, GameObject[]> buildingArrays = buildingManager.buildingArrays;

        currentSaveData.agriculture = new BuildingItemDataModel[buildingArrays["AGRICULTURE"].Length];

        for (int i = 0; i < buildingArrays["AGRICULTURE"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["AGRICULTURE"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.agriculture[i] = buildingData;
        }

        currentSaveData.pumpingFacility = new BuildingItemDataModel[buildingArrays["PUMPINGFACILITY"].Length];

        for (int i = 0; i < buildingArrays["PUMPINGFACILITY"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["PUMPINGFACILITY"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.pumpingFacility[i] = buildingData;
        }

        currentSaveData.factory = new BuildingItemDataModel[buildingArrays["FACTORY"].Length];

        for (int i = 0; i < buildingArrays["FACTORY"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["FACTORY"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.factory[i] = buildingData;
        }

        currentSaveData.commFacility = new BuildingItemDataModel[buildingArrays["COMMFACILITY"].Length];

        for (int i = 0; i < buildingArrays["COMMFACILITY"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["COMMFACILITY"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.commFacility[i] = buildingData;
        }

        currentSaveData.storageHouse = new BuildingItemDataModel[buildingArrays["STORAGEHOUSE"].Length];

        for (int i = 0; i < buildingArrays["STORAGEHOUSE"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["STORAGEHOUSE"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.storageHouse[i] = buildingData;
        }

        currentSaveData.navalFacility = new BuildingItemDataModel[buildingArrays["NAVALFACILITY"].Length];

        for (int i = 0; i < buildingArrays["NAVALFACILITY"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["NAVALFACILITY"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.navalFacility[i] = buildingData;
        }

        currentSaveData.oxygenFacility = new BuildingItemDataModel[buildingArrays["OXYGENFACILITY"].Length];

        for (int i = 0; i < buildingArrays["OXYGENFACILITY"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["OXYGENFACILITY"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.oxygenFacility[i] = buildingData;
        }

        currentSaveData.aviationFacility = new BuildingItemDataModel[buildingArrays["AVIATIONFACILITY"].Length];

        for (int i = 0; i < buildingArrays["AVIATIONFACILITY"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["AVIATIONFACILITY"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.aviationFacility[i] = buildingData;
        }

        currentSaveData.heatingFacility = new BuildingItemDataModel[buildingArrays["HEATINGFACILITY"].Length];

        for (int i = 0; i < buildingArrays["HEATINGFACILITY"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["HEATINGFACILITY"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.heatingFacility[i] = buildingData;
        }

        currentSaveData.coolingFacility = new BuildingItemDataModel[buildingArrays["COOLINGFACILITY"].Length];

        for (int i = 0; i < buildingArrays["COOLINGFACILITY"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["COOLINGFACILITY"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.coolingFacility[i] = buildingData;
        }

        currentSaveData.powerplant = new EnergyBuildingItemDataModel[buildingArrays["POWERPLANT"].Length];

        for (int i = 0; i < buildingArrays["POWERPLANT"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["POWERPLANT"][i];
            EnergyBuildingItemData itemDataComponent = itemGameObject.GetComponent<EnergyBuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            EnergyBuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                powerCycleData = itemDataComponent.powerCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerOutput = itemDataComponent.powerOutput,
                basePowerOutput = itemDataComponent.basePowerOutput,
                actualPowerOutput = itemDataComponent.actualPowerOutput,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.powerplant[i] = buildingData;
        }

        currentSaveData.oxygenStation = new BuildingItemDataModel[buildingArrays["OXYGENSTATION"].Length];

        for (int i = 0; i < buildingArrays["OXYGENSTATION"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["OXYGENSTATION"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.oxygenStation[i] = buildingData;
        }

        currentSaveData.miningRig = new BuildingItemDataModel[buildingArrays["MININGRIG"].Length];

        for (int i = 0; i < buildingArrays["MININGRIG"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["MININGRIG"][i];
            BuildingItemData itemDataComponent = itemGameObject.GetComponent<BuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            BuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                producedItems = itemDataComponent.producedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                productionCycleData = itemDataComponent.productionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                secondCycleCount = itemDataComponent.secondCycleCount,
                minuteCycleCount = itemDataComponent.minuteCycleCount,
                hourCycleCount = itemDataComponent.hourCycleCount,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                producedSlotCount = itemDataComponent.producedSlotCount,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction
            };

            currentSaveData.miningRig[i] = buildingData;
        }

        string jsonString = JsonConvert.SerializeObject(currentSaveData, Formatting.Indented, settings);
        return jsonString;

    }
    public void SaveToJsonFile()
    {
        string jsonString = SerializeGameData();
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
            CoroutineManager.registeredUser = loadedData.registeredUser;
        }
        else
        {
            Debug.LogError("JsonManager: Failed to load user data. File not found.");
        }

    }

    public void TestSaveGameDataOnServer()
    {
        int slotId = 1;

        // fill in game data

        Gaos.Routes.Model.GameDataJson.UserGameDataSaveRequest userGameDataSaveRequest = new()
        {
            UserId = Gaos.Context.Authentication.GetUserId(),
            SlotId = slotId,
            GameDataJson = SerializeGameData()
        };

        StartCoroutine(Gaos.GameData.UserGameDataSave.Save(slotId, userGameDataSaveRequest, OnUserGameDataSaveComplete));

    }

    public void OnUserGameDataSaveComplete(Gaos.Routes.Model.GameDataJson.UserGameDataSaveResponse response)
    {
        string responseString = JsonConvert.SerializeObject(response);


        //StartCoroutine(Gaos.GameData.UserGameDataGet.Get(1, OnUserGameDataGetComplete));
    }
    public static void OnUserGameDataGetComplete(Gaos.Routes.Model.GameDataJson.UserGameDataGetResponse response)
    {
        if (response.GameDataJson != null)
        {
            SaveDataModel gameData = JsonConvert.DeserializeObject<SaveDataModel>(response.GameDataJson);
            string gameDataStr = JsonConvert.SerializeObject(gameData);
        }
        else
        {
            Debug.Log("Nothing found!");
        }
    }
}
