using Assets.Scripts.ItemFactory;
using BuildingManagement;
using ItemManagement;
using Newtonsoft.Json;
using RecipeManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using static Enumerations;

public class SaveManager : MonoBehaviour
{

    public static int CurrentGameSlotId;

    private const string FileName = "Savegame.json";
    private string filePath;
    public InventoryManager inventoryManager;
    public RecipeManager recipeManager;
    public RecipeCreator recipeCreator;
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
        public string planet0WindStatus;
        public string MenuButtonTypeOn;
        public string inventoryTitle;
        public string recipeTitle;
        public string BuildingStatisticProcess;
        public string BuildingStatisticType;
        public string BuildingStatisticInterval;
        public string[] slotEquippedName;
        public int planet0UV;
        public int hours;
        public int minutes;
        public int seconds;
        public int ItemCreationID;
        public int BuildingUniqueID;
        public int RecipeOrderAdded;
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
        public bool BuildingStatisticTypeChanged;
        public bool BuildingIntervalTypeChanged;
        public bool[] slotEquipped;
        public float credits;
        public float playerMovementSpeed;
        public float AchievementPoints;
        public ItemDataJson[] basicInventoryObjects;
        public ItemDataJson[] processedInventoryObjects;
        public ItemDataJson[] enhancedInventoryObjects;
        public ItemDataJson[] assembledInventoryObjects;
        public SuitDataJson[] suitInventoryObjects;
        public HelmetDataJson[] helmetInventoryObjects;
        public ToolDataJson[] toolInventoryObjects;
        public MeleeWeaponDataJson[] meleeWeaponInventoryObjects;
        public RangedWeaponDataJson[] rangedWeaponInventoryObjects;
        public ShieldDataJson[] shieldInventoryObjects;
        public OffHandDataJson[] offHandInventoryObjects;
        public RecipeItemDataModel[] basicRecipeObjects;
        public RecipeItemDataModel[] processedRecipeObjects;
        public RecipeItemDataModel[] enhancedRecipeObjects;
        public RecipeItemDataModel[] assembledRecipeObjects;
        public RecipeItemDataModel[] buildingRecipeObjects;
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
        public ResearchBuildingItemDataModel[] laboratory;
        public Dictionary<string, object> Planet0StaticVariables = new();
        public Dictionary<string, object> PlayerStaticVariables = new();
        public Dictionary<string, object> PlayerResourcesStaticVariables = new();
        public List<EventIconModel> EventObjects = new();
        public List<QueueRecipeModel> QueueRecipes = new();
        public List<QueueRecipeModel> QueueInputRecipes = new();
    }

    [Serializable]
    public class EventIconModel
    {
        public Guid recipeGuid;
        public Guid recipeGuid2;
        public Guid recipeGuid3;
        public Guid recipeGuid4;
        public string Name;
        public Vector3 position;
        public EventIconType iconType;
        public EventSize eventSize;
        public float currentQuantity;
        public float elevation;
        public float minQuantityRange;
        public float maxQuantityRange;
        public int recipeIndex;
        public int recipeIndex2;
        public int recipeIndex3;
        public int recipeIndex4;
        public int eventLevel;
        public string recipeProduct;
        public string recipeProduct2;
        public string recipeProduct3;
        public string recipeProduct4;
    }

    [Serializable]
    public class RecipeItemDataModel
    {
        public Guid guid;
        public int index;
        public int orderAdded;
        public string recipeName;
        public string recipeProduct;
        public string recipeType;
        public string itemClass;
        public int experience;
        public float productionTime;
        public float outputValue;
        public float currentQuantity;
        public bool hasRequirements;
        public List<ChildData> childData;
    }

    public class BasicBuildingModel
    {
        public int index;
        public int ID;
        public string spriteIconName;
        public string buildingType;
        public string buildingName;
        public string buildingClass;
        public float timer;
        public float totalTime;
        public int efficiency;
        public int efficiencySetting;
        public int buildingCount;
        public bool isPaused;
        public bool enlistedProduction;
        public int consumedSlotCount;
        public int producedSlotCount;
    }

    [Serializable]
    public class QueueRecipeModel
    {
        public int index;
        public int quantity;
        public int output;
    }

    [Serializable]
    public class EnergyBuildingItemDataModel : BasicBuildingModel
    {
        public Vector3 buildingPosition;
        public List<SlotItemData> consumedItems;
        public PowerCycle powerCycleData = new();
        public int secondCycleCount;
        public int minuteCycleCount;
        public int hourCycleCount;
        public int powerOutput;
        public int basePowerOutput;
        public int actualPowerOutput;
    }
    [Serializable]
    public class BuildingItemDataModel : BasicBuildingModel
    {
        public Vector3 buildingPosition;
        public List<SlotItemData> consumedItems;
        public List<SlotItemData> producedItems;
        public PowerConsumptionCycle powerConsumptionCycleData = new();
        public ProductionCycle productionCycleData = new();
        public int secondCycleCount;
        public int minuteCycleCount;
        public int hourCycleCount;
        public int powerConsumption;
        public int actualPowerConsumption;
    }

    [Serializable]
    public class ResearchBuildingItemDataModel : BasicBuildingModel
    {
        public Vector3 buildingPosition;
        public List<SlotItemData> consumedItems;
        public PowerConsumptionCycle powerConsumptionCycleData = new();
        public int powerConsumption;
        public int actualPowerConsumption;
        public float researchPoints;
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
        //currentSaveData.username = UserName.userName;
        currentSaveData.username = Gaos.Context.Authentication.GetUserName();
        //currentSaveData.password = Password.password;
        currentSaveData.password = "xxxxxx";
        //currentSaveData.email = Email.email;
        currentSaveData.email = "xxxxx@xxxxx.xxxs";
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

        currentSaveData.QueueRecipes.Clear();

        // recipe creator - queue recipes
        foreach (var queueRecipe in recipeCreator.queueRecipes)
        {
            if (int.TryParse(queueRecipe.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text, out int quantity) &&
                int.TryParse(queueRecipe.name, out int index))
            {
                QueueRecipeModel queueRecipeModel = new()
                {
                    index = index,
                    quantity = quantity,
                };

                currentSaveData.QueueRecipes.Add(queueRecipeModel);
            }
        }

        currentSaveData.QueueInputRecipes.Clear();

        // recipe creator queue input recipes
        foreach (var queueInputRecipe in recipeCreator.queueInputRecipes)
        {
            if (int.TryParse(queueInputRecipe.transform.Find("Quantity").GetComponent<TMP_InputField>().text, out int quantity) &&
                int.TryParse(queueInputRecipe.transform.Find("Output").GetComponent<TextMeshProUGUI>().text, out int output) &&
                int.TryParse(queueInputRecipe.name, out int index))
            {
                QueueRecipeModel queueRecipeInputModel = new()
                {
                    index = index,
                    quantity = quantity,
                    output = output
                };

                currentSaveData.QueueInputRecipes.Add(queueRecipeInputModel);
            }
        }

        // slot equip array
        currentSaveData.slotEquipped = EquipmentManager.slotEquipped;
        currentSaveData.slotEquippedName = EquipmentManager.slotEquippedName;

        // store all Planet event objects with their coordinates
        currentSaveData.EventObjects.Clear();
        Planet planet = GameObject.Find("PlanetParent/StartPlanet").GetComponent<Planet>();
        foreach (GameObject eventObject in planet.eventObjects)
        {
            eventObject.TryGetComponent(out EventIcon component);

            if (component != null)
            {
                EventIconModel itemData = new()
                {
                    Name = eventObject.name,
                    position = eventObject.transform.localPosition,
                    iconType = component.IconType,
                    eventSize = component.EventSize,
                    eventLevel = component.EventLevel,
                    currentQuantity = component.CurrentQuantity,
                    minQuantityRange = component.MinQuantityRange,
                    maxQuantityRange = component.MaxQuantityRange,
                    recipeGuid = component.RecipeGuid,
                    recipeGuid2 = component.RecipeGuid2,
                    recipeGuid3 = component.RecipeGuid3,
                    recipeGuid4 = component.RecipeGuid4,
                    recipeIndex = component.RecipeIndex,
                    recipeIndex2 = component.RecipeIndex2,
                    recipeIndex3 = component.RecipeIndex3,
                    recipeIndex4 = component.RecipeIndex4,
                    recipeProduct = component.RecipeProduct,
                    recipeProduct2 = component.RecipeProduct2,
                    recipeProduct3 = component.RecipeProduct3,
                    recipeProduct4 = component.RecipeProduct4,
                    elevation = component.Elevation,
                };

                currentSaveData.EventObjects.Add(itemData);
            }

        }

        // Access the itemArrays dictionary through the inventoryManager reference
        Dictionary<string, GameObject[]> itemArrays = inventoryManager.itemArrays;

        currentSaveData.basicInventoryObjects = new ItemDataJson[itemArrays["BASIC"].Length];

        for (int i = 0; i < itemArrays["BASIC"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["BASIC"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemDataComponent.itemName = itemDataComponent.itemName.Replace("(Clone)", "");
            ItemDataJson itemData = new()
            {
                ID = itemDataComponent.ID,
                index = itemDataComponent.index,
                stackLimit = itemDataComponent.stackLimit,
                quantity = itemDataComponent.quantity,
                itemProduct = itemDataComponent.itemProduct,
                itemType = itemDataComponent.itemType,
                itemClass = itemDataComponent.itemClass,
                itemName = itemDataComponent.itemName,
                equipable = itemDataComponent.equipable,
                isEquipped = itemDataComponent.isEquipped
            };

            currentSaveData.basicInventoryObjects[i] = itemData;
        }

        currentSaveData.processedInventoryObjects = new ItemDataJson[itemArrays["PROCESSED"].Length];

        for (int i = 0; i < itemArrays["PROCESSED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["PROCESSED"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemDataComponent.itemName = itemDataComponent.itemName.Replace("(Clone)", "");
            ItemDataJson itemData = new()
            {
                ID = itemDataComponent.ID,
                index = itemDataComponent.index,
                stackLimit = itemDataComponent.stackLimit,
                quantity = itemDataComponent.quantity,
                itemProduct = itemDataComponent.itemProduct,
                itemType = itemDataComponent.itemType,
                itemClass = itemDataComponent.itemClass,
                itemName = itemDataComponent.itemName,
                equipable = itemDataComponent.equipable,
                isEquipped = itemDataComponent.isEquipped
            };

            currentSaveData.processedInventoryObjects[i] = itemData;
        }
        currentSaveData.enhancedInventoryObjects = new ItemDataJson[itemArrays["ENHANCED"].Length];

        for (int i = 0; i < itemArrays["ENHANCED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["ENHANCED"][i];
            ItemData itemDataComponent = itemGameObject.GetComponent<ItemData>();
            itemDataComponent.itemName = itemDataComponent.itemName.Replace("(Clone)", "");
            ItemDataJson itemData = new()
            {
                ID = itemDataComponent.ID,
                index = itemDataComponent.index,
                stackLimit = itemDataComponent.stackLimit,
                quantity = itemDataComponent.quantity,
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
        currentSaveData.assembledInventoryObjects = new ItemDataJson[itemArrays["ASSEMBLED"].Length];
        currentSaveData.suitInventoryObjects = new SuitDataJson[itemArrays["ASSEMBLED"].Length];
        currentSaveData.helmetInventoryObjects = new HelmetDataJson[itemArrays["ASSEMBLED"].Length];
        currentSaveData.toolInventoryObjects = new ToolDataJson[itemArrays["ASSEMBLED"].Length];
        currentSaveData.meleeWeaponInventoryObjects = new MeleeWeaponDataJson[itemArrays["ASSEMBLED"].Length];
        currentSaveData.rangedWeaponInventoryObjects = new RangedWeaponDataJson[itemArrays["ASSEMBLED"].Length];
        currentSaveData.shieldInventoryObjects = new ShieldDataJson[itemArrays["ASSEMBLED"].Length];
        currentSaveData.offHandInventoryObjects = new OffHandDataJson[itemArrays["ASSEMBLED"].Length];

        for (int i = 0; i < itemArrays["ASSEMBLED"].Length; i++)
        {
            GameObject itemGameObject = itemArrays["ASSEMBLED"][i];
            if (itemGameObject.TryGetComponent(out ItemData itemDataComponent))
            {
                itemDataComponent.itemName = itemDataComponent.itemName.Replace("(Clone)", "");
                if (itemDataComponent.itemType == "SUIT")
                {
                    SuitData suitDataComponent = itemGameObject.GetComponent<SuitData>();
                    SuitDataJson suitData = new()
                    {
                        ID = suitDataComponent.ID,
                        index = suitDataComponent.index,
                        stackLimit = suitDataComponent.stackLimit,
                        quantity = suitDataComponent.quantity,
                        itemProduct = suitDataComponent.itemProduct,
                        itemType = suitDataComponent.itemType,
                        itemClass = suitDataComponent.itemClass,
                        itemName = suitDataComponent.itemName,
                        equipable = suitDataComponent.equipable,
                        isEquipped = suitDataComponent.isEquipped,
                        physicalProtection = suitDataComponent.physicalProtection,
                        fireProtection = suitDataComponent.fireProtection,
                        coldProtection = suitDataComponent.coldProtection,
                        poisonProtection = suitDataComponent.poisonProtection,
                        energyProtection = suitDataComponent.energyProtection,
                        psiProtection = suitDataComponent.psiProtection,
                        shieldPoints = suitDataComponent.shieldPoints,
                        armor = suitDataComponent.armor,
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
                    HelmetDataJson helmetData = new()
                    {
                        ID = helmetDataComponent.ID,
                        index = helmetDataComponent.index,
                        stackLimit = helmetDataComponent.stackLimit,
                        quantity = helmetDataComponent.quantity,
                        itemProduct = helmetDataComponent.itemProduct,
                        itemType = helmetDataComponent.itemType,
                        itemClass = helmetDataComponent.itemClass,
                        itemName = helmetDataComponent.itemName,
                        equipable = helmetDataComponent.equipable,
                        isEquipped = helmetDataComponent.isEquipped,
                        physicalProtection = helmetDataComponent.physicalProtection,
                        fireProtection = helmetDataComponent.fireProtection,
                        coldProtection = helmetDataComponent.coldProtection,
                        poisonProtection = helmetDataComponent.poisonProtection,
                        energyProtection = helmetDataComponent.energyProtection,
                        psiProtection = helmetDataComponent.psiProtection,
                        shieldPoints = helmetDataComponent.shieldPoints,
                        armor = helmetDataComponent.armor,
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
                    ToolDataJson toolData = new()
                    {
                        ID = toolDataComponent.ID,
                        index = toolDataComponent.index,
                        stackLimit = toolDataComponent.stackLimit,
                        quantity = toolDataComponent.quantity,
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
                else if (itemDataComponent.itemType == "MELEEWEAPON")
                {
                    MeleeWeaponData meleeWeaponDataComponent = itemGameObject.GetComponent<MeleeWeaponData>();
                    MeleeWeaponDataJson meleeWeaponData = new()
                    {
                        ID = meleeWeaponDataComponent.ID,
                        index = meleeWeaponDataComponent.index,
                        stackLimit = meleeWeaponDataComponent.stackLimit,
                        quantity = meleeWeaponDataComponent.quantity,
                        itemProduct = meleeWeaponDataComponent.itemProduct,
                        itemType = meleeWeaponDataComponent.itemType,
                        itemClass = meleeWeaponDataComponent.itemClass,
                        itemName = meleeWeaponDataComponent.itemName,
                        equipable = meleeWeaponDataComponent.equipable,
                        isEquipped = meleeWeaponDataComponent.isEquipped,
                        durability = meleeWeaponDataComponent.durability,
                        maxDurability = meleeWeaponDataComponent.maxDurability,
                        strength = meleeWeaponDataComponent.strength,
                        perception = meleeWeaponDataComponent.perception,
                        intelligence = meleeWeaponDataComponent.intelligence,
                        agility = meleeWeaponDataComponent.agility,
                        charisma = meleeWeaponDataComponent.charisma,
                        willpower = meleeWeaponDataComponent.willpower,
                        meleePhysicalDamage = meleeWeaponDataComponent.meleePhysicalDamage,
                        meleeFireDamage = meleeWeaponDataComponent.meleeFireDamage,
                        meleeColdDamage = meleeWeaponDataComponent.meleeColdDamage,
                        meleePoisonDamage = meleeWeaponDataComponent.meleePoisonDamage,
                        meleeEnergyDamage = meleeWeaponDataComponent.meleeEnergyDamage,
                        psiDamage = meleeWeaponDataComponent.psiDamage,
                        attackSpeed = meleeWeaponDataComponent.attackSpeed,
                        hitChance = meleeWeaponDataComponent.hitChance,
                        dodge = meleeWeaponDataComponent.dodge,
                        resistance = meleeWeaponDataComponent.resistance,
                        counterChance = meleeWeaponDataComponent.counterChance,
                        penetration = meleeWeaponDataComponent.penetration,
                        weaponType = meleeWeaponDataComponent.weaponType,
                    };
                    currentSaveData.meleeWeaponInventoryObjects[i] = meleeWeaponData;
                }
                else if (itemDataComponent.itemType == "RANGEDWEAPON")
                {
                    RangedWeaponData rangedWeaponDataComponent = itemGameObject.GetComponent<RangedWeaponData>();
                    RangedWeaponDataJson rangedWeaponData = new()
                    {
                        ID = rangedWeaponDataComponent.ID,
                        index = rangedWeaponDataComponent.index,
                        stackLimit = rangedWeaponDataComponent.stackLimit,
                        quantity = rangedWeaponDataComponent.quantity,
                        itemProduct = rangedWeaponDataComponent.itemProduct,
                        itemType = rangedWeaponDataComponent.itemType,
                        itemClass = rangedWeaponDataComponent.itemClass,
                        itemName = rangedWeaponDataComponent.itemName,
                        equipable = rangedWeaponDataComponent.equipable,
                        isEquipped = rangedWeaponDataComponent.isEquipped,
                        durability = rangedWeaponDataComponent.durability,
                        maxDurability = rangedWeaponDataComponent.maxDurability,
                        strength = rangedWeaponDataComponent.strength,
                        perception = rangedWeaponDataComponent.perception,
                        intelligence = rangedWeaponDataComponent.intelligence,
                        agility = rangedWeaponDataComponent.agility,
                        charisma = rangedWeaponDataComponent.charisma,
                        willpower = rangedWeaponDataComponent.willpower,
                        rangedPhysicalDamage = rangedWeaponDataComponent.rangedPhysicalDamage,
                        rangedFireDamage = rangedWeaponDataComponent.rangedFireDamage,
                        rangedColdDamage = rangedWeaponDataComponent.rangedColdDamage,
                        rangedPoisonDamage = rangedWeaponDataComponent.rangedPoisonDamage,
                        rangedEnergyDamage = rangedWeaponDataComponent.rangedEnergyDamage,
                        psiDamage = rangedWeaponDataComponent.psiDamage,
                        attackSpeed = rangedWeaponDataComponent.attackSpeed,
                        hitChance = rangedWeaponDataComponent.hitChance,
                        dodge = rangedWeaponDataComponent.dodge,
                        resistance = rangedWeaponDataComponent.resistance,
                        counterChance = rangedWeaponDataComponent.counterChance,
                        penetration = rangedWeaponDataComponent.penetration,
                        weaponType = rangedWeaponDataComponent.weaponType,
                    };
                    currentSaveData.rangedWeaponInventoryObjects[i] = rangedWeaponData;
                }
                else if (itemDataComponent.itemType == "SHIELD")
                {
                    ShieldData shieldDataComponent = itemGameObject.GetComponent<ShieldData>();
                    ShieldDataJson shieldData = new()
                    {
                        ID = shieldDataComponent.ID,
                        index = shieldDataComponent.index,
                        stackLimit = shieldDataComponent.stackLimit,
                        quantity = shieldDataComponent.quantity,
                        itemProduct = shieldDataComponent.itemProduct,
                        itemType = shieldDataComponent.itemType,
                        itemClass = shieldDataComponent.itemClass,
                        itemName = shieldDataComponent.itemName,
                        equipable = shieldDataComponent.equipable,
                        isEquipped = shieldDataComponent.isEquipped,
                        durability = shieldDataComponent.durability,
                        maxDurability = shieldDataComponent.maxDurability,
                        strength = shieldDataComponent.strength,
                        perception = shieldDataComponent.perception,
                        intelligence = shieldDataComponent.intelligence,
                        agility = shieldDataComponent.agility,
                        charisma = shieldDataComponent.charisma,
                        willpower = shieldDataComponent.willpower,
                        shieldPoints = shieldDataComponent.shieldPoints,
                        armor = shieldDataComponent.armor,
                        hitPoints = shieldDataComponent.hitPoints,
                        meleePhysicalDamage = shieldDataComponent.meleePhysicalDamage,
                        meleeFireDamage = shieldDataComponent.meleeFireDamage,
                        meleeColdDamage = shieldDataComponent.meleeColdDamage,
                        meleePoisonDamage = shieldDataComponent.meleePoisonDamage,
                        meleeEnergyDamage = shieldDataComponent.meleeEnergyDamage,
                        psiDamage = shieldDataComponent.psiDamage,
                        physicalProtection = shieldDataComponent.physicalProtection,
                        fireProtection = shieldDataComponent.fireProtection,
                        coldProtection = shieldDataComponent.coldProtection,
                        poisonProtection = shieldDataComponent.poisonProtection,
                        energyProtection = shieldDataComponent.energyProtection,
                        psiProtection = shieldDataComponent.psiProtection,
                        attackSpeed = shieldDataComponent.attackSpeed,
                        hitChance = shieldDataComponent.hitChance,
                        dodge = shieldDataComponent.dodge,
                        resistance = shieldDataComponent.resistance,
                        counterChance = shieldDataComponent.counterChance,
                        penetration = shieldDataComponent.penetration,
                        weaponType = shieldDataComponent.weaponType,
                    };
                    currentSaveData.shieldInventoryObjects[i] = shieldData;
                }
                else if (itemDataComponent.itemType == "OFFHAND")
                {
                    OffHandData offHandDataComponent = itemGameObject.GetComponent<OffHandData>();
                    OffHandDataJson offHandData = new()
                    {
                        ID = offHandDataComponent.ID,
                        index = offHandDataComponent.index,
                        stackLimit = offHandDataComponent.stackLimit,
                        quantity = offHandDataComponent.quantity,
                        itemProduct = offHandDataComponent.itemProduct,
                        itemType = offHandDataComponent.itemType,
                        itemClass = offHandDataComponent.itemClass,
                        itemName = offHandDataComponent.itemName,
                        equipable = offHandDataComponent.equipable,
                        isEquipped = offHandDataComponent.isEquipped,
                        durability = offHandDataComponent.durability,
                        maxDurability = offHandDataComponent.maxDurability,
                        strength = offHandDataComponent.strength,
                        perception = offHandDataComponent.perception,
                        intelligence = offHandDataComponent.intelligence,
                        agility = offHandDataComponent.agility,
                        charisma = offHandDataComponent.charisma,
                        willpower = offHandDataComponent.willpower,
                        shieldPoints = offHandDataComponent.shieldPoints,
                        armor = offHandDataComponent.armor,
                        hitPoints = offHandDataComponent.hitPoints,
                        meleePhysicalDamage = offHandDataComponent.meleePhysicalDamage,
                        meleeFireDamage = offHandDataComponent.meleeFireDamage,
                        meleeColdDamage = offHandDataComponent.meleeColdDamage,
                        meleePoisonDamage = offHandDataComponent.meleePoisonDamage,
                        meleeEnergyDamage = offHandDataComponent.meleeEnergyDamage,
                        rangedPhysicalDamage = offHandDataComponent.rangedPhysicalDamage,
                        rangedFireDamage = offHandDataComponent.rangedFireDamage,
                        rangedColdDamage = offHandDataComponent.rangedColdDamage,
                        rangedPoisonDamage = offHandDataComponent.rangedPoisonDamage,
                        rangedEnergyDamage = offHandDataComponent.rangedEnergyDamage,
                        psiDamage = offHandDataComponent.psiDamage,
                        physicalProtection = offHandDataComponent.physicalProtection,
                        fireProtection = offHandDataComponent.fireProtection,
                        coldProtection = offHandDataComponent.coldProtection,
                        poisonProtection = offHandDataComponent.poisonProtection,
                        energyProtection = offHandDataComponent.energyProtection,
                        psiProtection = offHandDataComponent.psiProtection,
                        attackSpeed = offHandDataComponent.attackSpeed,
                        hitChance = offHandDataComponent.hitChance,
                        dodge = offHandDataComponent.dodge,
                        resistance = offHandDataComponent.resistance,
                        counterChance = offHandDataComponent.counterChance,
                        penetration = offHandDataComponent.penetration,
                        weaponType = offHandDataComponent.weaponType,
                    };
                    currentSaveData.meleeWeaponInventoryObjects[i] = offHandData;
                }
                else
                {
                    ItemDataJson itemData = new()
                    {
                        ID = itemDataComponent.ID,
                        index = itemDataComponent.index,
                        stackLimit = itemDataComponent.stackLimit,
                        quantity = itemDataComponent.quantity,
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
                guid = itemDataComponent.guid,
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                currentQuantity = itemDataComponent.currentQuantity,
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
                guid = itemDataComponent.guid,
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                currentQuantity = itemDataComponent.currentQuantity,
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
                guid = itemDataComponent.guid,
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                currentQuantity = itemDataComponent.currentQuantity,
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
                guid = itemDataComponent.guid,
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                currentQuantity = itemDataComponent.currentQuantity,
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
                guid = itemDataComponent.guid,
                index = itemDataComponent.index,
                orderAdded = itemDataComponent.orderAdded,
                recipeName = itemDataComponent.recipeName,
                recipeProduct = itemDataComponent.recipeProduct,
                recipeType = itemDataComponent.recipeType,
                itemClass = itemDataComponent.itemClass,
                experience = itemDataComponent.experience,
                productionTime = itemDataComponent.productionTime,
                outputValue = itemDataComponent.outputValue,
                currentQuantity = itemDataComponent.currentQuantity,
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

        currentSaveData.laboratory = new ResearchBuildingItemDataModel[buildingArrays["LABORATORY"].Length];

        for (int i = 0; i < buildingArrays["LABORATORY"].Length; i++)
        {
            GameObject itemGameObject = buildingArrays["LABORATORY"][i];
            ResearchBuildingItemData itemDataComponent = itemGameObject.GetComponent<ResearchBuildingItemData>();
            itemDataComponent.buildingName = itemDataComponent.buildingName.Replace("(Clone)", "");
            ResearchBuildingItemDataModel buildingData = new()
            {
                index = itemDataComponent.index,
                ID = itemDataComponent.ID,
                spriteIconName = itemDataComponent.spriteIconName,
                buildingType = itemDataComponent.buildingType,
                buildingName = itemDataComponent.buildingName,
                buildingClass = itemDataComponent.buildingClass,
                buildingPosition = itemDataComponent.buildingPosition,
                consumedItems = itemDataComponent.consumedItems,
                powerConsumptionCycleData = itemDataComponent.powerConsumptionCycleData,
                consumedSlotCount = itemDataComponent.consumedSlotCount,
                timer = itemDataComponent.timer,
                totalTime = itemDataComponent.totalTime,
                powerConsumption = itemDataComponent.powerConsumption,
                actualPowerConsumption = itemDataComponent.actualPowerConsumption,
                efficiency = itemDataComponent.efficiency,
                efficiencySetting = itemDataComponent.efficiencySetting,
                buildingCount = itemDataComponent.buildingCount,
                isPaused = itemDataComponent.isPaused,
                enlistedProduction = itemDataComponent.enlistedProduction,
                researchPoints = itemDataComponent.researchPoints,
            };

            currentSaveData.laboratory[i] = buildingData;
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
        int slotId = CurrentGameSlotId;

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
