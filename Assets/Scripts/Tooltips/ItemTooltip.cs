using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using ItemManagement;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TooltipObjects tooltipObjects;
    private Dictionary<string, GameObject> tooltipMap;

    private void Start()
    {
        // Initialize the tooltip map
        tooltipMap = new Dictionary<string, GameObject>
        {
            {"PlanetIndex", tooltipObjects.PlanetIndex},
            {"Plants", tooltipObjects.Plants},
            {"Water", tooltipObjects.Water},
            {"Biofuel", tooltipObjects.Biofuel},
            {"WaterBottle", tooltipObjects.WaterBottle},
            {"Battery", tooltipObjects.Battery},
            {"OxygenTank", tooltipObjects.OxygenTank}
        };
    }

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
        if (tooltipMap.TryGetValue(objectName, out GameObject tooltipObject))
        {
            tooltipObject.SetActive(true);
            if (objectName == "OxygenTank")
            {
                ItemData itemData = tooltipObject.GetComponent<ItemData>();
                if (itemData != null)
                {
                    TextMeshProUGUI countdownText = tooltipObject.transform.Find("TopDisplay/Countdown/OxygenCountdownValue")?.GetComponent<TextMeshProUGUI>();
                    if (countdownText != null)
                    {
                        int oxygenTimer = itemData.OxygenTimer;
                        string timeFormat = FormatTime(oxygenTimer);
                        countdownText.text = timeFormat;
                    }
                }
            }
        }
    }
    private string FormatTime(int timeInSeconds)
    {
        int hours = timeInSeconds / 3600;
        int minutes = (timeInSeconds % 3600) / 60;
        int seconds = timeInSeconds % 60;

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

    public void HideTooltip(string objectName)
    {
        if (tooltipMap.TryGetValue(objectName, out GameObject tooltipObject))
        {
            tooltipObject.SetActive(false);
        }
    }
}
