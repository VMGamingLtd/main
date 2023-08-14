using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TooltipObjects tooltipObjects;

    private void Awake()
    {
        GameObject tooltipObjectsGO = GameObject.Find("TooltipCanvas/TooltipObjects");
        tooltipObjects = tooltipObjectsGO.GetComponent<TooltipObjects>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string objectName = eventData.pointerEnter.transform.name;
        DisplayTooltip(objectName, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData != null)
        {
            string objectName = eventData.pointerEnter.transform.name;
            HideTooltip(objectName);
        }
        else
        {
            HideAllTooltips();
        }
    }
    private GameObject FindTooltipObject(string objectName)
    {
        foreach (GameObject tooltipObj in tooltipObjects.tooltipObjects)
        {
            if (tooltipObj.name == objectName)
            {
                return tooltipObj;
            }
        }

        return null;
    }

    public void DisplayTooltip(string objectName, PointerEventData eventData)
    {
        GameObject hoveredObject = eventData.pointerEnter;
        GameObject tooltipObject = FindTooltipObject(objectName);

        if (tooltipObject != null)
        {
            tooltipObject.SetActive(true);
        }
    }

    public void HideTooltip(string objectName)
    {
        GameObject tooltipObject = FindTooltipObject(objectName);

        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false);
        }
    }
    public void HideAllTooltips()
    {
        foreach (GameObject tooltipObject in tooltipObjects.tooltipObjects)
        {
            tooltipObject.SetActive(false);
        }
    }
}
