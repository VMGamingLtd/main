using BuildingManagement;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class HoldDecreaseButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private BuildingItemData itemData;
    private EnergyBuildingItemData itemDataEnergy;
    public BuildingOptionsInterface buildingOptionsInterface;
    private GameObject refObj;
    public TextMeshProUGUI efficiency;
    public TextMeshProUGUI totalTime;
    public TextMeshProUGUI powerOutput;
    private bool isHoldingButton = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        isHoldingButton = true;
        StartCoroutine(RepeatedlyDecreaseEfficiency());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Stop the coroutine when the button is released.
        isHoldingButton = false;
    }
    private IEnumerator RepeatedlyDecreaseEfficiency()
    {
        refObj = buildingOptionsInterface.mainObj;
        itemData = refObj.GetComponent<BuildingItemData>();
        itemDataEnergy = refObj.GetComponent<EnergyBuildingItemData>();
        float quantityBaseInput;
        if (itemDataEnergy != null)
        {
            int consumedSlots = itemDataEnergy.consumedSlotCount;
            if (itemDataEnergy.basePowerOutput == 99999)
            {
                quantityBaseInput = 2f;
            }
            else
            {
                quantityBaseInput = 0.5f;
            }
            while (isHoldingButton && itemDataEnergy.efficiencySetting > 1)
            {
                itemDataEnergy.efficiencySetting--;
                itemDataEnergy.totalTime += 0.05f;
                itemDataEnergy.powerOutput -= (int)(itemDataEnergy.basePowerOutput / 100f);
                for (int i = 0; i < consumedSlots; i++)
                {
                    itemDataEnergy.consumedItems[i].quantity -= quantityBaseInput / 100f;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
