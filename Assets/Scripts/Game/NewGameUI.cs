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
    private GoalManager goalManager;
    private GlobalCalculator globalCalculator;
    private ButtonManager buttonManager;
    private BuildingCreator buildingCreator;
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
    private void CallSaveSlot1Data(UserGameDataGetResponse response)
    {
        recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
        itemCreator = GameObject.Find("ItemCreatorList").GetComponent<ItemCreator>();
        coroutineManager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
        goalManager = GameObject.Find("GOALMANAGER").GetComponent<GoalManager>();
        globalCalculator = GameObject.Find("GlobalCalculator").GetComponent<GlobalCalculator>();
        buttonManager = GameObject.Find("BUTTONMANAGER").GetComponent<ButtonManager>();
        buildingCreator = GameObject.Find("BuildingCreatorList").GetComponent<BuildingCreator>();
        planet = GameObject.Find("PlanetParent/StartPlanet").GetComponent<Planet>();

        if (response != null)
        {
            SaveDataModel gameData = JsonConvert.DeserializeObject<SaveDataModel>(response.GameDataJson);
            Debug.Log(response.GameDataJson);
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
            Level.PlayerLevel = gameData.playerLevel;
            Level.PlayerCurrentExp = gameData.playerCurrentExp;
            Level.PlayerMaxExp = gameData.playerMaxExp;
            Level.SkillPoints = gameData.skillPoints;
            Level.StatPoints = gameData.statPoints;
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
            // deserialize buildings
            for (int i = 0; i < gameData.powerplant.Length; i++)
            {
                EnergyBuildingItemDataModel buildingData = gameData.powerplant[i];
                buildingCreator.RecreateEnergyBuilding(buildingData.index, buildingData.buildingName, buildingData.buildingType, buildingData.buildingClass,
                    buildingData.consumedSlotCount, buildingData.consumedItems, buildingData.totalTime, buildingData.basePowerOutput, buildingData.timer,
                    buildingData.actualPowerOutput, buildingData.powerOutput, buildingData.efficiency, buildingData.efficiencySetting, buildingData.buildingCount,
                    buildingData.isPaused, buildingData.buildingPosition, buildingData.secondCycleCount, buildingData.minuteCycleCount, buildingData.hourCycleCount,
                    buildingData.powerCycleData, buildingData.spriteIconName, buildingData.ID);
            }

            // sort items by ID added
            inventoryManager.SortItemRecipeArraysByOrderAdded();

            if (GoalManager.secondGoal) buttonManager.UnlockBaseButton();

            // finish loading after deserialization
            coroutineManager.InitializeResourceMap();
            coroutineManager.UpdateQuantityTexts("FibrousLeaves", "BASIC");
            coroutineManager.UpdateQuantityTexts("Water", "BASIC");
            coroutineManager.UpdateQuantityTexts("Biofuel", "PROCESSED");
            coroutineManager.UpdateQuantityTexts("DistilledWater", "PROCESSED");
            coroutineManager.UpdateQuantityTexts("Battery", "ASSEMBLED");
            coroutineManager.UpdateQuantityTexts("BatteryCore", "ENHANCED");
            coroutineManager.UpdateQuantityTexts("Wood", "BASIC");
            coroutineManager.UpdateQuantityTexts("IronOre", "BASIC");
            coroutineManager.UpdateQuantityTexts("Coal", "BASIC");
            coroutineManager.UpdateQuantityTexts("IronBeam", "PROCESSED");
            coroutineManager.UpdateBuildingText("BiofuelGenerator", Planet0Buildings.Planet0BiofuelGeneratorBlueprint.ToString());
            coroutineManager.UpdateQuantityTexts("IronSheet", "BASIC");
            coroutineManager.UpdateQuantityTexts("IronRod", "PROCESSED");
            CoroutineManager.ResetAllCoroutineBooleans();
            globalCalculator.UpdatePlayerConsumption();
            buildingIncrementor.InitializeBuildingCounts();
            saveSlots.SetActive(false);
            planet.OnColourSettingsUpdated();
            _ = LoadMenus();
        }
    }
    private async UniTask LoadMenus()
    {
        MainUI.SetActive(true);
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
        StartCoroutine(UserGameDataGet.Get(1, CallSaveSlot1Data));
    }

    public void CheckSlotStatus()
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
}
