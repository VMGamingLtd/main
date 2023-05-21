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
                    Transform targetChild = EnergySlot.transform.Cast<Transform>()
                        .FirstOrDefault(child => child.name.Contains("(Clone)"));
                    if (targetChild != null && targetChild.name == "Battery(Clone)")
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
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
                    Transform targetChild = OxygenSlot.transform.Cast<Transform>()
                        .FirstOrDefault(child => child.name.Contains("(Clone)"));
                    if (targetChild != null && targetChild.name == "OxygenTank(Clone)")
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
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
    public void ReduceEnergyTimer()
    {
        foreach (Transform child in EnergySlot.transform)
        {
            if (child.name.Contains("(Clone)"))
            {
                ItemData itemData = child.GetComponent<ItemData>();
                if (itemData != null)
                {
                    string energyTimer = itemData.EnergyTimer;
                    string[] timerParts = energyTimer.Split(':');
                    int days = int.Parse(timerParts[0]);
                    int hours = int.Parse(timerParts[1]);
                    int minutes = int.Parse(timerParts[2]);
                    int seconds = int.Parse(timerParts[3]);
                    seconds--; // Reduce the energyTimer by 1 second
                    if (seconds < 0)
                    {
                        seconds = 59;
                        minutes--;
                        if (minutes < 0)
                        {
                            minutes = 59;
                            hours--;
                            if (hours < 0)
                            {
                                hours = 23;
                                days--;
                                if (days < 0)
                                {
                                    days = 0; // or handle negative days accordingly
                                }
                            }
                        }
                    }
                    energyTimer = $"{days.ToString("00")}:{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";
                    itemData.EnergyTimer = energyTimer;
                }
            }
        }
    }


}
