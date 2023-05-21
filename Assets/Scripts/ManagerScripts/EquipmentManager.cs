using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ItemManagement;

public class EquipmentManager : MonoBehaviour
{
    public static bool[] slotEquipped = new bool[9];

    public GameObject HelmetSlot;
    public GameObject SuitSlot;
    public GameObject ToolSlot;
    public GameObject LeftHandSlot;
    public GameObject BackpackSlot;
    public GameObject RightHandSlot;
    public GameObject DrillSlot;
    public GameObject EnergySlot;
    public GameObject OxygenSlot;

    public void InitStartEquip()
    {
        for (int i = 0; i < slotEquipped.Length; i++)
        {
            slotEquipped[i] = false;
        }
    }

    public void CheckForEquip()
    {
        for (int i = 0; i < slotEquipped.Length; i++)
        {
            if (slotEquipped[i])
            {
                if (i == 7)
                {
                    Transform oxygenChild = OxygenSlot.transform.Cast<Transform>()
                        .FirstOrDefault(child => child.name.Contains("(Clone)"));
                    if (oxygenChild != null && oxygenChild.name == "Battery(Clone)")
                    {
                        ItemData itemData = oxygenChild.GetComponent<ItemData>();
                        if (itemData != null)
                        {
                            string energyTimer = itemData.EnergyTimer;
                            string[] timerParts = energyTimer.Split(':');
                            int days = int.Parse(timerParts[0]);
                            int hours = int.Parse(timerParts[1]);
                            int minutes = int.Parse(timerParts[2]);
                            int seconds = int.Parse(timerParts[3]);
                            PlayerResources.AddCurrentResourceTime(ref PlayerResources.PlayerEnergy, days, hours, minutes, seconds);
                        }
                    }
                }
                if (i == 8 && GlobalCalculator.isPlayerInBiologicalBiome == false)
                {
                    Transform oxygenChild = OxygenSlot.transform.Cast<Transform>()
                        .FirstOrDefault(child => child.name.Contains("(Clone)"));
                    if (oxygenChild != null && oxygenChild.name == "OxygenTank(Clone)")
                    {
                        ItemData itemData = oxygenChild.GetComponent<ItemData>();
                        if (itemData != null)
                        {
                            string oxygenTimer = itemData.OxygenTimer;
                            string[] timerParts = oxygenTimer.Split(':');
                            int days = int.Parse(timerParts[0]);
                            int hours = int.Parse(timerParts[1]);
                            int minutes = int.Parse(timerParts[2]);
                            int seconds = int.Parse(timerParts[3]);
                            PlayerResources.AddCurrentResourceTime(ref PlayerResources.PlayerOxygen, days, hours, minutes, seconds);
                        }

                    }
                }
            }
        }
    }
}
