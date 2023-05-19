using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static bool[] slotEquipped = new bool[9];

    public Image HelmetImage;
    public Image SuitImage;
    public Image ToolImage;
    public Image LeftHandImage;
    public Image BackpackImage;
    public Image RightHandImage;
    public Image DrillImage;
    public Image EnergyImage;
    public Image OxygenImage;

    public void InitStartEquip()
    {
        slotEquipped[0] = false;
        slotEquipped[1] = false;
        slotEquipped[2] = false;
        slotEquipped[3] = false;
        slotEquipped[4] = false;
        slotEquipped[5] = false;
        slotEquipped[6] = false;
        slotEquipped[7] = false;
        slotEquipped[8] = true;
    }

    public void CheckForEquip()
    {
        for (int i = 0; i < slotEquipped.Length; i++)
        {
            bool slotEquip = slotEquipped[i];

            switch (i)
            {
                case 0:
                    if (slotEquip)
                    {
                        Debug.Log("Slot at index 0 is equipped.");
                    }
                    break;

                case 1:
                    if (slotEquip)
                    {
                        Debug.Log("Slot at index 1 is equipped.");
                    }
                    break;

                case 2:
                    if (slotEquip)
                    {
                        Debug.Log("Slot at index 2 is equipped.");
                    }
                    break;

                case 3:
                    if (slotEquip)
                    {
                        Debug.Log("Slot at index 3 is equipped.");
                    }
                    break;

                case 4:
                    if (slotEquip)
                    {
                        Debug.Log("Slot at index 4 is equipped.");
                    }
                    break;

                case 5:
                    if (slotEquip)
                    {
                        Debug.Log("Slot at index 5 is equipped.");
                    }
                    break;

                case 6:
                    if (slotEquip)
                    {
                        Debug.Log("Slot at index 6 is equipped.");
                    }
                    break;

                case 7:
                    if (slotEquip)
                    {
                        Debug.Log("Slot at index 7 is equipped.");
                    }
                    break;

                case 8:
                    if (slotEquip)
                    {
                        string oxygenImage = OxygenImage.GetComponent<Image>().sprite.name;
                        if (oxygenImage == "OxygenTank")
                        {
                            // Perform actions for OxygenTank equipped
                        }
                        Debug.Log("Slot at index 8 is equipped.");
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
