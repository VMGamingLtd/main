using RecipeManagement;
using System.Collections;
using UnityEngine;

public class FunctionCaller : MonoBehaviour
{
    private Animation errorMessage;
    private BuildingBarPlanet0 buildingBar; // Reference to the BuildingBarPlanet0 script

    void Awake()
    {
        errorMessage = GetComponent<Animation>();
    }
    public void CallFunctionByName()
    {
        if (!CoroutineManager.autoProduction)
        {
            RecipeItemData recipeData = gameObject.GetComponent<RecipeItemData>();
            buildingBar = GameObject.Find("Planet0Production").GetComponent<BuildingBarPlanet0>();
            bool result = buildingBar.StartRecipeCreation(recipeData);
            if (!result)
            {
                StartCoroutine(ErrorCoroutine());
            }
        }
    }

    private void OnDisable()
    {
        if (errorMessage.isPlaying)
        {
            Transform errorObjectTransform = transform.Find("ErrorImage");
            if (errorObjectTransform != null)
            {
                errorObjectTransform?.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator ErrorCoroutine()
    {
        errorMessage.Play("ErrorMessage");
        yield return new WaitForSeconds(1.4f);
    }
}
