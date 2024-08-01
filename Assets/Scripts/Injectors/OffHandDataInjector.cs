using ItemManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OffHandDataInjector : MonoBehaviour
{
    private GameObject equipment;
    [SerializeField] Transform statList;
    [SerializeField] TranslationManager translationManager;
    [SerializeField] GameObject stat;

    public void InjectData(OffHandData offHandData)
    {
        equipment = transform.gameObject;
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = offHandData.name;
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignEquipmentSpriteToSlot(offHandData.name);
        equipment.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(offHandData.itemProduct);
        equipment.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(offHandData.itemType);
        equipment.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(offHandData.itemClass);
        equipment.transform.Find("Stats/Durability/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Durability");
        equipment.transform.Find("Stats/Durability/DurabilityValue").GetComponent<TextMeshProUGUI>().text = offHandData.durability.ToString();
        equipment.transform.Find("Stats/Durability/MaxDurabilityValue").GetComponent<TextMeshProUGUI>().text = offHandData.maxDurability.ToString();

        if (offHandData.shieldPoints > 0)
        {
            CreateStat(Constants.ShieldPoints, offHandData.shieldPoints.ToString());
        }

        if (offHandData.armor > 0)
        {
            CreateStat(Constants.Armor, offHandData.armor.ToString());
        }

        if (offHandData.hitPoints > 0)
        {
            CreateStat(Constants.HitPoints, offHandData.hitPoints.ToString());
        }

        if (offHandData.meleePhysicalDamage > 0)
        {
            CreateStat(Constants.MeleePhysicalDamage, offHandData.meleePhysicalDamage.ToString());
        }

        if (offHandData.meleeFireDamage > 0)
        {
            CreateStat(Constants.MeleeFireDamage, offHandData.meleeFireDamage.ToString());
        }

        if (offHandData.meleeColdDamage > 0)
        {
            CreateStat(Constants.MeleeColdDamage, offHandData.meleeColdDamage.ToString());
        }

        if (offHandData.meleePoisonDamage > 0)
        {
            CreateStat(Constants.MeleePoisonDamage, offHandData.meleePoisonDamage.ToString());
        }

        if (offHandData.meleeEnergyDamage > 0)
        {
            CreateStat(Constants.MeleeEnergyDamage, offHandData.meleeEnergyDamage.ToString());
        }

        if (offHandData.rangedPhysicalDamage > 0)
        {
            CreateStat(Constants.RangedPhysicalDamage, offHandData.rangedPhysicalDamage.ToString());
        }

        if (offHandData.rangedFireDamage > 0)
        {
            CreateStat(Constants.RangedFireDamage, offHandData.rangedFireDamage.ToString());
        }

        if (offHandData.rangedColdDamage > 0)
        {
            CreateStat(Constants.RangedColdDamage, offHandData.rangedColdDamage.ToString());
        }

        if (offHandData.rangedPoisonDamage > 0)
        {
            CreateStat(Constants.RangedPoisonDamage, offHandData.rangedPoisonDamage.ToString());
        }

        if (offHandData.rangedEnergyDamage > 0)
        {
            CreateStat(Constants.RangedEnergyDamage, offHandData.rangedEnergyDamage.ToString());
        }

        if (offHandData.psiDamage > 0)
        {
            CreateStat(Constants.PsiDamage, offHandData.psiDamage.ToString());
        }

        if (offHandData.physicalProtection > 0)
        {
            CreateStat(Constants.PhysicalProtection, offHandData.physicalProtection.ToString());
        }

        if (offHandData.fireProtection > 0)
        {
            CreateStat(Constants.FireProtection, offHandData.fireProtection.ToString());
        }

        if (offHandData.coldProtection > 0)
        {
            CreateStat(Constants.ColdProtection, offHandData.coldProtection.ToString());
        }

        if (offHandData.poisonProtection > 0)
        {
            CreateStat(Constants.PoisonProtection, offHandData.poisonProtection.ToString());
        }

        if (offHandData.energyProtection > 0)
        {
            CreateStat(Constants.EnergyProtection, offHandData.energyProtection.ToString());
        }

        if (offHandData.psiProtection > 0)
        {
            CreateStat(Constants.PsiProtection, offHandData.psiProtection.ToString());
        }

        if (offHandData.strength > 0)
        {
            CreateStat(Constants.Strength, offHandData.strength.ToString());
        }

        if (offHandData.perception > 0)
        {
            CreateStat(Constants.Perception, offHandData.perception.ToString());
        }

        if (offHandData.intelligence > 0)
        {
            CreateStat(Constants.Intelligence, offHandData.intelligence.ToString());
        }

        if (offHandData.agility > 0)
        {
            CreateStat(Constants.Agility, offHandData.agility.ToString());
        }

        if (offHandData.charisma > 0)
        {
            CreateStat(Constants.Charisma, offHandData.charisma.ToString());
        }

        if (offHandData.willpower > 0)
        {
            CreateStat(Constants.Willpower, offHandData.willpower.ToString());
        }

        if (offHandData.hitChance > 0)
        {
            CreateStat(Constants.HitChance, offHandData.hitChance.ToString());
        }

        if (offHandData.dodge > 0)
        {
            CreateStat(Constants.Dodge, offHandData.dodge.ToString());
        }

        if (offHandData.resistance > 0)
        {
            CreateStat(Constants.Resistance, offHandData.resistance.ToString());
        }

        if (offHandData.counterChance > 0)
        {
            CreateStat(Constants.CounterChance, offHandData.counterChance.ToString());
        }

        if (offHandData.penetration > 0)
        {
            CreateStat(Constants.Penetration, offHandData.penetration.ToString());
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
