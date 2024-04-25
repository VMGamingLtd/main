using RecipeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDataInjector : MonoBehaviour
{
    private GameObject tooltip;
    public Transform materialStatList;
    public Transform outputStatList;
    public TranslationManager translationManager;
    public GameObject stat;
    public GameObject MaterialsTitleText;
    public GameObject RemainingTitleText;
    public GameObject RemainingQuantityText;

    public void InjectData(RecipeItemData recipeData)
    {
        tooltip = transform.gameObject;
        tooltip.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeData.recipeName);

        if (recipeData.recipeProduct == "BUILDINGS")
        {
            tooltip.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssignBuildingSpriteToSlot(recipeData.recipeName);
        }
        else
        {
            tooltip.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssignResourceSpriteToSlot(recipeData.recipeName);
        }

        tooltip.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeData.recipeProduct);
        tooltip.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeData.recipeType);
        tooltip.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeData.itemClass);
        tooltip.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeData.recipeName + "Desc");
        tooltip.transform.Find("OutputTitle").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Output");

        if (recipeData.currentQuantity >= 0)
        {
            RemainingTitleText.SetActive(true);
            RemainingQuantityText.SetActive(true);
            tooltip.transform.Find("RemainingTitle").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("RemainingQuantity");
            tooltip.transform.Find("RemainingQuantity").GetComponent<TextMeshProUGUI>().text = recipeData.currentQuantity.ToString();
        }
        else
        {
            RemainingTitleText.SetActive(false);
            RemainingQuantityText.SetActive(false);
        }

        if (recipeData.childData.Count > 0)
        {
            MaterialsTitleText.SetActive(true);
            var textMeshPro = MaterialsTitleText.transform.GetComponent<TextMeshProUGUI>();
            textMeshPro.text = translationManager.Translate("Materials");
        }
        else
        {
            MaterialsTitleText.SetActive(false);
        }

        foreach (ChildData consumeItem in recipeData.childData)
        {
            GameObject constumeStat = Instantiate(stat, materialStatList);
            constumeStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(consumeItem.name);
            constumeStat.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = consumeItem.quantity.ToString();
            if (consumeItem.type == "BUILDINGS")
            {
                constumeStat.transform.Find("Icon").GetComponent<Image>().sprite = AssignBuildingSpriteToSlot(consumeItem.name);
            }
            else
            {
                constumeStat.transform.Find("Icon").GetComponent<Image>().sprite = AssignResourceSpriteToSlot(consumeItem.name);
            }
            constumeStat.transform.localPosition = Vector3.one;
            constumeStat.transform.localScale = Vector3.one;
        }

        GameObject newStat = Instantiate(stat, outputStatList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(recipeData.recipeName);
        newStat.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = recipeData.outputValue.ToString();
        if (recipeData.recipeProduct == "BUILDINGS")
        {
            newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssignBuildingSpriteToSlot(recipeData.recipeName);
        }
        else
        {
            newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssignResourceSpriteToSlot(recipeData.recipeName);
        }
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }

    void OnDisable()
    {
        foreach (Transform child in materialStatList)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child2 in outputStatList)
        {
            Destroy(child2.gameObject);
        }
    }

    private Sprite AssignResourceSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
        return sprite;
    }

    private Sprite AssignBuildingSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("buildingicons", spriteName);
        return sprite;
    }
}
