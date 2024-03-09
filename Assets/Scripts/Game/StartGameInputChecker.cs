using Cysharp.Threading.Tasks;
using ItemManagement;
using RecipeManagement;
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
    public BuildingIncrementor buildingIncrementor;
    private Planet planet;

    // Update is called once per frame
    public void startGameCheckForUsername()
    {
        Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 2500: startGameCheckForUsername");
        globalCalculator = GameObject.Find("GlobalCalculator").GetComponent<GlobalCalculator>();
        planet = GameObject.Find("PlanetParent/StartPlanet").GetComponent<Planet>();
        globalCalculator.UpdatePlayerConsumption();
        GlobalCalculator.GameStarted = true;
        NewGamePopup.SetActive(false);
        planet.GeneratePlanet();

        inventoryManager.PopulateInventoryArrays();
        buildingManager.PopulateBuildingArrays();
        recipeManager.PopulateInventoryArrays();

        var suitRectTransform = equipmentManager.SuitSlot.GetComponent<RectTransform>();
        var helmetRectTransfrom = equipmentManager.HelmetSlot.GetComponent<RectTransform>();
        var toolRectTransfrom = equipmentManager.LeftHandSlot.GetComponent<RectTransform>();

        itemCreator.CreateSuit(0, null, suitRectTransform);
        itemCreator.CreateHelmet(0, null, helmetRectTransfrom);
        itemCreator.CreateTool(0, null, toolRectTransfrom);

        _ = LoadMenus();

        equipmentManager.InitStartEquip();
        recipeCreator.CreateRecipe(1);
        recipeCreator.CreateRecipe(3);

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

        mainCanvasGroup.interactable = true;
        accountCanvasGroup.interactable = true;
        GlobalCalculator.GameStarted = true;

    }
}

