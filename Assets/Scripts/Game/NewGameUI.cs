using Assets.Scripts.ItemFactory;
using Cysharp.Threading.Tasks;
using Gaos.GameData;
using Gaos.Routes.Model.GameDataJson;
using ItemManagement;
using Newtonsoft.Json;
using RecipeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class NewGameUI : MonoBehaviour
{
    public int slotNumber;
    public GameObject newGameUI;
    public GameObject saveSlots;
    public BuildingManager buildingManager;
    public InventoryManager inventoryManager;
    public RecipeManager recipeManager;
    public GameObject MainUI;
    public GameObject Account;
    public Button buttonToClick;
    public BuildingIncrementor buildingIncrementor;
    public RectTransform[] equipButtons = new RectTransform[9];
    private TranslationManager translationManager;
    private RecipeCreator recipeCreator;
    private ItemCreator itemCreator;
    private CoroutineManager coroutineManager;
    private EquipmentManager equipmentManager;
    private GoalManager goalManager;
    private GlobalCalculator globalCalculator;
    private ButtonManager buttonManager;
    private BuildingCreator buildingCreator;
    private ProductionCreator productionCreator;
    private Planet planet;


    void Awake()
    {
        translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
    }

    public void StartNewGameUI()
    {
        newGameUI.SetActive(true);
        saveSlots.SetActive(false);
    }

    private void CalGetSlotData(UserGameDataGetResponse response)
    {
        recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
        itemCreator = GameObject.Find("ItemCreatorList").GetComponent<ItemCreator>();
        coroutineManager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
        equipmentManager = GameObject.Find("EquipmentManager").GetComponent<EquipmentManager>();
        goalManager = GameObject.Find("GOALMANAGER").GetComponent<GoalManager>();
        globalCalculator = GameObject.Find("GlobalCalculator").GetComponent<GlobalCalculator>();
        buttonManager = GameObject.Find("BUTTONMANAGER").GetComponent<ButtonManager>();
        buildingCreator = GameObject.Find("BuildingCreatorList").GetComponent<BuildingCreator>();
        productionCreator = GameObject.Find("PRODUCTIONCREATOR").GetComponent<ProductionCreator>();
        planet = GameObject.Find("PlanetParent/StartPlanet").GetComponent<Planet>();

        if (response != null)
        {
            SaveDataModel gameData = JsonConvert.DeserializeObject<SaveDataModel>(response.GameDataJson);
            //Debug.Log(response.GameDataJson);
            UserName.userName = gameData.username;
            Password.password = gameData.password;
            Email.email = gameData.email;
            InventoryManager.ShowItemProducts = gameData.showItemProducts;
            RecipeManager.ShowRecipeProducts = gameData.showRecipeProducts;
            InventoryManager.ShowItemTypes = gameData.showItemTypes;
            RecipeManager.ShowRecipeTypes = gameData.showRecipeTypes;
            InventoryManager.ShowItemClass = gameData.showItemClass;
            RecipeManager.ShowRecipeClass = gameData.showRecipeClass;
            Planet0Name.planet0Name = gameData.planet0Name;
            WindManager.Planet0WindStatus = gameData.planet0WindStatus;
            WeatherManager.planet0UV = gameData.planet0UV;
            WeatherManager.planet0Weather = gameData.planet0Weather;
            GlobalCalculator.hours = gameData.hours;
            GlobalCalculator.minutes = gameData.minutes;
            GlobalCalculator.seconds = gameData.seconds;
            CoroutineManager.registeredUser = gameData.registeredUser;
            GoalManager.firstGoal = gameData.firstGoal;
            GoalManager.secondGoal = gameData.secondGoal;
            GoalManager.thirdGoal = gameData.thirdGoal;
            GoalManager.fourthGoal = gameData.fourthGoal;
            GlobalCalculator.isPlayerInBiologicalBiome = gameData.isPlayerInBiologicalBiome;
            Credits.credits = gameData.credits;
            ButtonManager.MenuButtonTypeOn = gameData.MenuButtonTypeOn;
            BuildingManager.isDraggingBuilding = gameData.isDraggingBuilding;
            BuildingStatisticsManager.BuildingStatisticProcess = gameData.BuildingStatisticProcess;
            BuildingStatisticsManager.BuildingStatisticType = gameData.BuildingStatisticType;
            BuildingStatisticsManager.BuildingStatisticInterval = gameData.BuildingStatisticInterval;
            BuildingIntervalTypes.BuildingIntervalTypeChanged = gameData.BuildingIntervalTypeChanged;
            BuildingStatisticTypes.BuildingStatisticTypeChanged = gameData.BuildingStatisticTypeChanged;
            ItemFactory.ItemCreationID = gameData.ItemCreationID;
            BuildingCreator.BuildingUniqueID = gameData.BuildingUniqueID;
            RecipeCreator.recipeOrderAdded = gameData.RecipeOrderAdded;
            EquipmentManager.autoConsumption = gameData.autoConsumption;
            LoadStaticVariablesFromModel(gameData);

            for (int i = 0; i < gameData.EventObjects.Count; i++)
            {
                EventIconModel iconModel = gameData.EventObjects[i];
                planet.RecreateEventObject(iconModel.Name, iconModel.position);
            }

            // assign current goal
            goalManager.UpdateCurrentGoal();

            // slot equip array
            EquipmentManager.slotEquipped = gameData.slotEquipped;
            EquipmentManager.slotEquippedName = gameData.slotEquippedName;

            // Deserialize Recipes
            for (int i = 0; i < gameData.basicRecipeObjects.Length; i++)
            {
                RecipeItemDataModel recipeData = gameData.basicRecipeObjects[i];
                recipeCreator.RecreateRecipe(recipeData.recipeProduct, recipeData.recipeType, recipeData.itemClass, recipeData.recipeName, recipeData.index,
                    recipeData.experience, recipeData.productionTime, recipeData.outputValue, recipeData.hasRequirements, recipeData.childData, recipeData.orderAdded);
            }
            for (int i = 0; i < gameData.processedRecipeObjects.Length; i++)
            {
                RecipeItemDataModel recipeData = gameData.processedRecipeObjects[i];
                recipeCreator.RecreateRecipe(recipeData.recipeProduct, recipeData.recipeType, recipeData.itemClass, recipeData.recipeName, recipeData.index,
                    recipeData.experience, recipeData.productionTime, recipeData.outputValue, recipeData.hasRequirements, recipeData.childData, recipeData.orderAdded);
            }
            for (int i = 0; i < gameData.enhancedRecipeObjects.Length; i++)
            {
                RecipeItemDataModel recipeData = gameData.enhancedRecipeObjects[i];
                recipeCreator.RecreateRecipe(recipeData.recipeProduct, recipeData.recipeType, recipeData.itemClass, recipeData.recipeName, recipeData.index,
                    recipeData.experience, recipeData.productionTime, recipeData.outputValue, recipeData.hasRequirements, recipeData.childData, recipeData.orderAdded);
            }
            for (int i = 0; i < gameData.assembledRecipeObjects.Length; i++)
            {
                RecipeItemDataModel recipeData = gameData.assembledRecipeObjects[i];
                recipeCreator.RecreateRecipe(recipeData.recipeProduct, recipeData.recipeType, recipeData.itemClass, recipeData.recipeName, recipeData.index,
                    recipeData.experience, recipeData.productionTime, recipeData.outputValue, recipeData.hasRequirements, recipeData.childData, recipeData.orderAdded);
            }
            for (int i = 0; i < gameData.buildingRecipeObjects.Length; i++)
            {
                RecipeItemDataModel recipeData = gameData.buildingRecipeObjects[i];
                recipeCreator.RecreateRecipe(recipeData.recipeProduct, recipeData.recipeType, recipeData.itemClass, recipeData.recipeName, recipeData.index,
                    recipeData.experience, recipeData.productionTime, recipeData.outputValue, recipeData.hasRequirements, recipeData.childData, recipeData.orderAdded);
            }
            // sort recipes by orderAdded
            recipeManager.SortItemRecipeArraysByOrderAdded();


            // Deserialize Items
            for (int i = 0; i < gameData.basicInventoryObjects.Length; i++)
            {
                ItemDataModel itemData = gameData.basicInventoryObjects[i];
                if (itemData.isEquipped)
                {
                    if (itemData.itemType == "ENERGY")
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[5]);
                    }
                    if (itemData.itemType == "LIQUID")
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[7]);
                    }
                    if (itemData.itemType == "PLANTS" || itemData.itemType == "FLESH")
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[8]);
                    }
                }
                else
                {
                    itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                    itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped);
                }
            }
            for (int i = 0; i < gameData.processedInventoryObjects.Length; i++)
            {
                ItemDataModel itemData = gameData.processedInventoryObjects[i];
                if (itemData.isEquipped)
                {
                    if (itemData.itemType == "ENERGY")
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[5]);
                    }
                    if (itemData.itemType == "LIQUID")
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[7]);
                    }
                    if (itemData.itemType == "PLANTS" || itemData.itemType == "FLESH")
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[8]);
                    }
                }
                else
                {
                    itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                    itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped);
                }
            }
            for (int i = 0; i < gameData.enhancedInventoryObjects.Length; i++)
            {
                ItemDataModel itemData = gameData.enhancedInventoryObjects[i];
                if (itemData.isEquipped)
                {
                    if (itemData.itemType == "ENERGY")
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[5]);
                    }
                    if (itemData.itemType == "LIQUID")
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[7]);
                    }
                    if (itemData.itemType == "PLANTS" || itemData.itemType == "FLESH")
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[8]);
                    }
                }
                else
                {
                    itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                    itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped);
                }
            }
            for (int i = 0; i < gameData.assembledInventoryObjects.Length; i++)
            {
                ItemDataModel itemData = gameData.assembledInventoryObjects[i];
                if (itemData != null)
                {
                    if (itemData.isEquipped)
                    {
                        if (itemData.itemType == "ENERGY")
                        {
                            itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                            itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[5]);
                        }
                        if (itemData.itemType == "LIQUID")
                        {
                            itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                            itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[7]);
                        }
                        if (itemData.itemType == "PLANTS" || itemData.itemType == "FLESH")
                        {
                            itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                            itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped, equipButtons[8]);
                        }
                    }
                    else
                    {
                        itemCreator.Recreateitem(itemData.itemQuantity, itemData.itemProduct, itemData.itemType, itemData.itemClass,
                        itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable, itemData.ID, itemData.isEquipped);
                    }
                }
            }
            for (int i = 0; i < gameData.suitInventoryObjects.Length; i++)
            {
                SuitDataModel suitData = gameData.suitInventoryObjects[i];
                if (suitData != null)
                {
                    if (suitData.isEquipped)
                    {
                        itemCreator.RecreateSuit(suitData.itemQuantity, suitData.itemProduct, suitData.itemType, suitData.itemClass, suitData.itemName, suitData.index, suitData.stackLimit,
                            suitData.equipable, suitData.ID, suitData.isEquipped, suitData.physicalProtection, suitData.fireProtection, suitData.coldProtection, suitData.gasProtection,
                            suitData.explosionProtection, suitData.shieldPoints, suitData.hitPoints, suitData.energyCapacity, suitData.durability, suitData.maxDurability, suitData.inventorySlots,
                            suitData.strength, suitData.perception, suitData.intelligence, suitData.agility, suitData.charisma, suitData.willpower, equipButtons[1]);
                    }
                    else
                    {
                        itemCreator.RecreateSuit(suitData.itemQuantity, suitData.itemProduct, suitData.itemType, suitData.itemClass, suitData.itemName, suitData.index, suitData.stackLimit,
                            suitData.equipable, suitData.ID, suitData.isEquipped, suitData.physicalProtection, suitData.fireProtection, suitData.coldProtection, suitData.gasProtection,
                            suitData.explosionProtection, suitData.shieldPoints, suitData.hitPoints, suitData.energyCapacity, suitData.durability, suitData.maxDurability,
                            suitData.inventorySlots, suitData.strength, suitData.perception, suitData.intelligence, suitData.agility, suitData.charisma, suitData.willpower);
                    }
                }
            }
            for (int i = 0; i < gameData.helmetInventoryObjects.Length; i++)
            {
                HelmetDataModel helmetData = gameData.helmetInventoryObjects[i];
                if (helmetData != null)
                {
                    if (helmetData.isEquipped)
                    {
                        itemCreator.RecreateHelmet(helmetData.itemQuantity, helmetData.itemProduct, helmetData.itemType, helmetData.itemClass, helmetData.itemName, helmetData.index, helmetData.stackLimit,
                            helmetData.equipable, helmetData.ID, helmetData.isEquipped, helmetData.physicalProtection, helmetData.fireProtection, helmetData.coldProtection, helmetData.gasProtection,
                            helmetData.explosionProtection, helmetData.shieldPoints, helmetData.hitPoints, helmetData.durability, helmetData.maxDurability, helmetData.strength, helmetData.perception,
                            helmetData.intelligence, helmetData.agility, helmetData.charisma, helmetData.willpower, helmetData.visibilityRadius, helmetData.explorationRadius, helmetData.pickupRadius, equipButtons[0]);
                    }
                    else
                    {
                        itemCreator.RecreateHelmet(helmetData.itemQuantity, helmetData.itemProduct, helmetData.itemType, helmetData.itemClass, helmetData.itemName, helmetData.index, helmetData.stackLimit,
                            helmetData.equipable, helmetData.ID, helmetData.isEquipped, helmetData.physicalProtection, helmetData.fireProtection, helmetData.coldProtection, helmetData.gasProtection,
                            helmetData.explosionProtection, helmetData.shieldPoints, helmetData.hitPoints, helmetData.durability, helmetData.maxDurability, helmetData.strength, helmetData.perception,
                            helmetData.intelligence, helmetData.agility, helmetData.charisma, helmetData.willpower, helmetData.visibilityRadius, helmetData.explorationRadius, helmetData.pickupRadius);
                    }
                }
            }
            for (int i = 0; i < gameData.toolInventoryObjects.Length; i++)
            {
                ToolDataModel toolData = gameData.toolInventoryObjects[i];
                if (toolData != null)
                {
                    if (toolData.isEquipped)
                    {
                        itemCreator.RecreateTool(toolData.itemQuantity, toolData.itemProduct, toolData.itemType, toolData.itemClass, toolData.itemName, toolData.index, toolData.stackLimit,
                            toolData.equipable, toolData.ID, toolData.isEquipped, toolData.durability, toolData.maxDurability, toolData.strength, toolData.perception,
                            toolData.intelligence, toolData.agility, toolData.charisma, toolData.willpower, toolData.productionSpeed, toolData.materialCost, toolData.outcomeRate, equipButtons[2]);
                    }
                    else
                    {
                        itemCreator.RecreateTool(toolData.itemQuantity, toolData.itemProduct, toolData.itemType, toolData.itemClass, toolData.itemName, toolData.index, toolData.stackLimit,
                            toolData.equipable, toolData.ID, toolData.isEquipped, toolData.durability, toolData.maxDurability, toolData.strength, toolData.perception,
                            toolData.intelligence, toolData.agility, toolData.charisma, toolData.willpower, toolData.productionSpeed, toolData.materialCost, toolData.outcomeRate);
                    }
                }
            }

            // deserialize Powerplant buildings
            for (int i = 0; i < gameData.powerplant.Length; i++)
            {
                EnergyBuildingItemDataModel buildingData = gameData.powerplant[i];
                buildingCreator.RecreateEnergyBuilding(buildingData.index, buildingData.buildingName, buildingData.buildingType, buildingData.buildingClass,
                    buildingData.consumedSlotCount, buildingData.consumedItems, buildingData.totalTime, buildingData.basePowerOutput, buildingData.timer,
                    buildingData.actualPowerOutput, buildingData.powerOutput, buildingData.efficiency, buildingData.efficiencySetting, buildingData.buildingCount,
                    buildingData.isPaused, buildingData.buildingPosition, buildingData.secondCycleCount, buildingData.minuteCycleCount, buildingData.hourCycleCount,
                    buildingData.powerCycleData, buildingData.spriteIconName, buildingData.ID, buildingData.enlistedProduction);

                if (buildingData.enlistedProduction)
                    productionCreator.RecreateEnergyBuildingProduction(buildingData, buildingData.ID);
            }

            // deserialize PumpingFacility buildings
            for (int i = 0; i < gameData.pumpingFacility.Length; i++)
            {
                BuildingItemDataModel buildingData = gameData.pumpingFacility[i];
                buildingCreator.RecreateProductionBuilding(buildingData.index, buildingData.buildingName, buildingData.buildingType, buildingData.buildingClass,
                    buildingData.consumedSlotCount, buildingData.producedSlotCount, buildingData.consumedItems, buildingData.producedItems, buildingData.totalTime, buildingData.powerConsumption,
                    buildingData.timer, buildingData.actualPowerConsumption, buildingData.efficiency, buildingData.efficiencySetting, buildingData.buildingCount,
                    buildingData.isPaused, buildingData.buildingPosition, buildingData.secondCycleCount, buildingData.minuteCycleCount, buildingData.hourCycleCount,
                    buildingData.powerConsumptionCycleData, buildingData.productionCycleData, buildingData.spriteIconName, buildingData.ID, buildingData.enlistedProduction);

                if (buildingData.enlistedProduction)
                    productionCreator.RecreateBuildingProduction(buildingData, buildingData.ID);
            }

            // deserialize Agriculture buildings
            for (int i = 0; i < gameData.agriculture.Length; i++)
            {
                BuildingItemDataModel buildingData = gameData.agriculture[i];
                buildingCreator.RecreateProductionBuilding(buildingData.index, buildingData.buildingName, buildingData.buildingType, buildingData.buildingClass,
                    buildingData.consumedSlotCount, buildingData.producedSlotCount, buildingData.consumedItems, buildingData.producedItems, buildingData.totalTime, buildingData.powerConsumption,
                    buildingData.timer, buildingData.actualPowerConsumption, buildingData.efficiency, buildingData.efficiencySetting, buildingData.buildingCount,
                    buildingData.isPaused, buildingData.buildingPosition, buildingData.secondCycleCount, buildingData.minuteCycleCount, buildingData.hourCycleCount,
                    buildingData.powerConsumptionCycleData, buildingData.productionCycleData, buildingData.spriteIconName, buildingData.ID, buildingData.enlistedProduction);

                if (buildingData.enlistedProduction)
                    productionCreator.RecreateBuildingProduction(buildingData, buildingData.ID);
            }

            // deserialize Research buildings
            for (int i = 0; i < gameData.research.Length; i++)
            {
                ResearchBuildingItemDataModel buildingData = gameData.research[i];
                buildingCreator.RecreateResearchBuilding(buildingData.index, buildingData.buildingName, buildingData.buildingType, buildingData.buildingClass,
                    buildingData.consumedSlotCount, buildingData.consumedItems, buildingData.totalTime, buildingData.powerConsumption, buildingData.timer,
                    buildingData.actualPowerConsumption, buildingData.efficiency, buildingData.efficiencySetting, buildingData.buildingCount,
                    buildingData.isPaused, buildingData.buildingPosition, buildingData.powerConsumptionCycleData, buildingData.spriteIconName, buildingData.ID,
                    buildingData.enlistedProduction, buildingData.researchPoints);

                if (buildingData.enlistedProduction)
                    productionCreator.RecreateResearchBuildingProduction(buildingData, buildingData.ID);
            }

            if (Player.CanProduce == false)
            {
                equipmentManager.DisableProduction();
            }
            else { equipmentManager.EnableProduction(); }

            // sort items by ID added
            inventoryManager.SortItemRecipeArraysByOrderAdded();

            if (GoalManager.secondGoal) buttonManager.UnlockBaseButton();
            if (GoalManager.thirdGoal) buttonManager.UnlockResearchButton();

            MainUI.SetActive(true);

            // finish loading after deserialization
            coroutineManager.InitializeResourceMap();
            coroutineManager.InitializeProductionOutcomeMap();
            coroutineManager.InitializeProductionTimeMap();
            coroutineManager.InitializeMaterialCostMap();

            coroutineManager.UpdateQuantityTexts("FibrousLeaves", "BASIC");
            coroutineManager.UpdateQuantityTexts("Water", "BASIC");
            coroutineManager.UpdateQuantityTexts("Biofuel", "ENHANCED");
            coroutineManager.UpdateQuantityTexts("DistilledWater", "PROCESSED");
            coroutineManager.UpdateQuantityTexts("Battery", "ASSEMBLED");
            coroutineManager.UpdateQuantityTexts("BatteryCore", "ENHANCED");
            coroutineManager.UpdateQuantityTexts("Wood", "BASIC");
            coroutineManager.UpdateQuantityTexts("IronOre", "BASIC");
            coroutineManager.UpdateQuantityTexts("Coal", "BASIC");
            coroutineManager.UpdateQuantityTexts("IronBeam", "PROCESSED");
            coroutineManager.UpdateBuildingText("BiofuelGenerator", Planet0Buildings.Planet0BiofuelGeneratorBlueprint.ToString());
            coroutineManager.UpdateQuantityTexts("IronSheet", "BASIC");
            coroutineManager.UpdateQuantityTexts("BiomassLeaves", "PROCESSED");
            coroutineManager.UpdateQuantityTexts("BiomassWood", "PROCESSED");
            coroutineManager.UpdateQuantityTexts("LatexFoam", "BASIC");
            coroutineManager.UpdateQuantityTexts("ProteinBeans", "BASIC");
            coroutineManager.UpdateQuantityTexts("ProteinPowder", "PROCESSED");
            coroutineManager.UpdateQuantityTexts("BioOil", "PROCESSED");
            coroutineManager.UpdateQuantityTexts("IronTube", "PROCESSED");
            coroutineManager.UpdateBuildingText("BiofuelGenerator", Planet0Buildings.Planet0WaterPumpBlueprint.ToString());
            coroutineManager.UpdateBuildingText("FibrousPlantField", Planet0Buildings.Planet0FibrousPlantFieldBlueprint.ToString());

            CoroutineManager.ResetAllCoroutineBooleans();
            globalCalculator.UpdatePlayerConsumption();
            buildingIncrementor.InitializeAvailableBuildings();
            buildingIncrementor.InitializeBuildingCounts();
            saveSlots.SetActive(false);
            inventoryManager.CalculateInventorySlots();
            equipmentManager.RefreshStats();
            equipmentManager.RefreshRecipeStats();
            planet.OnColourSettingsUpdated();
            _ = LoadMenus();
        }
    }
    private async UniTask LoadMenus()
    {
        await UniTask.DelayFrame(10);
        buttonToClick.onClick.Invoke();
        await UniTask.DelayFrame(50);
        CanvasGroup mainCanvasGroup = MainUI.GetComponent<CanvasGroup>();
        CanvasGroup accountCanvasGroup = Account.GetComponent<CanvasGroup>();
        float totalTime = 0.5f;
        float currentTime = 0f;
        float currentAlpha = 0f;
        float targetAlpha = 1f;



        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / totalTime;
            mainCanvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, t);
            accountCanvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, t);
            await UniTask.Yield();
        }
        mainCanvasGroup.interactable = true;
        accountCanvasGroup.interactable = true;
        GlobalCalculator.GameStarted = true;


    }

    public void LoadSlotGame()
    {
        inventoryManager.PopulateInventoryArrays();
        buildingManager.PopulateBuildingArrays();
        recipeManager.PopulateInventoryArrays();
        StartCoroutine(UserGameDataGet.Get(slotNumber, CalGetSlotData));
    }

    private bool IsNewSlot(Gaos.Routes.Model.DeviceJson.DeviceRegisterResponseUserSlot slot) {
        if (slot.Hours == 0 && slot.Minutes == 0 && slot.Seconds == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BootGameInSlot()
    {
        if (true)
        {
            CoroutineManager.CurrentGameSlotId = slotNumber;
            SaveManager.CurrentGameSlotId = slotNumber;

            // Search in gaos context if there's an active user slot with same slotNumber
            var userSlots = Gaos.Context.Authentication.GetUserSlots();
            foreach (var slot in userSlots)
            {
                if (slot.SlotId == slotNumber)
                {
                    if (IsNewSlot(slot) == true)
                    {
                        // If there's a user slot with same slotNumber and game has never been saved on the slot then create a new game on the slot, whatever players date before first game data save will be lost
                        StartNewGameUI();
                        return;
                    }
                    else
                    {
                        // If there's a user slot with same slotNumber and there was at least one game data save from the game on the slot then boot existing game, new players game data after last save will be lost
                        LoadSlotGame();
                        return;
                    }
                }
            }

            // Ensure new slot exists in backend
            StartCoroutine(Gaos.GameData.EnsureNewSlot.EnsureNewSlotExists(slotNumber, OnEnsureNewSlotComplete));

            // If there's no acive user slot with same slotNumber, then start new game on the slot
            //StartNewGameUI();
        }
        /*
        else
        {
            string slotName = transform.Find("Info/Name").GetComponent<TextMeshProUGUI>().text;
            CoreTranslations matchedEntry = translationManager.FindEntryBySubstring(slotName);
            if (matchedEntry != null)
            {
                StartNewGameUI();
            }
            else
            {
                LoadSlotGame();
            }
        }
        */
    }

    public void OnEnsureNewSlotComplete(EnsureNewSlotResponse response)
    {
        if (response != null)
        {
            CoroutineManager.CurrentGameSlotId = slotNumber;
            StartNewGameUI();
        }
    }
}
