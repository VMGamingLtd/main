using RecipeManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Scripts used to start a manual production queue in Production/Planning UI
/// </summary>
public class QueueRecipePlanning : MonoBehaviour
{
    public void AddRecipeToQueue()
    {
        RecipeCreator recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
        string objectName = gameObject.name;
        string recipeQuantityText = gameObject.transform.Find("Quantity").GetComponent<TMP_InputField>().text;

        if (int.TryParse(objectName, out int recipeId) &&
            int.TryParse(recipeQuantityText, out int recipeQuantity) &&
            recipeCreator.queueRecipes.Count < 8)
        {
            recipeCreator.CreateQueueRecipe(recipeId, recipeQuantity);
        }
    }

    public void RemoveRecipeFromQeue()
    {
        RecipeCreator recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
        List<GameObject> recipesToRemove = new();

        string objectName = gameObject.name;

        foreach (var recipe in recipeCreator.queueRecipes)
        {
            if (recipe.name == objectName)
            {
                recipesToRemove.Add(recipe);
            }
        }

        if (int.TryParse(objectName, out int recipeID))
        {
            recipeCreator.DestroyQueueRecipe(recipeID);
        }

        foreach (var item in recipesToRemove)
        {
            recipeCreator.queueRecipes.Remove(item);
        }
    }
}
