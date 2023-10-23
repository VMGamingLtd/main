using BuildingManagement;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class HoldIncreaseButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private BuildingItemData itemData;
    private EnergyBuildingItemData itemDataEnergy;
    public BuildingOptionsInterface buildingOptionsInterface;
    private GameObject refObj;
    public TextMeshProUGUI efficiency;
    public TextMeshProUGUI totalTime;
    public TextMeshProUGUI powerOutput;
    public TextMeshProUGUI[] consumedQuantity;
    private bool isHoldingButton = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        isHoldingButton = true;
        StartCoroutine(RepeatedlyIncreaseEfficiency());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Stop the coroutine when the button is released.
        isHoldingButton = false;
    }
    private IEnumerator RepeatedlyIncreaseEfficiency()
    {
        refObj = buildingOptionsInterface.mainObj;
        itemData = refObj.GetComponent<BuildingItemData>();
        itemDataEnergy = refObj.GetComponent<EnergyBuildingItemData>();
        float quantityBaseInput;
        if (itemDataEnergy != null)
        {
            if (itemDataEnergy.basePowerOutput == 99999)
            {
                quantityBaseInput = 2f;
            }
            else
            {
                quantityBaseInput = 0.5f;
            }
            int consumedSlots = itemDataEnergy.consumedSlotCount;
            while (isHoldingButton && itemDataEnergy.efficiencySetting < 200)
            {
                for (int i = 0; i < consumedSlots; i++)
                {
                    itemDataEnergy.consumedItems[i].quantity += quantityBaseInput / 100f;
                }
                itemDataEnergy.efficiencySetting++;
                itemDataEnergy.totalTime -= 0.05f;
                itemDataEnergy.powerOutput += (int)(itemDataEnergy.basePowerOutput / 100f);
                yield return new WaitForSeconds(0.1f); // Repeat every 1 second (adjust as needed).
            }
        }
    }
}
