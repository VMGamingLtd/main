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
            {"LevelInfo", tooltipObjects.LevelInfo},
            {"FriendsPanel", tooltipObjects.FriendsPanel},
            {"FibrousLeaves", tooltipObjects.FibrousLeaves},
            {"AlienWater", tooltipObjects.AlienWater},
            {"Biofuel", tooltipObjects.Biofuel},
            {"PurifiedWater", tooltipObjects.PurifiedWater},
            {"BatteryCore", tooltipObjects.BatteryCore},
            {"Battery", tooltipObjects.Battery},
            {"OxygenTank", tooltipObjects.OxygenTank},
            {"CraftBattery", tooltipObjects.CraftBattery},
            {"EngineeringSkill", tooltipObjects.EngineeringSkill}
        };
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

    public void DisplayTooltip(string objectName, PointerEventData eventData)
    {
        GameObject hoveredObject = eventData.pointerEnter;
        if (tooltipMap.TryGetValue(objectName, out GameObject tooltipObject))
        {
            tooltipObject.SetActive(true);
            ItemData itemData = hoveredObject.GetComponentInParent<ItemData>();
            if (itemData != null)
            {
                TextMeshProUGUI countdownText = tooltipObject.transform.Find("TopDisplay/Countdown/Value")?.GetComponent<TextMeshProUGUI>();
                if (countdownText != null)
                {
                    if (objectName == "OxygenTank")
                    {
                        string[] timerParts = itemData.OxygenTimer.Split(':');
                        string formattedTimer = "";

                        int days = int.Parse(timerParts[0]);
                        int hours = int.Parse(timerParts[1]);
                        int minutes = int.Parse(timerParts[2]);
                        int seconds = int.Parse(timerParts[3]);

                        if (days > 0)
                        {
                            formattedTimer += timerParts[0] + "d:";
                        }
                        if (hours > 0 || (days > 0 && hours == 0))
                        {
                            formattedTimer += timerParts[1] + "h:";
                        }
                        if (minutes > 0 || (days > 0 && hours == 0 && minutes == 0))
                        {
                            formattedTimer += timerParts[2] + "m:";
                        }
                        formattedTimer += timerParts[3] + "s";

                        if (days > 0 || hours > 0 || minutes > 0)
                        {
                            countdownText.text = "<color=yellow>" + formattedTimer + "</color>";
                        }
                        else
                        {
                            countdownText.text = formattedTimer;
                        }
                    }
                    else if (objectName == "Battery")
                    {
                        string[] timerParts = itemData.EnergyTimer.Split(':');
                        string formattedTimer = "";

                        int days = int.Parse(timerParts[0]);
                        int hours = int.Parse(timerParts[1]);
                        int minutes = int.Parse(timerParts[2]);
                        int seconds = int.Parse(timerParts[3]);

                        if (days > 0)
                        {
                            formattedTimer += timerParts[0] + "d:";
                        }
                        if (hours > 0 || (days > 0 && hours == 0))
                        {
                            formattedTimer += timerParts[1] + "h:";
                        }
                        if (minutes > 0 || (days > 0 && hours == 0 && minutes == 0))
                        {
                            formattedTimer += timerParts[2] + "m:";
                        }
                        formattedTimer += timerParts[3] + "s";

                        if (days > 0 || hours > 0 || minutes > 0)
                        {
                            countdownText.text = "<color=yellow>" + formattedTimer + "</color>";
                        }
                        else
                        {
                            countdownText.text = formattedTimer;
                        }
                    }
                }
            }
        }
    }

    public void HideTooltip(string objectName)
    {
        if (tooltipMap.TryGetValue(objectName, out GameObject tooltipObject))
        {
            tooltipObject.SetActive(false);
        }
    }
    public void HideAllTooltips()
    {
        foreach (var kvp in tooltipMap)
        {
            kvp.Value.SetActive(false);
        }
    }
}
