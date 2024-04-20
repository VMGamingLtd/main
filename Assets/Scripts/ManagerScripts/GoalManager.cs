using Cysharp.Threading.Tasks;
using RecipeManagement;
using System;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public Animation goalAnimation;
    private RecipeCreator recipeCreator;
    private ButtonManager buttonManager;
    private CoroutineManager coroutineManager;
    public BuildingIncrementor buildingIncrementor;
    public GameObject[] Goals;
    public static bool firstGoal = false;
    public static bool secondGoal = false;
    public static bool thirdGoal = false;
    public static bool fourthGoal = false;

    void Awake()
    {
        recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
        buttonManager = GameObject.Find("BUTTONMANAGER").GetComponent<ButtonManager>();
        coroutineManager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
    }

    public void UpdateCurrentGoal()
    {
        if (fourthGoal)
        {
            ChangeGoal("AutomateBattery");
        }
        else if (thirdGoal)
        {
            ChangeGoal("ResearchScienceProjects");
        }
        else if (secondGoal)
        {
            ChangeGoal("BuildBase");
        }
        else if (firstGoal)
        {
            ChangeGoal("CraftBattery");
        }
        else
        {
            ChangeGoal("CraftAndUseWater");
        }
    }
    public void ChangeGoal(string GoalName)
    {
        for (int i = 0; i < Goals.Length; i++)
        {
            Goals[i].SetActive(Goals[i].name == GoalName);
        }
    }
    public async UniTask SetSecondGoal()
    {
        goalAnimation.Play("Success");
        await UniTask.Delay(1000);
        ChangeGoal("CraftBattery");
        recipeCreator.CreateRecipe(2, Guid.NewGuid());
        recipeCreator.CreateRecipe(15, Guid.NewGuid());
        recipeCreator.CreateRecipe(17, Guid.NewGuid());
        recipeCreator.CreateRecipe(20, Guid.NewGuid());
        recipeCreator.CreateRecipe(6, Guid.NewGuid());
        recipeCreator.CreateRecipe(4, Guid.NewGuid());
        goalAnimation.Play("Idle");
        firstGoal = true;
        coroutineManager.InitializeResourceMap();
    }

    public async UniTask SetThirdGoal()
    {
        goalAnimation.Play("Success");
        await UniTask.Delay(1000);
        ChangeGoal("BuildBase");
        goalAnimation.Play("Idle");
        secondGoal = true;
        buttonManager.UnlockBaseButton();
        coroutineManager.InitializeResourceMap();
    }

    public async UniTask SetFourthGoal()
    {
        goalAnimation.Play("Success");
        await UniTask.Delay(1000);
        Planet0Buildings.FibrousPlantFieldUnlocked = true;
        Planet0Buildings.WaterPumpUnlocked = true;
        Planet0Buildings.ResearchDeviceUnlocked = true;
        buttonManager.UnlockResearchButton();
        buildingIncrementor.InitializeAvailableBuildings();
        ChangeGoal("ResearchScienceProjects");
        goalAnimation.Play("Idle");
        thirdGoal = true;
        coroutineManager.InitializeResourceMap();
    }

    public async UniTask SetFifthGoal()
    {
        goalAnimation.Play("Success");
        await UniTask.Delay(1000);
        Planet0Buildings.BoilerUnlocked = true;
        Planet0Buildings.SteamGeneratorUnlocked = true;
        Planet0Buildings.FurnaceUnlocked = true;
        buildingIncrementor.InitializeAvailableBuildings();
        ChangeGoal("AutomateBattery");
        goalAnimation.Play("Idle");
        fourthGoal = true;
        coroutineManager.InitializeResourceMap();
    }
}
