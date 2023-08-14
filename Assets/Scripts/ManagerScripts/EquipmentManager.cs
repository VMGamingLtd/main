using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ItemManagement;

public class EquipmentManager : MonoBehaviour
{
    public static bool[] slotEquipped = new bool[9];
    public static string[] slotEquippedName = new string[9];

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

    /*public void CheckForEquip()
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
                        if (itemData != null)
                        {

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
            }
        }
    }*/
}
