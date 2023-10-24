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
        RecipeItemData recipeData = gameObject.GetComponent<RecipeItemData>();
        buildingBar = GameObject.Find("Planet0Production").GetComponent<BuildingBarPlanet0>();
        bool result = buildingBar.StartRecipeCreation(recipeData);
        if (!result)
        {
            StartCoroutine(errorCoroutine());
        }
        /*string functionName = "Create" + gameObject.name;
        // Use reflection to call the function based on the provided function name
        System.Reflection.MethodInfo method = typeof(BuildingBarPlanet0).GetMethod(functionName);
        if (method != null)
        {
            // Invoke the method and get the result (bool)
            bool result = (bool)method.Invoke(buildingBar, null);

            // Check if the result is false, then start errorCoroutine
            if (!result)
            {
                StartCoroutine(errorCoroutine());
            }
        }
        else
        {
            Debug.LogError("Function '" + functionName + "' not found.");
        }*/
    }

    private void OnDisable()
    {
        if (errorMessage.isPlaying)
        {
            Transform errorObjectTransform = transform.Find("ErrorImage");
            if (errorObjectTransform != null)
            {
                errorObjectTransform.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator errorCoroutine()
    {
        errorMessage.Play("ErrorMessage");
        yield return new WaitForSeconds(1.4f);
    }
}
