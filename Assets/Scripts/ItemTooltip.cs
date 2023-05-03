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
        Debug.Log(objectName);
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
        else if (objectName == "Water")
        {
            tooltipObjects.Water.SetActive(true);
        }
    }

    public void HideTooltip(string objectName)
    {
        if (objectName == "PlanetIndex")
        {
            tooltipObjects.PlanetIndex.SetActive(false);
        }
        else if (objectName == "Water")
        {
            tooltipObjects.Water.SetActive(false);
        }
    }
}
