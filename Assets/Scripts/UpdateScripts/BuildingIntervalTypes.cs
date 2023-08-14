using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BuildingManagement;
public class BuildingIntervalTypes : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public BuildingManager buildingManager;
    public List<string> options = new List<string>{"1 second", "10 seconds", "30 seconds", "1 minute", "10 minutes", "30 minutes", "1 hour", "6 hours"};
    private string selectedOptionText;
    public static bool BuildingIntervalTypeChanged = false;

    private void OnEnable()
    {
        dropdown.ClearOptions(); // Clear existing options

        // Add the options from the list
        dropdown.AddOptions(options);

        int selectedOptionIndex = options.FindIndex(option => option == selectedOptionText);
        if (selectedOptionIndex >= 0)
        {
            dropdown.value = selectedOptionIndex;
            dropdown.captionText.text = selectedOptionText;
        }
        else
        {
            dropdown.value = 0;
            selectedOptionText = options[0];
            dropdown.captionText.text = selectedOptionText;
        }
    }

    private string GetLocalizedString(string key)
    {
        // Get the system language
        SystemLanguage language = Application.systemLanguage;

        // Replace the localized strings with actual translations based on the system language
        switch (key)
        {
            case "1 second":
                if (language == SystemLanguage.English)
                {
                    return "1 second";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "1 секунда";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "1秒";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "1 sekunda";
                }
                break;
            case "10 seconds":
                if (language == SystemLanguage.English)
                {
                    return "10 seconds";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "10 секунд";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "10秒";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "10 sekúnd";
                }
                break;
            case "30 seconds":
                if (language == SystemLanguage.English)
                {
                    return "30 seconds";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "30 секунд";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "30秒";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "30 sekúnd";
                }
                break;
            case "1 minute":
                if (language == SystemLanguage.English)
                {
                    return "1 minute";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "1 минута";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "1分钟";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "1 minúta";
                }
                break;
            case "10 minutes":
                if (language == SystemLanguage.English)
                {
                    return "10 minutes";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "10 минут";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "10分钟";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "10 minút";
                }
                break;
            case "30 minutes":
                if (language == SystemLanguage.English)
                {
                    return "30 minutes";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "30 минут";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "30分钟";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "30 minút";
                }
                break;
            case "1 hour":
                if (language == SystemLanguage.English)
                {
                    return "1 hour";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "1 час";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "1小时";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "1 hodina";
                }
                break;
            case "6 hours":
                if (language == SystemLanguage.English)
                {
                    return "6 hours";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "6 часов";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "6个小时";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "6 hodín";
                }
                break;
        }

        return key;
    }
    public void OnDropdownValueChanged()
    {
        TMP_Dropdown.OptionData selectedOption = dropdown.options[dropdown.value];

        string optionText = selectedOption.text;

        switch (optionText)
        {
            case "1 second":
            case "1 секунда":
            case "1秒":
            case "1 sekunda":
                BuildingStatisticsManager.BuildingStatisticInterval = "1 second";
                break;
            case "10 seconds":
            case "10 секунд":
            case "10秒":
            case "10 sekúnd":
                BuildingStatisticsManager.BuildingStatisticInterval = "10 seconds";
                break;
            case "30 seconds":
            case "30 секунд":
            case "30秒":
            case "30 sekúnd":
                BuildingStatisticsManager.BuildingStatisticInterval = "30 seconds";
                break;
            case "1 minute":
            case "1 минута":
            case "1分钟":
            case "1 minúta":
                BuildingStatisticsManager.BuildingStatisticInterval = "1 minute";
                break;
            case "10 minutes":
            case "10 минут":
            case "10分钟":
            case "10 minút":
                BuildingStatisticsManager.BuildingStatisticInterval = "10 minutes";
                break;
            case "30 minutes":
            case "30 минут":
            case "30分钟":
            case "30 minút":
                BuildingStatisticsManager.BuildingStatisticInterval = "30 minutes";
                break;
            case "1 hour":
            case "1 час":
            case "1小时":
            case "1 hodina":
                BuildingStatisticsManager.BuildingStatisticInterval = "1 hour";
                break;
            case "6 hours":
            case "6 часов":
            case "6个小时":
            case "6 hodín":
                BuildingStatisticsManager.BuildingStatisticInterval = "6 hours";
                break;
        }
        BuildingIntervalTypes.BuildingIntervalTypeChanged = true;
    }
}
