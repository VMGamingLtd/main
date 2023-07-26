using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingManagement;
using TMPro;
using UnityEngine.EventSystems;
public class HoldIncreaseButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
        while (isHoldingButton && itemData.efficiencySetting < 200)
        {
            itemData.efficiencySetting++;
            itemData.totalTime -= 0.1f;
            itemData.powerOutput += (int)(itemData.basePowerOutput / 100f);
            yield return new WaitForSeconds(0.1f); // Repeat every 1 second (adjust as needed).
        }
    }
}
