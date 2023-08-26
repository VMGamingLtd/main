using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ItemManagement;
using TMPro;
using System.Globalization;

public class EquipmentManager : MonoBehaviour
{
    public static bool[] slotEquipped = new bool[9];
    public static string[] slotEquippedName = new string[9];
    public static bool autoConsumption;
    public GlobalCalculator globalCalculator;

    public GameObject HelmetSlot;
    public GameObject SuitSlot;
    public GameObject LeftHandSlot;
    public GameObject BackpackSlot;
    public GameObject RightHandSlot;
    public GameObject EnergySlot;
    public GameObject OxygenSlot;
    public GameObject WaterSlot;
    public GameObject HungerSlot;

    public void InitStartEquip()
    {
        for (int i = 0; i < slotEquipped.Length; i++)
        {
            slotEquipped[i] = false;
        }
    }

    public void DeductFromEquip()
    {
        for (int i = 0; i < slotEquipped.Length; i++)
        {
            if (slotEquipped[i])
            {
                if (i == 5)
                {
                    Transform targetChild = EnergySlot.transform.Cast<Transform>()
                        .FirstOrDefault(child => child.name.Contains("(Clone)"));
                    if (targetChild != null && targetChild.name == "Battery(Clone)")
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null && itemData.itemQuantity > PlayerResources.PlayerEnergy)
                        {
                            itemData.itemQuantity -= PlayerResources.PlayerEnergy;
                            TextMeshProUGUI newCountText = itemData.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                            if (newCountText != null)
                            {
                                newCountText.text = itemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            if (!autoConsumption)
                            {
                                slotEquipped[5] = false;
                                slotEquippedName[5] = "";
                                PlayerResources.PlayerEnergy = 0f;
                                GameObject noEnergyObjects = GameObject.Find("NoEnergyObjects");
                                if (noEnergyObjects != null)
                                {
                                    ActivateObjects activateScript = noEnergyObjects.GetComponent<ActivateObjects>();
                                    activateScript?.ActivateAllObjects();
                                }
                                targetChild.parent.transform.Find("EmptyButton")?.GetComponent<Image>()?.gameObject.SetActive(true);
                                Destroy(targetChild.gameObject);
                                globalCalculator.UpdatePlayerConsumption();
                            }
                            Debug.Log("battery is empty");
                        }
                    }
                }
                if (i == 6 && GlobalCalculator.isPlayerInBiologicalBiome == false)
                {
                    Transform targetChild = OxygenSlot.transform.Cast<Transform>()
                        .FirstOrDefault(child => child.name.Contains("(Clone)"));
                    if (targetChild != null && targetChild.name == "OxygenTank(Clone)")
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null)
                        {

                        }

                    }
                }
                if (i == 7)
                {
                    Transform targetChild = WaterSlot.transform.Cast<Transform>()
                        .FirstOrDefault(child => child.name.Contains("(Clone)"));
                    if (targetChild != null && targetChild.name == "DistilledWater(Clone)")
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null && itemData.itemQuantity > PlayerResources.PlayerWater)
                        {
                            itemData.itemQuantity -= PlayerResources.PlayerWater;
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }
}
