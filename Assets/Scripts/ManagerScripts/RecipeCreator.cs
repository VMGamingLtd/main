using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RecipeManagement
{
    public class RecipeItemData : MonoBehaviour
        {
            public string itemProduct;
            public string itemType;
            public string itemClass;
        }
    public class RecipeCreator : MonoBehaviour
    {
        public GameObject[] recipePrefabs;
        public RecipeManager recipeManager;
        public void CreatePlantsRecipe()
        {
            CreateRecipe(recipePrefabs[0], "BASIC", "PLANTS", "CLASS-F", recipePrefabs[0].name);
        }
        public void CreateWaterRecipe()
        {
            CreateRecipe(recipePrefabs[1], "BASIC", "LIQUID", "CLASS-F", recipePrefabs[1].name);
        }
        public void CreateBiofuelRecipe()
        {
            CreateRecipe(recipePrefabs[2], "PROCESSED", "LIQUID", "CLASS-F", recipePrefabs[2].name);
        }
        public void CreateDistilledWaterRecipe()
        {
            CreateRecipe(recipePrefabs[3], "PROCESSED", "LIQUID", "CLASS-F", recipePrefabs[3].name);
        }
        public void CreateBatteryRecipe()
        {
            CreateRecipe(recipePrefabs[4], "ASSEMBLED", "ENERGY", "CLASS-F", recipePrefabs[4].name);
        }
        public void CreateOxygenTanksRecipe()
        {
            CreateRecipe(recipePrefabs[5], "ASSEMBLED", "OXYGEN", "CLASS-F", recipePrefabs[5].name);
        }
        public void CreateBatteryCoreRecipe()
        {
            CreateRecipe(recipePrefabs[6], "ENHANCED", "ENERGY", "CLASS-F", recipePrefabs[6].name);
        }
        public void CreateWoodRecipe()
        {
            CreateRecipe(recipePrefabs[7], "BASIC", "PLANTS", "CLASS-F", recipePrefabs[7].name);
        }
        public void CreateIronOreRecipe()
        {
            CreateRecipe(recipePrefabs[8], "BASIC", "MINERALS", "CLASS-F", recipePrefabs[8].name);
        }
        public void CreateCoalRecipe()
        {
            CreateRecipe(recipePrefabs[9], "BASIC", "MINERALS", "CLASS-F", recipePrefabs[9].name);
        }
        public void CreateIronBeamRecipe()
        {
            CreateRecipe(recipePrefabs[10], "PROCESSED", "METALS", "CLASS-F", recipePrefabs[10].name);
        }
        public void CreateBiofuelGeneratorBlueprint()
        {
            CreateRecipe(recipePrefabs[11], "BUILDINGS", "POWERPLANT", null, recipePrefabs[11].name);
        }

        private void CreateRecipe(GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName)
        {
            // Create the item once and set the initial quantity
            GameObject newItem = Instantiate(prefab);

            // Get or add the ItemData component to the new item
            RecipeItemData newItemData = newItem.GetComponent<RecipeItemData>();
            if (newItemData == null)
            {
                newItemData = newItem.AddComponent<RecipeItemData>();
            }
            newItemData.itemProduct = itemProduct;
            newItemData.itemType = itemType;
            newItemData.itemClass = itemClass;
            recipeManager.AddToItemArray(itemProduct, newItem);

            newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);
            newItem.transform.localScale = Vector3.one;
        }
    }
}
