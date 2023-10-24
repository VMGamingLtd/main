using RecipeManagement;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBarPlanet0 : MonoBehaviour
{


    public CoroutineManager coroutineManager;
    public InventoryManager inventoryManager;

    public bool StartRecipeCreation(RecipeItemData recipeData)
    {
        bool allItemsMet = true;
        RefreshResourceMap();
        if (recipeData.childData.Count > 0)
        {
            List<ChildData> recipeDataChildList = recipeData.childData;
            for (int i = 0; i < recipeDataChildList.Count; i++)
            {
                bool isQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[i].name, recipeDataChildList[i].product, recipeDataChildList[i].quantity);
                if (!isQuantityMet)
                {
                    allItemsMet = false;
                    break;
                }
            }
        }
        if (CoroutineManager.AllCoroutineBooleans[recipeData.index] == true)
        {
            coroutineManager.StartCoroutine("StopCreateRecipe", recipeData);
            return true;
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (allItemsMet)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[recipeData.index] = true;
                coroutineManager.StartCoroutine("CreateRecipe", recipeData);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (allItemsMet)
        {
            CoroutineManager.AllCoroutineBooleans[recipeData.index] = true;
            coroutineManager.StartCoroutine("CreateRecipe", recipeData);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RefreshResourceMap()
    {
        coroutineManager.InitializeResourceMap();
    }

}
