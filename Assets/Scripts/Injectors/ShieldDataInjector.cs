using ItemManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDataInjector : MonoBehaviour
{
    private GameObject equipment;
    [SerializeField] Transform statList;
    [SerializeField] TranslationManager translationManager;
    [SerializeField] GameObject stat;

    public void InjectData(ShieldData shieldData)
    {
        equipment = transform.gameObject;
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = shieldData.name;
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignEquipmentSpriteToSlot(shieldData.name);
        equipment.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(shieldData.itemProduct);
        equipment.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(shieldData.itemType);
        equipment.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(shieldData.itemClass);
        equipment.transform.Find("Stats/Durability/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Durability");
        equipment.transform.Find("Stats/Durability/DurabilityValue").GetComponent<TextMeshProUGUI>().text = shieldData.durability.ToString();
        equipment.transform.Find("Stats/Durability/MaxDurabilityValue").GetComponent<TextMeshProUGUI>().text = shieldData.maxDurability.ToString();

        if (shieldData.shieldPoints > 0)
        {
            CreateStat(Constants.ShieldPoints, shieldData.shieldPoints.ToString());
        }

        if (shieldData.armor > 0)
        {
            CreateStat(Constants.Armor, shieldData.armor.ToString());
        }

        if (shieldData.hitPoints > 0)
        {
            CreateStat(Constants.HitPoints, shieldData.hitPoints.ToString());
        }

        if (shieldData.meleePhysicalDamage > 0)
        {
            CreateStat(Constants.MeleePhysicalDamage, shieldData.meleePhysicalDamage.ToString());
        }

        if (shieldData.meleeFireDamage > 0)
        {
            CreateStat(Constants.MeleeFireDamage, shieldData.meleeFireDamage.ToString());
        }

        if (shieldData.meleeColdDamage > 0)
        {
            CreateStat(Constants.MeleeColdDamage, shieldData.meleeColdDamage.ToString());
        }

        if (shieldData.meleePoisonDamage > 0)
        {
            CreateStat(Constants.MeleePoisonDamage, shieldData.meleePoisonDamage.ToString());
        }

        if (shieldData.meleeEnergyDamage > 0)
        {
            CreateStat(Constants.MeleeEnergyDamage, shieldData.meleeEnergyDamage.ToString());
        }

        if (shieldData.psiDamage > 0)
        {
            CreateStat(Constants.PsiDamage, shieldData.psiDamage.ToString());
        }

        if (shieldData.physicalProtection > 0)
        {
            CreateStat(Constants.PhysicalProtection, shieldData.physicalProtection.ToString());
        }

        if (shieldData.fireProtection > 0)
        {
            CreateStat(Constants.FireProtection, shieldData.fireProtection.ToString());
        }

        if (shieldData.coldProtection > 0)
        {
            CreateStat(Constants.ColdProtection, shieldData.coldProtection.ToString());
        }

        if (shieldData.poisonProtection > 0)
        {
            CreateStat(Constants.PoisonProtection, shieldData.poisonProtection.ToString());
        }

        if (shieldData.energyProtection > 0)
        {
            CreateStat(Constants.EnergyProtection, shieldData.energyProtection.ToString());
        }

        if (shieldData.psiProtection > 0)
        {
            CreateStat(Constants.PsiProtection, shieldData.psiProtection.ToString());
        }

        if (shieldData.strength > 0)
        {
            CreateStat(Constants.Strength, shieldData.strength.ToString());
        }

        if (shieldData.perception > 0)
        {
            CreateStat(Constants.Perception, shieldData.perception.ToString());
        }

        if (shieldData.intelligence > 0)
        {
            CreateStat(Constants.Intelligence, shieldData.intelligence.ToString());
        }

        if (shieldData.agility > 0)
        {
            CreateStat(Constants.Agility, shieldData.agility.ToString());
        }

        if (shieldData.charisma > 0)
        {
            CreateStat(Constants.Charisma, shieldData.charisma.ToString());
        }

        if (shieldData.willpower > 0)
        {
            CreateStat(Constants.Willpower, shieldData.willpower.ToString());
        }

        if (shieldData.hitChance > 0)
        {
            CreateStat(Constants.HitChance, shieldData.hitChance.ToString());
        }

        if (shieldData.dodge > 0)
        {
            CreateStat(Constants.Dodge, shieldData.dodge.ToString());
        }

        if (shieldData.resistance > 0)
        {
            CreateStat(Constants.Resistance, shieldData.resistance.ToString());
        }

        if (shieldData.counterChance > 0)
        {
            CreateStat(Constants.CounterChance, shieldData.counterChance.ToString());
        }

        if (shieldData.penetration > 0)
        {
            CreateStat(Constants.Penetration, shieldData.penetration.ToString());
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
