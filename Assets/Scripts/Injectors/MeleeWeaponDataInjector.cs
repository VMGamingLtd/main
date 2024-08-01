using ItemManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeaponDataInjector : MonoBehaviour
{
    private GameObject equipment;
    [SerializeField] Transform statList;
    [SerializeField] TranslationManager translationManager;
    [SerializeField] GameObject stat;

    public void InjectData(MeleeWeaponData meleeWeaponData)
    {
        equipment = transform.gameObject;
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = meleeWeaponData.name;
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignEquipmentSpriteToSlot(meleeWeaponData.name);
        equipment.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(meleeWeaponData.itemProduct);
        equipment.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(meleeWeaponData.itemType);
        equipment.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(meleeWeaponData.itemClass);
        equipment.transform.Find("Stats/Durability/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Durability");
        equipment.transform.Find("Stats/Durability/DurabilityValue").GetComponent<TextMeshProUGUI>().text = meleeWeaponData.durability.ToString();
        equipment.transform.Find("Stats/Durability/MaxDurabilityValue").GetComponent<TextMeshProUGUI>().text = meleeWeaponData.maxDurability.ToString();

        if (meleeWeaponData.meleePhysicalDamage > 0)
        {
            CreateStat(Constants.MeleePhysicalDamage, meleeWeaponData.meleePhysicalDamage.ToString());
        }

        if (meleeWeaponData.meleeFireDamage > 0)
        {
            CreateStat(Constants.MeleeFireDamage, meleeWeaponData.meleeFireDamage.ToString());
        }

        if (meleeWeaponData.meleeColdDamage > 0)
        {
            CreateStat(Constants.MeleeColdDamage, meleeWeaponData.meleeColdDamage.ToString());
        }

        if (meleeWeaponData.meleePoisonDamage > 0)
        {
            CreateStat(Constants.MeleePoisonDamage, meleeWeaponData.meleePoisonDamage.ToString());
        }

        if (meleeWeaponData.meleeEnergyDamage > 0)
        {
            CreateStat(Constants.MeleeEnergyDamage, meleeWeaponData.meleeEnergyDamage.ToString());
        }

        if (meleeWeaponData.psiDamage > 0)
        {
            CreateStat(Constants.PsiDamage, meleeWeaponData.psiDamage.ToString());
        }

        if (meleeWeaponData.strength > 0)
        {
            CreateStat(Constants.Strength, meleeWeaponData.strength.ToString());
        }

        if (meleeWeaponData.perception > 0)
        {
            CreateStat(Constants.Perception, meleeWeaponData.perception.ToString());
        }

        if (meleeWeaponData.intelligence > 0)
        {
            CreateStat(Constants.Intelligence, meleeWeaponData.intelligence.ToString());
        }

        if (meleeWeaponData.agility > 0)
        {
            CreateStat(Constants.Agility, meleeWeaponData.agility.ToString());
        }

        if (meleeWeaponData.charisma > 0)
        {
            CreateStat(Constants.Charisma, meleeWeaponData.charisma.ToString());
        }

        if (meleeWeaponData.willpower > 0)
        {
            CreateStat(Constants.Willpower, meleeWeaponData.willpower.ToString());
        }

        if (meleeWeaponData.hitChance > 0)
        {
            CreateStat(Constants.HitChance, meleeWeaponData.hitChance.ToString());
        }

        if (meleeWeaponData.dodge > 0)
        {
            CreateStat(Constants.Dodge, meleeWeaponData.dodge.ToString());
        }

        if (meleeWeaponData.resistance > 0)
        {
            CreateStat(Constants.Resistance, meleeWeaponData.resistance.ToString());
        }

        if (meleeWeaponData.counterChance > 0)
        {
            CreateStat(Constants.CounterChance, meleeWeaponData.counterChance.ToString());
        }

        if (meleeWeaponData.penetration > 0)
        {
            CreateStat(Constants.Penetration, meleeWeaponData.penetration.ToString());
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
