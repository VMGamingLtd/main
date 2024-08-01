using ItemManagement;
using RecipeManagement;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A local service of FightManager.cs that handles the loot 
/// </summary>
public class LootService
{
    private readonly FightManager _fightManager;
    private ItemCreator _itemCreator;
    private Transform _lootField;
    private RecipeCreator _recipeCreator;

    public LootService(FightManager fightManager, ItemCreator itemCreator, Transform lootField)
    {
        _fightManager = fightManager != null ? fightManager : throw new ArgumentNullException(nameof(fightManager));
        _itemCreator = itemCreator != null ? itemCreator : throw new ArgumentNullException(nameof(itemCreator));
        _lootField = lootField != null ? lootField : throw new ArgumentNullException(nameof(lootField));
        _recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
    }

    public void CreateRandomLoot()
    {
        // give player 12 iron, 10 coal
        _itemCreator.CreateItem(8, 12);
        CreateLootItem(8, 12);
        _itemCreator.CreateItem(10, 10);
        CreateLootItem(10, 10);
    }

    public void ClearLoot()
    {
        foreach (Transform child in _lootField)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    private void CreateLootItem(int index, float quantity)
    {
        RecipeDataJson recipeData = _recipeCreator.recipeDataList.Find(recipe => recipe.index == index);
        GameObject newItem = UnityEngine.Object.Instantiate(_itemCreator.itemTemplate, _lootField);

        if (_fightManager.GetDungeonName() == Constants.VolcanicCave)
        {
            newItem.name = recipeData.recipeName;
            newItem.transform.Find("ChildName").name = recipeData.recipeName;
            newItem.transform.Find("CountInventory").GetComponent<TextMeshProUGUI>().text = quantity.ToString();
            newItem.transform.Find("Image").GetComponent<Image>().sprite = AssetBundleManager.AssignResourceSpriteToSlot(recipeData.recipeName);

            ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();
            newItemData.quantity = quantity;
            newItemData.itemProduct = recipeData.recipeProduct;
            newItemData.itemType = recipeData.recipeType;
            newItemData.itemClass = recipeData.itemClass;
            newItemData.index = index;
            newItemData.itemName = recipeData.recipeName;
        }
    }
}
