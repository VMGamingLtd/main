using ItemManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuitDataInjector : MonoBehaviour
{
    private GameObject equipment;
    public Transform statList;
    public TranslationManager translationManager;
    public GameObject stat;

    public void InjectData(SuitData suitData)
    {
        equipment = transform.gameObject;
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = suitData.name;
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(suitData.name);
        equipment.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(suitData.itemProduct);
        equipment.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(suitData.itemType);
        equipment.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(suitData.itemClass);
        equipment.transform.Find("Stats/Durability/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Durability");
        equipment.transform.Find("Stats/Durability/DurabilityValue").GetComponent<TextMeshProUGUI>().text = suitData.durability.ToString();
        equipment.transform.Find("Stats/Durability/MaxDurabilityValue").GetComponent<TextMeshProUGUI>().text = suitData.maxDurability.ToString();

        if (suitData.physicalProtection > 0)
        {
            CreateStat("PhysicalProtection", suitData.physicalProtection.ToString());
        }

        if (suitData.fireProtection > 0)
        {
            CreateStat("FireProtection", suitData.fireProtection.ToString());
        }

        if (suitData.coldProtection > 0)
        {
            CreateStat("ColdProtection", suitData.coldProtection.ToString());
        }

        if (suitData.gasProtection > 0)
        {
            CreateStat("GasProtection", suitData.gasProtection.ToString());
        }

        if (suitData.explosionProtection > 0)
        {
            CreateStat("ExplosionProtection", suitData.explosionProtection.ToString());
        }

        if (suitData.shieldPoints > 0)
        {
            CreateStat("ShieldPoints", suitData.shieldPoints.ToString());
        }

        if (suitData.hitPoints > 0)
        {
            CreateStat("HitPoints", suitData.hitPoints.ToString());
        }

        if (suitData.energyCapacity > 0)
        {
            CreateStat("EnergyCapacity", suitData.energyCapacity.ToString());
        }

        if (suitData.inventorySlots > 0)
        {
            CreateStat("InventorySlots", suitData.inventorySlots.ToString());
        }

        if (suitData.strength > 0)
        {
            CreateStat("Strength", suitData.strength.ToString());
        }

        if (suitData.perception > 0)
        {
            CreateStat("Perception", suitData.perception.ToString());
        }

        if (suitData.intelligence > 0)
        {
            CreateStat("Intelligence", suitData.intelligence.ToString());
        }

        if (suitData.agility > 0)
        {
            CreateStat("Agility", suitData.agility.ToString());
        }

        if (suitData.charisma > 0)
        {
            CreateStat("Charisma", suitData.charisma.ToString());
        }

        if (suitData.willpower > 0)
        {
            CreateStat("Willpower", suitData.willpower.ToString());
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
    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("equipmenticons", spriteName);
        return sprite;
    }
}
