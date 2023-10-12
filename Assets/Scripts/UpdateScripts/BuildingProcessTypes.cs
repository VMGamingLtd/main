using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BuildingManagement;
public class BuildingProcessTypes : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public BuildingManager buildingManager;
    public List<string> options = new() { "Electricity", "Production"};
    private string selectedOptionText;
    private TranslationManager translationManager;

    void Awake()
    {
        translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
    }
    private void OnEnable()
    {
        dropdown.ClearOptions(); // Clear existing options

        // Add the options from the list
        dropdown.AddOptions(options);

        int selectedOptionIndex = options.FindIndex(option => option == translationManager.Translate(selectedOptionText));
        if (selectedOptionIndex >= 0)
        {
            dropdown.value = selectedOptionIndex;
            dropdown.captionText.text = translationManager.Translate(selectedOptionText);
        }
        else
        {
            dropdown.value = 0;
            selectedOptionText = options[0];
            dropdown.captionText.text = translationManager.Translate(selectedOptionText);
        }
    }

    private string GetLocalizedString(string key)
    {
        // Get the system language
        SystemLanguage language = Application.systemLanguage;

        // Replace the localized strings with actual translations based on the system language
        switch (key)
        {
            case "Electricity":
                if (language == SystemLanguage.English)
                {
                    return "Electricity";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Электричество";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "电力";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Elektrina";
                }
                break;
            case "Production":
                if (language == SystemLanguage.English)
                {
                    return "Production";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Производство";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "生产";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Výroba";
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
            case "Electricity":
            case "Электричество":
            case "电力":
            case "Elektrina":
                BuildingStatisticsManager.BuildingStatisticInterval = "Electricity";
                break;
            case "Production":
            case "Производство":
            case "生产":
            case "Výroba":
                BuildingStatisticsManager.BuildingStatisticInterval = "Production";
                break;
        }
        BuildingIntervalTypes.BuildingIntervalTypeChanged = true;
    }
}
