using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using ItemManagement;
using RecipeManagement;

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
    public EquipmentManager equipmentManager;
    private GlobalCalculator globalCalculator;

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
        recipeCreator.CreatePlantsRecipe();
        recipeCreator.CreateBiofuelRecipe();
        recipeCreator.CreateBatteryCoreRecipe();
        recipeCreator.CreateBatteryRecipe();
        itemCreator.CreateDistilledWater(1);

        //initialize starting resources
        Credits.ResetCredits();
        Credits.AddCredits(42);

        MainUI.SetActive(true);

        equipmentManager.InitStartEquip();

        // force button click action to open the building menu that the button is set for
        buttonToClick.onClick.Invoke();

        // enable first achievement when starting new game
        AchievementManager.EnableAchievement(ref AchievementManager.achievement1, true);

        // with fresh game, all production booleans needs to reset to false
        CoroutineManager.ResetAllCoroutineBooleans();
    }
}

