using ItemManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RangedWeaponDataInjector : MonoBehaviour
{
    private GameObject equipment;
    [SerializeField] Transform statList;
    [SerializeField] TranslationManager translationManager;
    [SerializeField] GameObject stat;

    public void InjectData(RangedWeaponData rangedWeaponData)
    {
        equipment = transform.gameObject;
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(rangedWeaponData.name);
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignEquipmentSpriteToSlot(rangedWeaponData.name);
        equipment.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(rangedWeaponData.itemProduct);
        equipment.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(rangedWeaponData.itemType);
        equipment.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(rangedWeaponData.itemClass);
        equipment.transform.Find("Stats/Durability/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Durability");
        equipment.transform.Find("Stats/Durability/DurabilityValue").GetComponent<TextMeshProUGUI>().text = rangedWeaponData.durability.ToString();
        equipment.transform.Find("Stats/Durability/MaxDurabilityValue").GetComponent<TextMeshProUGUI>().text = rangedWeaponData.maxDurability.ToString();

        if (rangedWeaponData.rangedPhysicalDamage > 0)
        {
            CreateStat(Constants.RangedPhysicalDamage, rangedWeaponData.rangedPhysicalDamage.ToString());
        }

        if (rangedWeaponData.rangedFireDamage > 0)
        {
            CreateStat(Constants.RangedFireDamage, rangedWeaponData.rangedFireDamage.ToString());
        }

        if (rangedWeaponData.rangedColdDamage > 0)
        {
            CreateStat(Constants.RangedColdDamage, rangedWeaponData.rangedColdDamage.ToString());
        }

        if (rangedWeaponData.rangedPoisonDamage > 0)
        {
            CreateStat(Constants.RangedPoisonDamage, rangedWeaponData.rangedPoisonDamage.ToString());
        }

        if (rangedWeaponData.rangedEnergyDamage > 0)
        {
            CreateStat(Constants.RangedEnergyDamage, rangedWeaponData.rangedEnergyDamage.ToString());
        }

        if (rangedWeaponData.psiDamage > 0)
        {
            CreateStat(Constants.PsiDamage, rangedWeaponData.psiDamage.ToString());
        }

        if (rangedWeaponData.strength > 0)
        {
            CreateStat(Constants.Strength, rangedWeaponData.strength.ToString());
        }

        if (rangedWeaponData.perception > 0)
        {
            CreateStat(Constants.Perception, rangedWeaponData.perception.ToString());
        }

        if (rangedWeaponData.intelligence > 0)
        {
            CreateStat(Constants.Intelligence, rangedWeaponData.intelligence.ToString());
        }

        if (rangedWeaponData.agility > 0)
        {
            CreateStat(Constants.Agility, rangedWeaponData.agility.ToString());
        }

        if (rangedWeaponData.charisma > 0)
        {
            CreateStat(Constants.Charisma, rangedWeaponData.charisma.ToString());
        }

        if (rangedWeaponData.willpower > 0)
        {
            CreateStat(Constants.Willpower, rangedWeaponData.willpower.ToString());
        }

        if (rangedWeaponData.hitChance > 0)
        {
            CreateStat(Constants.HitChance, rangedWeaponData.hitChance.ToString());
        }

        if (rangedWeaponData.dodge > 0)
        {
            CreateStat(Constants.Dodge, rangedWeaponData.dodge.ToString());
        }

        if (rangedWeaponData.resistance > 0)
        {
            CreateStat(Constants.Resistance, rangedWeaponData.resistance.ToString());
        }

        if (rangedWeaponData.counterChance > 0)
        {
            CreateStat(Constants.CounterChance, rangedWeaponData.counterChance.ToString());
        }

        if (rangedWeaponData.penetration > 0)
        {
            CreateStat(Constants.Penetration, rangedWeaponData.penetration.ToString());
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
