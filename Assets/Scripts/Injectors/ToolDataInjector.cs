using ItemManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolDataInjector : MonoBehaviour
{
    private GameObject equipment;
    public Transform statList;
    public TranslationManager translationManager;
    public GameObject stat;

    public void InjectData(ToolData toolData)
    {
        equipment = transform.gameObject;
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(toolData.name);
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignEquipmentSpriteToSlot(toolData.name);
        equipment.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(toolData.itemProduct);
        equipment.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(toolData.itemType);
        equipment.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(toolData.itemClass);
        equipment.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(toolData.itemName + "Desc");
        equipment.transform.Find("Stats/Durability/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Durability");
        equipment.transform.Find("Stats/Durability/DurabilityValue").GetComponent<TextMeshProUGUI>().text = toolData.durability.ToString();
        equipment.transform.Find("Stats/Durability/MaxDurabilityValue").GetComponent<TextMeshProUGUI>().text = toolData.maxDurability.ToString();

        if (toolData.strength > 0)
        {
            CreateStat(Constants.Strength, toolData.strength.ToString());
        }

        if (toolData.perception > 0)
        {
            CreateStat(Constants.Perception, toolData.perception.ToString());
        }

        if (toolData.intelligence > 0)
        {
            CreateStat(Constants.Intelligence, toolData.intelligence.ToString());
        }

        if (toolData.agility > 0)
        {
            CreateStat(Constants.Agility, toolData.agility.ToString());
        }

        if (toolData.charisma > 0)
        {
            CreateStat(Constants.Charisma, toolData.charisma.ToString());
        }

        if (toolData.willpower > 0)
        {
            CreateStat(Constants.Willpower, toolData.willpower.ToString());
        }

        if (toolData.productionSpeed > 0)
        {
            CreateStat(Constants.ProductionSpeed, toolData.productionSpeed.ToString("F1") + "x");
        }

        if (toolData.materialCost > 0)
        {
            CreateStat(Constants.MaterialCost, toolData.materialCost.ToString("F1") + "x");
        }

        if (toolData.outcomeRate > 0)
        {
            CreateStat(Constants.OutcomeRate, toolData.outcomeRate.ToString("F1") + "x");
        }
    }


    void OnDisable()
    {
        foreach (Transform child in statList)
        {
            if (child.name != "Durability") Destroy(child.gameObject);
        }
    }

    private void CreateStat(string Name, string Value)
    {
        GameObject newStat = Instantiate(stat, statList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
        newStat.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Value;
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }
}
