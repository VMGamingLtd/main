using Cysharp.Threading.Tasks;
using RecipeManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public Animation goalAnimation;
    private RecipeCreator recipeCreator;
    private ButtonManager buttonManager;
    public GameObject[] Goals;
    public static bool firstGoal = false;
    public static bool secondGoal = false;
    public static bool thirdGoal = false;

    void Awake()
    {
        recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
        buttonManager = GameObject.Find("BUTTONMANAGER").GetComponent<ButtonManager>();
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
        await UniTask.Delay(1000); // Delay for 1 second
        recipeCreator.CreateRecipe(1);
        recipeCreator.CreateRecipe(3);
        ChangeGoal("CraftAndUseWater");
        goalAnimation.Play("Idle");
        firstGoal = true;
    }

    public async UniTask SetThirdGoal()
    {
        goalAnimation.Play("Success");
        await UniTask.Delay(1000); // Delay for 1 second
        recipeCreator.CreateRecipe(8);
        recipeCreator.CreateRecipe(9);
        recipeCreator.CreateRecipe(10);
        recipeCreator.CreateRecipe(11);
        ChangeGoal("BuildBase");
        goalAnimation.Play("Idle");
        secondGoal = true;
        buttonManager.UnlockBaseButton();
    }

    public async UniTask SetFourthGoal()
    {
        goalAnimation.Play("Success");
        await UniTask.Delay(1000); // Delay for 1 second

        ChangeGoal("BuildBase");
        goalAnimation.Play("Idle");
        thirdGoal = true;
    }
}
