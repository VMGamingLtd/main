using ItemManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDataInjector : MonoBehaviour
{
    private GameObject item;
    public TranslationManager translationManager;

    public void InjectData(ItemData itemData)
    {
        item = transform.gameObject;
        item.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(itemData.itemName);
        item.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(itemData.name);
        item.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(itemData.itemProduct);
        item.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(itemData.itemType);
        item.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(itemData.itemClass);
        item.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(itemData.name + "Desc");
    }

    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
        return sprite;
    }
}
