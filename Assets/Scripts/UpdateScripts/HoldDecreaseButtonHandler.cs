using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingManagement;
using TMPro;
using UnityEngine.EventSystems;
public class HoldDecreaseButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private BuildingItemData itemData;
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
        float quantityBaseInput;
        if (itemData.itemType == "POWERPLANT")
        {
            int consumedSlots = itemData.consumedSlotCount;
            if (itemData.basePowerOutput == 99999)
            {
                quantityBaseInput = 2f;
            }
            else
            {
                quantityBaseInput = 0.5f;
            }
            while (isHoldingButton && itemData.efficiencySetting > 1)
            {
                itemData.efficiencySetting--;
                itemData.totalTime += 0.05f;
                itemData.powerOutput -= (int)(itemData.basePowerOutput / 100f);
                for (int i = 0; i < consumedSlots; i++)
                {
                    itemData.consumedItems[i].quantity -= quantityBaseInput / 100f;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
