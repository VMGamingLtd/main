using BuildingManagement;
using RecipeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionCreator : MonoBehaviour
{
    public GameObject manualProductionRowTemplate;
    public GameObject productionRowTemplate;
    public RectTransform overviewProductionList;
    public TranslationManager translationManager;

    public void EnlistManualProduction(RecipeItemData recipeData)
    {
        GameObject newItem = Instantiate(manualProductionRowTemplate, overviewProductionList);
        newItem.name = "manual";
        CoroutineManager.manualProduction = true;
        if (recipeData.recipeProduct == "BUILDINGS")
        {
            newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(recipeData.recipeName);
        }
        else
        {
            newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(recipeData.recipeName);
        }
        newItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeData.recipeName);
        newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = recipeData.outputValue.ToString();
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("ManualProduction");
    }
    public void EnlistBuildingProduction(EnergyBuildingItemData itemData)
    {
        GameObject newItem = Instantiate(productionRowTemplate, overviewProductionList);
        newItem.name = itemData.ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        newItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Electricity");
        newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = itemData.powerOutput.ToString();
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;
    }
    public void DelistEnergyBuildingProduction(EnergyBuildingItemData itemData)
    {
        string childID = itemData.ID.ToString();
        Debug.Log(childID);
        GameObject foundChild = FindChild(overviewProductionList, childID);
        if (foundChild != null)
        {
            Debug.Log("child found!");
            Destroy(foundChild);
        }
    }
    public void DelistManualProduction()
    {
        CoroutineManager.manualProduction = false;
        string childName = "manual";
        GameObject foundChild = FindChild(overviewProductionList, childName);
        if (foundChild != null)
        {
            Destroy(foundChild);
        }
    }

    GameObject FindChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }

            GameObject foundChild = FindChild(child, name);
            if (foundChild != null)
            {
                return foundChild;
            }
        }

        return null; // Child with the specified name not found
    }

    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
        return sprite;
    }
    private Sprite AssignSpriteToSlotBuilding(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("buildingicons", spriteName);
        return sprite;
    }
}
