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
    public RecipeManager recipeManager;
    public RecipeCreator recipeCreator;
    public GameObject NewGamePopup;
    public GameObject MainUI;
    public Button buttonToClick;
    public EquipmentManager equipmentManager;

    // Update is called once per frame
    public void startGameCheckForUsername()
    {
        GlobalCalculator.GameStarted = true;
        NewGamePopup.SetActive(false);

        inventoryManager.PopulateInventoryArrays();
        recipeManager.PopulateInventoryArrays();
        recipeCreator.CreatePlantsRecipe();
        recipeCreator.CreateBiofuelRecipe();
        recipeCreator.CreateBatteryCoreRecipe();
        recipeCreator.CreateBatteryRecipe();
        itemCreator.CreateBattery(1);

        //initialize starting resources
        Credits.ResetCredits();
        Credits.AddCredits(42);
        PlayerResources.AddCurrentResourceTime(ref PlayerResources.PlayerWater, 0, 0, 5, 40);
        PlayerResources.AddCurrentResourceTime(ref PlayerResources.PlayerHunger, 0, 0, 12, 35);
        PlayerResources.AddCurrentResourceTime(ref PlayerResources.PlayerEnergy, 0, 0, 0, 0);

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

