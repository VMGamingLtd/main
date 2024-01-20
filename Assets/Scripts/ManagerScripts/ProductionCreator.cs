using BuildingManagement;
using RecipeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class ProductionCreator : MonoBehaviour
{
    public GameObject manualProductionRowTemplate;
    public GameObject productionRowTemplate;
    public RectTransform overviewProductionList;
    public TranslationManager translationManager;

    public GameObject EnlistManualProduction(RecipeItemData recipeData)
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

        string quantity = recipeData.outputValue.ToString();
        if (Player.OutcomeRate > 0)
        {
            float finalQuantity = Player.OutcomeRate * recipeData.outputValue;
            quantity = finalQuantity.ToString();
        }
        if (quantity.EndsWith(".00"))
        {
            quantity = quantity[..^3];
        }
        newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = quantity;

        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("ManualProduction");

        return newItem;
    }
    public GameObject EnlistBuildingProduction(BuildingItemData itemData)
    {
        GameObject newItem = Instantiate(productionRowTemplate, overviewProductionList);
        newItem.name = itemData.ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        //newItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Electricity");
        //newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = itemData.powerOutput.ToString();
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);
            Transform childTransform = newItem.transform.Find(childName);
            if (childTransform != null)
            {
                GameObject childObject = childTransform.gameObject;
                SlotItemData child = (i < itemData.consumedItems.Count) ? itemData.consumedItems[i] : null;

                if (child != null)
                {
                    childObject.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(child.itemName);
                    childObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = child.quantity.ToString();
                }
                else
                {
                    childObject.SetActive(false);
                }
            }
        }

        return newItem;
    }

    public GameObject EnlistEnergyBuildingProduction(EnergyBuildingItemData itemData)
    {
        GameObject newItem = Instantiate(productionRowTemplate, overviewProductionList);
        newItem.name = itemData.ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        newItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Electricity");
        newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = itemData.powerOutput.ToString();
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);
            Transform childTransform = newItem.transform.Find(childName);
            if (childTransform != null)
            {
                GameObject childObject = childTransform.gameObject;
                SlotItemData child = (i < itemData.consumedItems.Count) ? itemData.consumedItems[i] : null;

                if (child != null)
                {
                    childObject.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(child.itemName);
                    childObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = child.quantity.ToString();
                }
                else
                {
                    childObject.SetActive(false);
                }
            }
        }

        return newItem;
    }

    public GameObject RecreateBuildingProduction(BuildingItemDataModel itemData, int ID)
    {
        GameObject newItem = Instantiate(productionRowTemplate, overviewProductionList);
        newItem.name = ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        //newItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Electricity");
        //newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = itemData.powerOutput.ToString();
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);
            Transform childTransform = newItem.transform.Find(childName);
            if (childTransform != null)
            {
                GameObject childObject = childTransform.gameObject;
                SlotItemData child = (i < itemData.consumedItems.Count) ? itemData.consumedItems[i] : null;

                if (child != null)
                {
                    childObject.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(child.itemName);
                    childObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = child.quantity.ToString();
                }
                else
                {
                    childObject.SetActive(false);
                }
            }
        }

        return newItem;
    }

    public GameObject RecreateEnergyBuildingProduction(EnergyBuildingItemDataModel itemData, int ID)
    {
        GameObject newItem = Instantiate(productionRowTemplate, overviewProductionList);
        newItem.name = ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        newItem.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Electricity");
        newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = itemData.powerOutput.ToString();
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);
            Transform childTransform = newItem.transform.Find(childName);
            if (childTransform != null)
            {
                GameObject childObject = childTransform.gameObject;
                SlotItemData child = (i < itemData.consumedItems.Count) ? itemData.consumedItems[i] : null;

                if (child != null)
                {
                    childObject.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(child.itemName);
                    childObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = child.quantity.ToString();
                }
                else
                {
                    childObject.SetActive(false);
                }
            }
        }

        return newItem;
    }

    public void ChangeProductionItemBackground(EnergyBuildingItemData itemData, Color color)
    {
        string childID = itemData.ID.ToString();
        GameObject foundChild = FindChild(overviewProductionList, childID);
        if (foundChild != null)
        {
            if (foundChild.TryGetComponent<Image>(out var image))
            {
                image.color = color;
            }
        }
    }
    public void DelistEnergyBuildingProduction(EnergyBuildingItemData itemData)
    {
        string childID = itemData.ID.ToString();
        GameObject foundChild = FindChild(overviewProductionList, childID);
        if (foundChild != null)
        {
            Destroy(foundChild);
        }
    }

    public TimebarControl LinkTimebarToBuildingData(EnergyBuildingItemData itemData)
    {
        string childID = itemData.ID.ToString();
        GameObject foundChild = FindChild(overviewProductionList, childID);
        if (foundChild != null)
        {
            if (foundChild.TryGetComponent<TimebarControl>(out var timebarControl))
            {
                return timebarControl;
            }
        }
        return null;
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

        return null;
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
