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
    public Button productionTabClick;
    public EquipmentManager equipmentManager;
    private GlobalCalculator globalCalculator;
    public BuildingIncrementor buildingIncrementor;

    // Update is called once per frame
    public void startGameCheckForUsername()
    {
        globalCalculator = GameObject.Find("GlobalCalculator").GetComponent<GlobalCalculator>();
        globalCalculator.UpdatePlayerConsumption();
        GlobalCalculator.GameStarted = true;
        NewGamePopup.SetActive(false);

        inventoryManager.PopulateInventoryArrays();
        buildingManager.PopulateBuildingArrays();
        recipeManager.PopulateInventoryArrays();
        recipeCreator.CreateRecipe(0);
        recipeCreator.CreateRecipe(2);
        recipeCreator.CreateRecipe(6);
        recipeCreator.CreateRecipe(4);

        //initialize starting resources
        Credits.ResetCredits();
        Credits.AddCredits(42);

        _ = LoadMenus();

        // test purposes
        //itemCreator.CreateItem(3);
        //itemCreator.CreateItem(6);
        //itemCreator.CreateItem(2, 20);

        equipmentManager.InitStartEquip();

        // force button click action to open the Production menu that the button is set for
        buttonToClick.onClick.Invoke();

        // after the Production menu is open we also want to display the Overview tab as the first one
        productionTabClick.onClick.Invoke();

        // enable first achievement when starting new game
        AchievementManager.EnableAchievement(ref AchievementManager.achievement1, true);

        // with fresh game, all production booleans needs to reset to false
        CoroutineManager.ResetAllCoroutineBooleans();

        buildingIncrementor.InitializeBuildingCounts();
    }

    private async UniTask LoadMenus()
    {
        MainUI.SetActive(true);
        buttonToClick.onClick.Invoke();
        productionTabClick.onClick.Invoke();
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

