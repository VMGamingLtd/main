using Cysharp.Threading.Tasks;
using ItemManagement;
using RecipeManagement;
using System;
using UnityEngine;
using UnityEngine.UI;

public class StartGameInputChecker : MonoBehaviour
{
    public ItemCreator itemCreator;
    public InventoryManager inventoryManager;
    public BuildingManager buildingManager;
    public RecipeManager recipeManager;
    public RecipeCreator recipeCreator;
    public GameObject NewGamePopup;
    public GameObject MainUI;
    public GameObject Account;
    public Button buttonToClick;
    public EquipmentManager equipmentManager;
    private GlobalCalculator globalCalculator;
    private MessageManager messageManager;
    private TranslationManager translationManager;
    public BuildingIncrementor buildingIncrementor;
    private Planet planet;
    private SwitchSettingsManager switchSettingsManager;

    // Update is called once per frame
    public void startGameCheckForUsername()
    {
        globalCalculator = GameObject.Find("GlobalCalculator").GetComponent<GlobalCalculator>();
        messageManager = GameObject.Find("MESSAGEMANAGER").GetComponent<MessageManager>();
        planet = GameObject.Find("PlanetParent/StartPlanet").GetComponent<Planet>();
        translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
        switchSettingsManager = GameObject.Find("SWITCHSETTINGSMANAGER").GetComponent<SwitchSettingsManager>();
        globalCalculator.UpdatePlayerConsumption();
        GlobalCalculator.GameStarted = true;
        NewGamePopup.SetActive(false);
        planet.GeneratePlanet();

        inventoryManager.PopulateInventoryArrays();
        buildingManager.PopulateBuildingArrays();
        recipeManager.PopulateInventoryArrays();

        _ = LoadMenus();

        equipmentManager.InitStartEquip();

        //initialize starting resources
        Credits.ResetCredits();
        Credits.AddCredits(42);

        // force button click action to open the Exploration menu that the button is set for
        buttonToClick.onClick.Invoke();

        // enable first achievement when starting new game
        AchievementManager.EnableAchievement(ref AchievementManager.achievement1, true);

        // with fresh game, all production booleans needs to reset to false
        CoroutineManager.ResetAllCoroutineBooleans();
        inventoryManager.CalculateInventorySlots();
        buildingIncrementor.InitializeBuildingCounts();
        InitializeStartSettings();
    }

    private async UniTask LoadMenus()
    {
        MainUI.SetActive(true);
        buttonToClick.onClick.Invoke();
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

        var suitRectTransform = equipmentManager.SuitSlot.GetComponent<RectTransform>();
        var helmetRectTransfrom = equipmentManager.HelmetSlot.GetComponent<RectTransform>();
        var toolRectTransfrom = equipmentManager.LeftHandSlot.GetComponent<RectTransform>();

        itemCreator.CreateSuit(0, null, suitRectTransform);
        itemCreator.CreateHelmet(0, null, helmetRectTransfrom);
        itemCreator.CreateTool(0, null, toolRectTransfrom);

        recipeCreator.CreateRecipe(1, Guid.NewGuid());
        recipeCreator.CreateRecipe(3, Guid.NewGuid());
        recipeCreator.RefreshQueueRecipes();

        mainCanvasGroup.interactable = true;
        accountCanvasGroup.interactable = true;
        GlobalCalculator.GameStarted = true;
        messageManager.CreateEventMessage(translationManager.Translate("FirstEventMessage"));
    }

    private void InitializeStartSettings()
    {
        Player.CavesSwitch = true;
        Player.VolcanicCaveSwitch = true;
        Player.IceCaveSwitch = true;
        Player.HiveNestSwitch = true;
        Player.CyberHideoutSwitch = true;
        Player.MissionsSwitch = true;
        Player.AlienBaseSwitch = true;
        Player.WormTunnelsSwitch = true;
        Player.ShipwreckSwitch = true;
        Player.MysticTempleSwitch = true;
        Player.MonstersSwitch = true;
        Player.XenoSpiderSwitch = true;
        Player.SporeBehemothSwitch = true;
        Player.ElectroBeastSwitch = true;
        Player.VoidReaperSwitch = true;
        Player.ResourcesDiscoverySwitch = true;
        Player.LiquidsDiscoverySwitch = true;
        Player.MineralsDiscoverySwitch = true;
        Player.GasDiscoverySwitch = true;
        Player.FoamsDiscoverySwitch = true;
        Player.MeatDiscoverySwitch = true;
        Player.AnomalySwitch = true;
        Player.MysteryDevicesSwitch = true;

        Player.ItemsCollectionSwitch = false;
        Player.ResourcesCollectionSwitch = false;

        switchSettingsManager.InitializeAllSwitches();
    }
}

