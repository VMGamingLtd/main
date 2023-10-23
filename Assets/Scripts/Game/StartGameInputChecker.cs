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

        MainUI.SetActive(true);

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
}

