using ItemManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelmetDataInjector : MonoBehaviour
{
    private GameObject equipment;
    [SerializeField] Transform statList;
    [SerializeField] TranslationManager translationManager;
    [SerializeField] GameObject stat;

    public void InjectData(HelmetData helmetData)
    {
        equipment = transform.gameObject;
        equipment.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = helmetData.name;
        equipment.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignEquipmentSpriteToSlot(helmetData.name);
        equipment.transform.Find("Product").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(helmetData.itemProduct);
        equipment.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(helmetData.itemType);
        equipment.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(helmetData.itemClass);
        equipment.transform.Find("Stats/Durability/Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Durability");
        equipment.transform.Find("Stats/Durability/DurabilityValue").GetComponent<TextMeshProUGUI>().text = helmetData.durability.ToString();
        equipment.transform.Find("Stats/Durability/MaxDurabilityValue").GetComponent<TextMeshProUGUI>().text = helmetData.maxDurability.ToString();

        if (helmetData.physicalProtection > 0)
        {
            CreateStat(Constants.PhysicalProtection, helmetData.physicalProtection.ToString());
        }

        if (helmetData.fireProtection > 0)
        {
            CreateStat(Constants.FireProtection, helmetData.fireProtection.ToString());
        }

        if (helmetData.coldProtection > 0)
        {
            CreateStat(Constants.ColdProtection, helmetData.coldProtection.ToString());
        }

        if (helmetData.poisonProtection > 0)
        {
            CreateStat(Constants.PoisonProtection, helmetData.poisonProtection.ToString());
        }

        if (helmetData.energyProtection > 0)
        {
            CreateStat(Constants.EnergyProtection, helmetData.energyProtection.ToString());
        }

        if (helmetData.psiProtection > 0)
        {
            CreateStat(Constants.PsiProtection, helmetData.psiProtection.ToString());
        }

        if (helmetData.shieldPoints > 0)
        {
            CreateStat(Constants.ShieldPoints, helmetData.shieldPoints.ToString());
        }

        if (helmetData.armor > 0)
        {
            CreateStat(Constants.Armor, helmetData.armor.ToString());
        }

        if (helmetData.hitPoints > 0)
        {
            CreateStat(Constants.HitPoints, helmetData.hitPoints.ToString());
        }

        if (helmetData.visibilityRadius > 0)
        {
            CreateStat(Constants.VisibilityRadius, helmetData.visibilityRadius.ToString());
        }

        if (helmetData.explorationRadius > 0)
        {
            CreateStat(Constants.ExplorationRadius, helmetData.explorationRadius.ToString());
        }

        if (helmetData.pickupRadius > 0)
        {
            CreateStat(Constants.PickupRadius, helmetData.pickupRadius.ToString());
        }

        if (helmetData.strength > 0)
        {
            CreateStat(Constants.Strength, helmetData.strength.ToString());
        }

        if (helmetData.perception > 0)
        {
            CreateStat(Constants.Perception, helmetData.perception.ToString());
        }

        if (helmetData.intelligence > 0)
        {
            CreateStat(Constants.Intelligence, helmetData.intelligence.ToString());
        }

        if (helmetData.agility > 0)
        {
            CreateStat(Constants.Agility, helmetData.agility.ToString());
        }

        if (helmetData.charisma > 0)
        {
            CreateStat(Constants.Charisma, helmetData.charisma.ToString());
        }

        if (helmetData.willpower > 0)
        {
            CreateStat(Constants.Willpower, helmetData.willpower.ToString());
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
