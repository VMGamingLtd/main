using ItemManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuitDataInjector : MonoBehaviour
{
    private GameObject equipment;
    [SerializeField] Transform statList;
    [SerializeField] TranslationManager translationManager;
    [SerializeField] GameObject stat;

    public void InjectData(SuitData suitData)
    {
        equipment = transform.gameObject;
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(suitData.name);
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignEquipmentSpriteToSlot(suitData.name);
        equipment.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(suitData.itemProduct);
        equipment.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(suitData.itemType);
        equipment.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(suitData.itemClass);
        equipment.transform.Find("Stats/Durability/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Durability");
        equipment.transform.Find("Stats/Durability/DurabilityValue").GetComponent<TextMeshProUGUI>().text = suitData.durability.ToString();
        equipment.transform.Find("Stats/Durability/MaxDurabilityValue").GetComponent<TextMeshProUGUI>().text = suitData.maxDurability.ToString();

        if (suitData.physicalProtection > 0)
        {
            CreateStat(Constants.PhysicalProtection, suitData.physicalProtection.ToString());
        }

        if (suitData.fireProtection > 0)
        {
            CreateStat(Constants.FireProtection, suitData.fireProtection.ToString());
        }

        if (suitData.coldProtection > 0)
        {
            CreateStat(Constants.ColdProtection, suitData.coldProtection.ToString());
        }

        if (suitData.poisonProtection > 0)
        {
            CreateStat(Constants.PoisonProtection, suitData.poisonProtection.ToString());
        }

        if (suitData.energyProtection > 0)
        {
            CreateStat(Constants.EnergyProtection, suitData.energyProtection.ToString());
        }

        if (suitData.psiProtection > 0)
        {
            CreateStat(Constants.PsiProtection, suitData.psiProtection.ToString());
        }

        if (suitData.shieldPoints > 0)
        {
            CreateStat(Constants.ShieldPoints, suitData.shieldPoints.ToString());
        }

        if (suitData.armor > 0)
        {
            CreateStat(Constants.Armor, suitData.armor.ToString());
        }

        if (suitData.hitPoints > 0)
        {
            CreateStat(Constants.HitPoints, suitData.hitPoints.ToString());
        }

        if (suitData.energyCapacity > 0)
        {
            CreateStat(Constants.EnergyCapacity, suitData.energyCapacity.ToString());
        }

        if (suitData.inventorySlots > 0)
        {
            CreateStat(Constants.InventorySlots, suitData.inventorySlots.ToString());
        }

        if (suitData.strength > 0)
        {
            CreateStat(Constants.Strength, suitData.strength.ToString());
        }

        if (suitData.perception > 0)
        {
            CreateStat(Constants.Perception, suitData.perception.ToString());
        }

        if (suitData.intelligence > 0)
        {
            CreateStat(Constants.Intelligence, suitData.intelligence.ToString());
        }

        if (suitData.agility > 0)
        {
            CreateStat(Constants.Agility, suitData.agility.ToString());
        }

        if (suitData.charisma > 0)
        {
            CreateStat(Constants.Charisma, suitData.charisma.ToString());
        }

        if (suitData.willpower > 0)
        {
            CreateStat(Constants.Willpower, suitData.willpower.ToString());
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
