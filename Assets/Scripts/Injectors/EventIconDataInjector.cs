using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enumerations;

public class EventIconDataInjector : MonoBehaviour
{
    private GameObject equipment;
    public Transform statList;
    public TranslationManager translationManager;
    public GameObject stat;

    public void InjectData(EventIconData eventIconData)
    {
        equipment = transform.gameObject;

        if (eventIconData.Type == EventIconType.Player)
        {
            equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = UserName.userName;
            equipment.transform.Find("Stats/Quantity").gameObject.SetActive(false);
            equipment.transform.Find("Header/Image").gameObject.SetActive(false);
            return;
        }
        else if (eventIconData.EventLevel > -1)
        {
            equipment.transform.Find("Header/Image").gameObject.SetActive(true);
            equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(eventIconData.Name);
            equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignBuildingSpriteToSlot(eventIconData.name);
            equipment.transform.Find("Stats/Quantity").gameObject.SetActive(false);
            equipment.transform.Find("Stats/Level").gameObject.SetActive(true);
            equipment.transform.Find("Stats/Size").gameObject.SetActive(true);
            equipment.transform.Find("Stats/Level/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Level");
            equipment.transform.Find("Stats/Size/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Size");
            equipment.transform.Find("Stats/Level/Value").GetComponent<TextMeshProUGUI>().text = eventIconData.EventLevel.ToString();
            equipment.transform.Find("Stats/Size/Value").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(eventIconData.Size.ToString());
            return;
        }

        NumberFormater numberFormater = new();
        equipment.transform.Find("Header/Image").gameObject.SetActive(true);
        equipment.transform.Find("Stats/Quantity").gameObject.SetActive(true);
        equipment.transform.Find("Stats/Level").gameObject.SetActive(false);
        equipment.transform.Find("Stats/Size").gameObject.SetActive(false);
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(eventIconData.Name);
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignResourceSpriteToSlot(eventIconData.name);
        equipment.transform.Find("Stats/Quantity/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Quantity");
        equipment.transform.Find("Stats/Quantity/CurrentQuantity").GetComponent<TextMeshProUGUI>().text = numberFormater.FormatNumberInThousands(eventIconData.CurrentQuantity).ToString();
        equipment.transform.Find("Stats/Quantity/MaxQuantity").GetComponent<TextMeshProUGUI>().text = numberFormater.FormatNumberInThousands(eventIconData.MaxQuantityRange).ToString();


        if (eventIconData.Type == EventIconType.Fish)
        {
            CreateStat(Constants.FishMeat);
        }
        else if (eventIconData.Type == EventIconType.FurAnimal)
        {
            CreateStat(Constants.AnimalMeat);
            CreateStat(Constants.AnimalSkin);
        }
        else if (eventIconData.Type == EventIconType.MilkAnimal)
        {
            CreateStat(Constants.AnimalMeat);
            CreateStat(Constants.Milk);
        }
    }

    void OnDisable()
    {
        foreach (Transform child in statList)
        {
            if (child.name != "Quantity" && child.name != "Level" && child.name != "Size") Destroy(child.gameObject);
        }
    }

    private void CreateStat(string Name)
    {
        GameObject newStat = Instantiate(stat, statList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
        newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignResourceSpriteToSlot(Name);
        newStat.transform.Find("Value").gameObject.SetActive(false);
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }
}
