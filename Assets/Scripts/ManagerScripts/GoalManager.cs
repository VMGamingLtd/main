using Cysharp.Threading.Tasks;
using RecipeManagement;
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
            ChangeGoal("AutomateBattery");
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
        recipeCreator.CreateRecipe(0);
        recipeCreator.CreateRecipe(17);
        recipeCreator.CreateRecipe(15);
        recipeCreator.CreateRecipe(20);
        recipeCreator.CreateRecipe(2);
        recipeCreator.CreateRecipe(6);
        recipeCreator.CreateRecipe(4);
        ChangeGoal("CraftBattery");
        goalAnimation.Play("Idle");
        firstGoal = true;
        coroutineManager.InitializeResourceMap();
    }

    public async UniTask SetThirdGoal()
    {
        goalAnimation.Play("Success");
        await UniTask.Delay(1000);
        recipeCreator.CreateRecipe(8);
        recipeCreator.CreateRecipe(9);
        recipeCreator.CreateRecipe(10);
        recipeCreator.CreateRecipe(11);
        recipeCreator.CreateRecipe(12);
        recipeCreator.CreateRecipe(13);
        recipeCreator.CreateRecipe(14);
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
        buildingIncrementor.InitializeAvailableBuildings();
        recipeCreator.CreateRecipe(21);
        recipeCreator.CreateRecipe(22);
        recipeCreator.CreateRecipe(23);
        ChangeGoal("AutomateBattery");
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
