using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventIconDataInjector : MonoBehaviour
{
    private GameObject equipment;
    public Transform statList;
    public TranslationManager translationManager;
    public GameObject stat;


    public void InjectData(EventIconData eventIconData)
    {
        equipment = transform.gameObject;
        NumberFormater numberFormater = new();
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(eventIconData.Name);
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(eventIconData.name);
        equipment.transform.Find("Stats/Quantity/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Quantity");
        equipment.transform.Find("Stats/Quantity/CurrentQuantity").GetComponent<TextMeshProUGUI>().text = numberFormater.FormatNumberInThousands(eventIconData.CurrentQuantity).ToString();
        equipment.transform.Find("Stats/Quantity/MaxQuantity").GetComponent<TextMeshProUGUI>().text = numberFormater.FormatNumberInThousands(eventIconData.MaxQuantity).ToString();

        if (eventIconData.Type == EventIconType.Fish)
        {
            CreateStat("FishMeat");
        }
        else if (eventIconData.Type == EventIconType.FurAnimal)
        {
            CreateStat("AnimalSkin");
        }
        else if (eventIconData.Type == EventIconType.MilkAnimal)
        {
            CreateStat("Milk");
        }
    }

    void OnDisable()
    {
        foreach (Transform child in statList)
        {
            if (child.name != "Quantity") Destroy(child.gameObject);
        }
    }

    private void CreateStat(string Name)
    {
        GameObject newStat = Instantiate(stat, statList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
        newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(Name);
        newStat.transform.Find("Value").gameObject.SetActive(false);
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }

    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
        return sprite;
    }
}
