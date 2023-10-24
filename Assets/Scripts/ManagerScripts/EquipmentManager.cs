using ItemManagement;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static bool[] slotEquipped = new bool[9];
    public static string[] slotEquippedName = new string[9];
    public static bool autoConsumption;
    public GlobalCalculator globalCalculator;
    public InventoryManager inventoryManager;

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
                    Transform targetChild = EnergySlot.transform.Cast<Transform>().FirstOrDefault(child => child.name.Contains("Battery"));
                    if (targetChild != null && targetChild.name == "Battery")
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null && itemData.itemQuantity > PlayerResources.PlayerEnergy)
                        {
                            itemData.itemQuantity -= PlayerResources.PlayerEnergy;
                            TextMeshProUGUI newCountTextEnergy = itemData.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                            if (newCountTextEnergy != null)
                            {
                                newCountTextEnergy.text = itemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
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
                                targetChild.parent.transform.Find("EmptyButton").GetComponent<Image>().gameObject.SetActive(true);
                                inventoryManager.DestroySpecificItem(targetChild.name, itemData.itemProduct, itemData.ID);
                                Destroy(targetChild.gameObject);
                                globalCalculator.UpdatePlayerConsumption();
                            }
                        }
                    }
                }
                if (i == 6 && GlobalCalculator.isPlayerInBiologicalBiome == false)
                {
                    Transform targetChild = OxygenSlot.transform.Cast<Transform>().FirstOrDefault(child => child.name.Contains("OxygenTank"));
                    if (targetChild != null && targetChild.name == "OxygenTank")
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null)
                        {

                        }

                    }
                }
                if (i == 7)
                {
                    Transform targetChild = WaterSlot.transform.Cast<Transform>().FirstOrDefault(child => child.name.Contains("DistilledWater"));
                    if (targetChild != null)
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null && itemData.itemQuantity > PlayerResources.PlayerWater)
                        {
                            itemData.itemQuantity -= PlayerResources.PlayerWater;
                            TextMeshProUGUI newCountTextWater = itemData.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                            if (newCountTextWater != null)
                            {
                                newCountTextWater.text = itemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            if (!autoConsumption)
                            {
                                slotEquipped[7] = false;
                                slotEquippedName[7] = "";
                                PlayerResources.PlayerWater = 0f;
                                targetChild.parent.transform.Find("EmptyButton").GetComponent<Image>().gameObject.SetActive(true);
                                inventoryManager.DestroySpecificItem(targetChild.name, itemData.itemProduct, itemData.ID);
                                Destroy(targetChild.gameObject);
                                globalCalculator.UpdatePlayerConsumption();
                            }
                        }
                    }
                }
            }
        }
    }
}
