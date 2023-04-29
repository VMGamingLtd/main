using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class StartGameInputChecker : MonoBehaviour
{
    public GameObject NewGamePopup;
    public GameObject MainUI;
    public Button buttonToClick;

    public GlobalCalculator globalCalculator;

    // Update is called once per frame
    public void startGameCheckForUsername()
    {
         globalCalculator.GameStarted = true;
         NewGamePopup.SetActive(false);
         AchievementPoints.ResetPoints();

         //initialize starting resources
         Credits.ResetCredits();
         Credits.AddCredits(42);
         CurrentPopulationManager.AddCurrentPopulation(ref CurrentPopulationManager.Planet0CurrentPopulation, 12);
         DecimalResourceManager.AddDecimalResource(ref DecimalResourceManager.Planet0Oxygen, 20);
         DecimalResourceManager .AddDecimalResource(ref DecimalResourceManager.Planet0Water, 15);

         MainUI.SetActive(true);

         // force button click action to open the building menu that the button is set for
         buttonToClick.onClick.Invoke();

         // enable first achievement when starting new game
         AchievementManager.EnableAchievement(ref AchievementManager.achievement1, true);

         // with fresh game, all production booleans needs to reset to false
         CoroutineManager.ResetAllCoroutineBooleans();
    }
}

