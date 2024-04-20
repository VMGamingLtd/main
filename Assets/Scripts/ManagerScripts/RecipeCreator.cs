using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RecipeManagement
{
    [Serializable]
    public class RecipeDataJson
    {
        public int index;
        public string recipeName;
        public string recipeProduct;
        public string recipeType;
        public string itemClass;
        public int experience;
        public float minQuantityRange;
        public float maxQuantityRange;
        public float productionTime;
        public float outputValue;
        public float currentQuantity;
        public bool hasRequirements;
        public List<ChildData> childDataList;
    }

    [Serializable]
    public class ResearchDataJson
    {
        public int index;
        public string projectName;
        public string projectProduct;
        public string projectType;
        public string projectClass;
        public int experience;
        public float researchTime;
        public bool hasRequirements;
        public List<ChildData> requirementsList;
        public List<ChildData> rewardsList;
    }

    [Serializable]
    public class ChildData
    {
        public int index;
        public string name;
        public string product;
        public string type;
        public string itemClass;
        public float quantity;
    }
    public class RecipeItemData : MonoBehaviour
    {
        public Guid guid;
        public int index;
        public int orderAdded;
        public string recipeName;
        public string recipeProduct;
        public string recipeType;
        public string itemClass;
        public int experience;
        public float productionTime;
        public float outputValue;
        public float currentQuantity;
        public bool hasRequirements;
        public List<ChildData> childData;
    }
    public class RecipeCreator : MonoBehaviour
    {
        public GameObject recipeTemplate;
        public RecipeManager recipeManager;
        public Transform recipeList;
        public List<RecipeDataJson> recipeDataList;
        public List<ResearchDataJson> researchDataList;
        private TranslationManager translationManager;
        public static int recipeOrderAdded;

        [Serializable]
        private class JsonArray
        {
            public List<RecipeDataJson> recipes;
        }
        private class JsonArray2
        {
            public List<ResearchDataJson> projects;
        }
        private void Awake()
        {
            string jsonText = Assets.Scripts.Models.RecipeListJson.json;
            JsonArray jsonArray = JsonUtility.FromJson<JsonArray>(jsonText);
            if (jsonArray != null)
            {
                recipeDataList = jsonArray.recipes;
            }

            string jsonText2 = Assets.Scripts.Models.ResearchListJson.json;
            JsonArray2 jsonArray2 = JsonUtility.FromJson<JsonArray2>(jsonText2);
            if (jsonArray2 != null)
            {
                researchDataList = jsonArray2.projects;
            }
        }
        public void RecreateRecipe(string recipeProduct, Guid guid, string recipeType, string itemClass, string recipeName, int index, int experience,
            float productionTime, float outputValue, bool hasRequirements, List<ChildData> childDataList, int orderAdded, float currentQuantity)
        {
            RecreateRecipe(recipeTemplate, guid, recipeProduct, recipeType, itemClass, recipeName, index,
                experience, productionTime, outputValue, hasRequirements, childDataList, orderAdded, currentQuantity);
        }
        public void CreateRecipe(int recipeIndex, Guid guid, float? quantity = null)
        {
            var itemData = recipeDataList[recipeIndex];

            if (quantity != null)
            {
                itemData.currentQuantity = (float)quantity;
            }
            else
            {
                itemData.currentQuantity = -1;
            }

            CreateRecipe(recipeTemplate, guid, itemData.recipeProduct, itemData.recipeType, itemData.itemClass, itemData.recipeName, itemData.index,
                itemData.experience, itemData.productionTime, itemData.outputValue, itemData.hasRequirements, itemData.childDataList, itemData.currentQuantity);
        }
        private void RecreateRecipe(GameObject template, Guid guid, string recipeProduct, string recipeType, string itemClass, string recipeName, int index,
            int experience, float productionTime, float outputValue, bool hasRequirements, List<ChildData> childDataList, int orderAdded, float currentQuantity)
        {
            // RecipeTemplate attribute injection
            GameObject newItem = Instantiate(template);
            newItem.name = recipeName;
            newItem.transform.Find("RecipeNameHolder").name = recipeName;
            translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
            newItem.transform.Find("Content/Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeName);
            Transform CountHeader = newItem.transform.Find("Content/Header/CountHeader/CountValue");
            CountHeader.name = recipeName;
            CountHeader.GetComponent<AddToTextArrayAdaptive>().AssignTextToCoroutineManagerArray();

            Transform TimeValue = newItem.transform.Find("Content/Cost/TimeValue");
            TimeValue.GetComponent<AddToProductionTimeTextArray>().AssignTextToCoroutineManagerArray(recipeName);

            float newProductionTime = productionTime / Player.ProductionSpeed;
            if (Player.ProductionSpeed <= 0) newProductionTime = 0;
            float newOutputValue = outputValue * Player.OutcomeRate;
            if (Player.OutcomeRate <= 0) newOutputValue = 0;

            newItem.transform.Find("Content/Cost/EXP").GetComponent<TextMeshProUGUI>().text = experience.ToString() + "XP";
            newItem.transform.Find("Content/Cost/TimeValue").GetComponent<TextMeshProUGUI>().text = newProductionTime.ToString() + "s";
            newItem.transform.Find("Content/Image/CountValue").GetComponent<TextMeshProUGUI>().text = newOutputValue.ToString();
            newItem.transform.Find("Content/Image/CountValue").GetComponent<AddToProductionOutcomeTextArray>().AssignTextToCoroutineManagerArray(recipeName);
            newItem.transform.Find("Content/Image/Image").GetComponent<Image>().sprite = AssignSpriteToSlot(recipeName, recipeProduct);

            // ProductCost is a child with additional material display objects that is only available if the item has some material requirements
            GameObject productCost = newItem.transform.Find("ProductCost").gameObject;

            if (!hasRequirements)
            {
                productCost.SetActive(false);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    string childName = "Product" + (i + 1);
                    Transform childTransform = productCost.transform.Find(childName);
                    if (childTransform != null)
                    {
                        GameObject childObject = childTransform.gameObject;
                        ChildData child = (i < childDataList.Count) ? childDataList[i] : null;

                        if (child != null)
                        {
                            childObject.transform.Find("Image").GetComponent<Image>().sprite = AssignChildSlot(child.name);
                            float newChildQuantity = child.quantity / Player.MaterialCost;
                            if (newChildQuantity <= 0) newChildQuantity = 0;
                            childObject.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = newChildQuantity.ToString();
                            childObject.transform.Find("Quantity").GetComponent<AddToMaterialCostTextArray>().AssignTextToCoroutineManagerArray(child.name);
                            childObject.transform.Find("ChildName").name = child.name;
                            childObject.transform.Find(child.name).GetComponent<AddToTextArrayAdaptive>().AssignTextToCoroutineManagerArray();
                        }
                        else
                        {
                            childObject.SetActive(false);
                        }
                    }
                }
            }
            // Initialize RecipeItemData component that will hold all recipe data througout the game
            RecipeItemData newRecipeData = newItem.GetComponent<RecipeItemData>() ?? newItem.AddComponent<RecipeItemData>();
            newRecipeData.guid = guid;
            newRecipeData.recipeName = recipeName;
            newRecipeData.recipeProduct = recipeProduct;
            newRecipeData.recipeType = recipeType;
            newRecipeData.itemClass = itemClass;
            newRecipeData.index = index;
            newRecipeData.experience = experience;
            newRecipeData.productionTime = productionTime;
            newRecipeData.outputValue = outputValue;
            newRecipeData.currentQuantity = currentQuantity;
            newRecipeData.hasRequirements = hasRequirements;
            newRecipeData.childData = childDataList;
            newRecipeData.orderAdded = orderAdded;
            //recipeManager.itemRecipeArrays[recipeProduct].Add(newItem);
            recipeManager.AddToItemArray(recipeProduct, newItem);

            newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);
            newItem.transform.localScale = Vector3.one;
        }
        private void CreateRecipe(GameObject template, Guid guid, string recipeProduct, string recipeType, string itemClass, string recipeName,
            int index, int experience, float productionTime, float outputValue, bool hasRequirements, List<ChildData> childDataList, float currentQuantity)
        {
            // RecipeTemplate attribute injection
            GameObject newItem = Instantiate(template);
            newItem.name = recipeName;
            newItem.transform.Find("RecipeNameHolder").name = recipeName;
            translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
            newItem.transform.Find("Content/Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeName);
            Transform CountHeader = newItem.transform.Find("Content/Header/CountHeader/CountValue");
            CountHeader.name = recipeName;
            CountHeader.GetComponent<AddToTextArrayAdaptive>().AssignTextToCoroutineManagerArray();

            Transform TimeValue = newItem.transform.Find("Content/Cost/TimeValue");
            TimeValue.GetComponent<AddToProductionTimeTextArray>().AssignTextToCoroutineManagerArray(recipeName);

            float newProductionTime = productionTime / Player.ProductionSpeed;
            if (Player.ProductionSpeed <= 0) newProductionTime = 0;
            float newOutputValue = outputValue * Player.OutcomeRate;
            if (Player.OutcomeRate <= 0) newOutputValue = 0;

            newItem.transform.Find("Content/Cost/EXP").GetComponent<TextMeshProUGUI>().text = experience.ToString() + "XP";
            newItem.transform.Find("Content/Cost/TimeValue").GetComponent<TextMeshProUGUI>().text = newProductionTime.ToString() + "s";
            newItem.transform.Find("Content/Image/CountValue").GetComponent<TextMeshProUGUI>().text = newOutputValue.ToString();
            newItem.transform.Find("Content/Image/CountValue").GetComponent<AddToProductionOutcomeTextArray>().AssignTextToCoroutineManagerArray(recipeName);
            newItem.transform.Find("Content/Image/Image").GetComponent<Image>().sprite = AssignSpriteToSlot(recipeName, recipeProduct);

            // ProductCost is a child with additional material display objects that is only available if the item has some material requirements
            GameObject productCost = newItem.transform.Find("ProductCost").gameObject;

            if (!hasRequirements)
            {
                productCost.SetActive(false);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    string childName = "Product" + (i + 1);
                    Transform childTransform = productCost.transform.Find(childName);
                    if (childTransform != null)
                    {
                        GameObject childObject = childTransform.gameObject;
                        ChildData child = (i < childDataList.Count) ? childDataList[i] : null;

                        if (child != null)
                        {
                            childObject.transform.Find("Image").GetComponent<Image>().sprite = AssignChildSlot(child.name);
                            float newChildQuantity = child.quantity / Player.MaterialCost;
                            if (newChildQuantity <= 0) newChildQuantity = 0;
                            childObject.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = newChildQuantity.ToString();
                            childObject.transform.Find("Quantity").GetComponent<AddToMaterialCostTextArray>().AssignTextToCoroutineManagerArray(child.name);
                            childObject.transform.Find("ChildName").name = child.name;
                            childObject.transform.Find(child.name).GetComponent<AddToTextArrayAdaptive>().AssignTextToCoroutineManagerArray();

                        }
                        else
                        {
                            childObject.SetActive(false);
                        }
                    }
                }
            }
            // Initialize RecipeItemData component that will hold all recipe data througout the game
            RecipeItemData newRecipeData = newItem.GetComponent<RecipeItemData>() ?? newItem.AddComponent<RecipeItemData>();
            newRecipeData.guid = guid;
            newRecipeData.recipeName = recipeName;
            newRecipeData.recipeProduct = recipeProduct;
            newRecipeData.recipeType = recipeType;
            newRecipeData.itemClass = itemClass;
            newRecipeData.index = index;
            newRecipeData.experience = experience;
            newRecipeData.productionTime = productionTime;
            newRecipeData.outputValue = outputValue;
            newRecipeData.currentQuantity = currentQuantity;
            newRecipeData.hasRequirements = hasRequirements;
            newRecipeData.childData = childDataList;
            newRecipeData.orderAdded = recipeOrderAdded++;

            recipeManager.AddToItemArray(recipeProduct, newItem);

            newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);
            newItem.transform.localScale = Vector3.one;
        }

        private Sprite AssignChildSlot(string spriteName)
        {
            Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
            return sprite;
        }
        private Sprite AssignSpriteToSlot(string spriteName, string recipeProduct)
        {
            if (recipeProduct == "BUILDINGS")
            {
                Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("buildingicons", spriteName);
                return sprite;
            }
            else
            {
                Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
                return sprite;
            }
        }
    }
}
