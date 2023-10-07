using RecipeManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionCreator : MonoBehaviour
{
    public GameObject manualProductionRowTemplate;
    public GameObject productionRowTemplate;
    public RectTransform overviewProductionList;
    public TranslationManager translationManager;
    public static int rowID = 0;
    public static int manualRowID;


    public void EnlistManualProduction(RecipeDataJson recipeData)
    {
        rowID++;
        manualRowID = rowID;
        GameObject newItem = Instantiate(manualProductionRowTemplate, overviewProductionList);
        newItem.name = rowID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(recipeData.recipeName);
        newItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeData.recipeName);
        newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = recipeData.outputValue.ToString();
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("ManualProduction");
    }

    public void DelistManualProduction()
    {
        if (manualRowID != 0)
        {
            string childName = manualRowID.ToString();
            GameObject foundChild = FindChild(overviewProductionList, childName);
            if (foundChild != null)
            {
                Destroy(foundChild);
            }
            manualRowID = 0;
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
}
