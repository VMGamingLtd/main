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
    public GameObject energyProductionRowTemplate;
    public RectTransform overviewProductionList;
    public TranslationManager translationManager;

    public GameObject EnlistManualProduction(RecipeItemData recipeData)
    {
        GameObject newItem = Instantiate(manualProductionRowTemplate, overviewProductionList);
        newItem.name = "manual";
        CoroutineManager.manualProduction = true;
        if (recipeData.recipeProduct == "BUILDINGS")
        {
            newItem.transform.Find("OutputIcon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(recipeData.recipeName);
        }
        else
        {
            newItem.transform.Find("OutputIcon").GetComponent<Image>().sprite = AssignSpriteToSlot(recipeData.recipeName);
        }

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
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        if (itemData.consumedSlotCount == 0)
        {
            newItem.transform.Find("RedArrow").gameObject.SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);
            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
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

        for (int i = 0; i < 4; i++)
        {
            string childName = "OutputResource" + (i + 1);
            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
            {
                GameObject childObject = childTransform.gameObject;
                SlotItemData child = (i < itemData.producedItems.Count) ? itemData.producedItems[i] : null;

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
        NumberFormater formatter = new();
        GameObject newItem = Instantiate(energyProductionRowTemplate, overviewProductionList);
        newItem.name = itemData.ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        newItem.transform.Find("OutputIcon").GetComponent<Image>().sprite = AssignSpriteToSlot("Electricity");

        string formattedPower = formatter.FormatEnergyInThousands(itemData.powerOutput);
        newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = formattedPower;

        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);
            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
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

    public GameObject EnlistResearchBuildingProduction(ResearchBuildingItemData itemData)
    {
        GameObject newItem = Instantiate(productionRowTemplate, overviewProductionList);
        newItem.name = itemData.ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        if (itemData.consumedSlotCount == 0)
        {
            newItem.transform.Find("RedArrow").gameObject.SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);

            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
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

        string childName2 = "OutputResource1";
        if (newItem.transform.Find(childName2).TryGetComponent<Transform>(out var childTransform2))
        {
            GameObject childObject = childTransform2.gameObject;
            childObject.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot("ResearchPoint");
            childObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = itemData.researchPoints.ToString();
        }

        // leave only first 'OutputResource1' to display and disable the rest
        for (int i = 1; i < 4; i++)
        {
            string childName = "OutputResource" + (i + 1);
            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
            {
                GameObject childObject = childTransform.gameObject;
                childObject.SetActive(false);
            }
        }

        return newItem;
    }

    public GameObject RecreateBuildingProduction(BuildingItemDataModel itemData, int ID)
    {
        GameObject newItem = Instantiate(productionRowTemplate, overviewProductionList);
        newItem.name = ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        if (itemData.consumedSlotCount == 0)
        {
            newItem.transform.Find("RedArrow").gameObject.SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);

            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
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

        for (int i = 0; i < 4; i++)
        {
            string childName = "OutputResource" + (i + 1);
            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
            {
                GameObject childObject = childTransform.gameObject;
                SlotItemData child = (i < itemData.producedItems.Count) ? itemData.producedItems[i] : null;

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

    public GameObject RecreateResearchBuildingProduction(ResearchBuildingItemDataModel itemData, int ID)
    {
        GameObject newItem = Instantiate(productionRowTemplate, overviewProductionList);
        newItem.name = ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        if (itemData.consumedSlotCount == 0)
        {
            newItem.transform.Find("RedArrow").gameObject.SetActive(false);
        }

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);

            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
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

        string childName2 = "OutputResource1";
        if (newItem.transform.Find(childName2).TryGetComponent<Transform>(out var childTransform2))
        {
            GameObject childObject = childTransform2.gameObject;
            childObject.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot("ResearchPoint");
            childObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = itemData.researchPoints.ToString();
        }

        // leave only first 'OutputResource1' to display and disable the rest
        for (int i = 1; i < 4; i++)
        {
            string childName = "OutputResource" + (i + 1);
            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
            {
                GameObject childObject = childTransform.gameObject;
                childObject.SetActive(false);
            }
        }

        return newItem;
    }

    public GameObject RecreateEnergyBuildingProduction(EnergyBuildingItemDataModel itemData, int ID)
    {
        NumberFormater formatter = new();
        GameObject newItem = Instantiate(energyProductionRowTemplate, overviewProductionList);
        newItem.name = ID.ToString();
        newItem.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlotBuilding(itemData.spriteIconName);
        newItem.transform.Find("OutputIcon").GetComponent<Image>().sprite = AssignSpriteToSlot("Electricity");

        string formattedPower = formatter.FormatEnergyInThousands(itemData.powerOutput);
        newItem.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = formattedPower;

        newItem.transform.Find("Location").GetComponent<TextMeshProUGUI>().text = itemData.buildingName;

        for (int i = 0; i < 4; i++)
        {
            string childName = "ConsumeResource" + (i + 1);
            if (newItem.transform.Find(childName).TryGetComponent<Transform>(out var childTransform))
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

    public void ChangeProductionItemBackground(Color color, EnergyBuildingItemData energyItemData = null, BuildingItemData itemData = null,
        ResearchBuildingItemData researchItemData = null)
    {
        string childID;

        if (energyItemData != null)
        {
            childID = energyItemData.ID.ToString();
        }
        else if (researchItemData != null)
        {
            childID = researchItemData.ID.ToString();
        }
        else
        {
            childID = itemData.ID.ToString();
        }

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

    public TimebarControl LinkTimebarToBuildingData(EnergyBuildingItemData energyItemData = null, BuildingItemData itemData = null,
        ResearchBuildingItemData researchItemData = null)
    {
        string childID;

        if (energyItemData != null)
        {
            childID = energyItemData.ID.ToString();
        }
        else if (researchItemData != null)
        {
            childID = researchItemData.ID.ToString();
        }
        else
        {
            childID = itemData.ID.ToString();
        }
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
