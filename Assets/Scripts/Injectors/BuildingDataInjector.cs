using BuildingManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDataInjector : MonoBehaviour
{
    private GameObject tooltip;
    [SerializeField] Transform consumeStatList;
    [SerializeField] Transform outputStatList;
    [SerializeField] TranslationManager translationManager;
    [SerializeField] GameObject stat;

    public void InjectData(BuildingItemData buildingData)
    {
        tooltip = transform.gameObject;
        tooltip.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = buildingData.buildingName;
        tooltip.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignBuildingSpriteToSlot(buildingData.name);
        tooltip.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(buildingData.buildingType);
        tooltip.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(buildingData.buildingClass);
        tooltip.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(buildingData.buildingName + "Desc");

        foreach (SlotItemData consumeItem in buildingData.consumedItems)
        {
            CreateConsumeStat(consumeItem.itemName, consumeItem.quantity.ToString(), consumeItem.itemName);
        }

        foreach (SlotItemData outputItem in buildingData.producedItems)
        {
            CreateOutputStat(outputItem.itemName, outputItem.quantity.ToString(), outputItem.itemName);
        }
    }

    private void CreateConsumeStat(string Name, string Value, string SpriteName)
    {
        GameObject newStat = Instantiate(stat, consumeStatList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
        newStat.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Value;
        newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignBuildingSpriteToSlot(SpriteName);
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }

    private void CreateOutputStat(string Name, string Value, string SpriteName)
    {
        GameObject newStat = Instantiate(stat, outputStatList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
        newStat.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Value;
        newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignBuildingSpriteToSlot(SpriteName);
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }

    void OnDisable()
    {
        foreach (Transform child in consumeStatList)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child2 in outputStatList)
        {
            Destroy(child2.gameObject);
        }
    }
}
