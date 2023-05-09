using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TooltipObjects tooltipObjects;

    public void OnPointerEnter(PointerEventData eventData)
    {

        string objectName = eventData.pointerEnter.transform.name;
        DisplayTooltip(objectName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        string objectName = eventData.pointerEnter.transform.name;
        HideTooltip(objectName);
    }

    public void DisplayTooltip(string objectName)
    {
        if (objectName == "PlanetIndex")
        {
            tooltipObjects.PlanetIndex.SetActive(true);
        }
        else if (objectName == "Plants")
        {
            tooltipObjects.Plants.SetActive(true);
        }
        else if (objectName == "Water")
        {
            tooltipObjects.Water.SetActive(true);
        }
        else if (objectName == "Biofuel")
        {
            tooltipObjects.Biofuel.SetActive(true);
        }
        else if (objectName == "WaterBottle")
        {
            tooltipObjects.WaterBottle.SetActive(true);
        }
        else if (objectName == "Battery")
        {
            tooltipObjects.Battery.SetActive(true);
        }
        else if (objectName == "OxygenTank")
        {
            tooltipObjects.OxygenTank.SetActive(true);
        }
    }

    public void HideTooltip(string objectName)
    {
        if (objectName == "PlanetIndex")
        {
            tooltipObjects.PlanetIndex.SetActive(false);
        }
        else if (objectName == "Plants")
        {
            tooltipObjects.Plants.SetActive(false);
        }
        else if (objectName == "Water")
        {
            tooltipObjects.Water.SetActive(false);
        }
        else if (objectName == "Biofuel")
        {
            tooltipObjects.Biofuel.SetActive(false);
        }
        else if (objectName == "WaterBottle")
        {
            tooltipObjects.WaterBottle.SetActive(false);
        }
        else if (objectName == "Battery")
        {
            tooltipObjects.Battery.SetActive(false);
        }
        else if (objectName == "OxygenTank")
        {
            tooltipObjects.OxygenTank.SetActive(false);
        }
    }
}
