using RecipeManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventIconObject
{
    public string RecipeProduct;
    public GameObject GameObject;
    public Guid Guid;

    public EventIconObject(string recipeProduct, GameObject gameObject, Guid guid)
    {
        RecipeProduct = recipeProduct;
        GameObject = gameObject;
        Guid = guid;
    }
}


public class PlayerRangeCollider : MonoBehaviour
{
    private RecipeCreator recipeCreator;

    void Start()
    {
        recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
    }
    void OnTriggerEnter(Collider other)
    {
        _ = StartCoroutine(nameof(AddRecipe), other);
    }

    private IEnumerator AddRecipe(Collider other)
    {
        yield return new WaitForSeconds(0.5f);

        if (other.CompareTag("EventIcon"))
        {
            var eventIcon = other.gameObject.GetComponent<EventIcon>();
            eventIcon.PlayerDistance = true;

            if (eventIcon.RecipeIndex > -1)
            {
                if (!CheckIfRecipeExists(eventIcon))
                    recipeCreator.CreateRecipe(eventIcon.RecipeIndex, eventIcon.RecipeGuid, eventIcon.CurrentQuantity);
            }

            if (eventIcon.RecipeIndex2 > -1)
            {
                if (!CheckIfRecipeExists(eventIcon))
                    recipeCreator.CreateRecipe(eventIcon.RecipeIndex2, eventIcon.RecipeGuid2, eventIcon.CurrentQuantity);
            }

            if (eventIcon.RecipeIndex3 > -1)
            {
                if (!CheckIfRecipeExists(eventIcon))
                    recipeCreator.CreateRecipe(eventIcon.RecipeIndex3, eventIcon.RecipeGuid3, eventIcon.CurrentQuantity);
            }

            if (eventIcon.RecipeIndex4 > -1)
            {
                if (!CheckIfRecipeExists(eventIcon))
                    recipeCreator.CreateRecipe(eventIcon.RecipeIndex4, eventIcon.RecipeGuid4, eventIcon.CurrentQuantity);
            }
        }
    }

    /// <summary>
    /// If player moves out of this collider reach, then the event icon representing the resource will remove it from production menu. 
    /// If there are multiple resources of the same type, the recipe will stay in production menu but will have decreased current quantity.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EventIcon"))
        {
            var eventIcon = other.gameObject.GetComponent<EventIcon>();
            eventIcon.PlayerDistance = false;

            List<EventIconObject> objectsToRemove = new();

            if (eventIcon.RecipeIndex > -1)
            {
                foreach (var item in recipeCreator.recipeManager.itemRecipeArrays.Values.Cast<GameObject[]>())
                {
                    foreach (var itemObject in item)
                    {
                        RecipeItemData recipeData = itemObject.GetComponent<RecipeItemData>();
                        if (recipeData.guid == eventIcon.RecipeGuid)
                        {
                            objectsToRemove.Add(new EventIconObject(eventIcon.RecipeProduct, itemObject, eventIcon.RecipeGuid));
                        }
                    }
                }
            }

            if (eventIcon.RecipeIndex2 > -1)
            {
                foreach (var item in recipeCreator.recipeManager.itemRecipeArrays.Values.Cast<GameObject[]>())
                {
                    foreach (var itemObject in item)
                    {
                        RecipeItemData recipeData = itemObject.GetComponent<RecipeItemData>();
                        if (recipeData.guid == eventIcon.RecipeGuid2)
                        {
                            objectsToRemove.Add(new EventIconObject(eventIcon.RecipeProduct2, itemObject, eventIcon.RecipeGuid2));
                        }
                    }
                }
            }

            if (eventIcon.RecipeIndex3 > -1)
            {
                foreach (var item in recipeCreator.recipeManager.itemRecipeArrays.Values.Cast<GameObject[]>())
                {
                    foreach (var itemObject in item)
                    {
                        RecipeItemData recipeData = itemObject.GetComponent<RecipeItemData>();
                        if (recipeData.guid == eventIcon.RecipeGuid3)
                        {
                            objectsToRemove.Add(new EventIconObject(eventIcon.RecipeProduct3, itemObject, eventIcon.RecipeGuid3));
                        }
                    }
                }
            }

            if (eventIcon.RecipeIndex4 > -1)
            {
                foreach (var item in recipeCreator.recipeManager.itemRecipeArrays.Values.Cast<GameObject[]>())
                {
                    foreach (var itemObject in item)
                    {
                        RecipeItemData recipeData = itemObject.GetComponent<RecipeItemData>();
                        if (recipeData.guid == eventIcon.RecipeGuid4)
                        {
                            objectsToRemove.Add(new EventIconObject(eventIcon.RecipeProduct4, itemObject, eventIcon.RecipeGuid4));
                        }
                    }
                }
            }
            List<Transform> transformsToRemove = new();

            if (objectsToRemove.Count > 0)
            {
                foreach (var item in objectsToRemove)
                {
                    recipeCreator.recipeManager.RemoveFromItemArray(item.RecipeProduct, item.GameObject);

                    foreach (Transform childTransform in recipeCreator.recipeList)
                    {
                        if (childTransform.TryGetComponent(out RecipeItemData recipeData))
                        {
                            if (recipeData.guid == item.Guid)
                            {
                                transformsToRemove.Add(childTransform);
                            }
                        }
                    }
                }
            }

            if (transformsToRemove.Count > 0)
            {
                foreach (var transform in transformsToRemove)
                {
                    Destroy(transform.gameObject);
                }
            }
        }
    }

    private bool CheckIfRecipeExists(EventIcon eventIcon)
    {
        foreach (var item in recipeCreator.recipeManager.itemRecipeArrays.Values.Cast<GameObject[]>())
        {
            foreach (var itemObject in item)
            {
                RecipeItemData recipeData = itemObject.GetComponent<RecipeItemData>();
                if (recipeData.guid == eventIcon.RecipeGuid)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
