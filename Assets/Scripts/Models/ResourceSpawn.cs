using RecipeManagement;
using System;
using UnityEngine;

public class ResourceSpawn
{
    private RecipeCreator recipeCreator;

    /// <summary>
    /// Spawns a resource GameObject as 'EventIcon' on the Planet according the available RecipeListJson data.
    /// </summary>
    /// <param name="eventObject"></param>
    /// <param name="resourceName"></param>
    /// <param name="elevation"></param>
    /// <returns></returns>
    public GameObject SpawnResource(GameObject eventObject, string resourceName, EventIconType type, float elevation)
    {
        recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();

        RecipeDataJson recipeData = null;
        var component = eventObject.AddComponent<EventIcon>();

        if (type == EventIconType.Fish)
        {
            recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.recipeName == "FishMeat");
            component.RecipeIndex = recipeData.index;
            component.RecipeProduct = recipeData.recipeProduct;
            component.RecipeGuid = Guid.NewGuid();
        }
        else if (type == EventIconType.Animal)
        {
            recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.recipeName == "AnimalMeat");
            component.RecipeIndex = recipeData.index;
            component.RecipeProduct = recipeData.recipeProduct;
            component.RecipeGuid = Guid.NewGuid();
        }
        else if (type == EventIconType.FurAnimal)
        {
            recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.recipeName == "AnimalMeat");
            component.RecipeIndex = recipeData.index;
            component.RecipeProduct = recipeData.recipeProduct;
            component.RecipeGuid = Guid.NewGuid();
            component.RecipeIndex2 = recipeCreator.recipeDataList.Find(recipe => recipe.recipeName == "AnimalSkin").index;
            component.RecipeProduct2 = recipeCreator.recipeDataList.Find(recipe => recipe.recipeName == "AnimalSkin").recipeProduct;
            component.RecipeGuid2 = Guid.NewGuid();
        }
        else if (type == EventIconType.MilkAnimal)
        {
            recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.recipeName == "AnimalMeat");
            component.RecipeIndex = recipeData.index;
            component.RecipeProduct = recipeData.recipeProduct;
            component.RecipeGuid = Guid.NewGuid();
            component.RecipeIndex2 = recipeCreator.recipeDataList.Find(recipe => recipe.recipeName == "Milk").index;
            component.RecipeProduct2 = recipeCreator.recipeDataList.Find(recipe => recipe.recipeName == "Milk").recipeProduct;
            component.RecipeGuid2 = Guid.NewGuid();
        }
        else
        {
            recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.recipeName == resourceName);
            component.RecipeIndex = recipeData.index;
            component.RecipeProduct = recipeData.recipeProduct;
            component.RecipeGuid = Guid.NewGuid();
        }

        eventObject.name = resourceName;
        component.iconType = type;
        component.MinQuantityRange = recipeData.minQuantityRange;
        component.MaxQuantityRange = recipeData.maxQuantityRange;
        component.Elevation = elevation;

        return eventObject;
    }
}
