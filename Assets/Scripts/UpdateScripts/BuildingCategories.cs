using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using BuildingManagement;

public class BuildingCategories : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public BuildingManager buildingManager;
    private string selectedOptionText = "All buildings";

    void OnEnable()
    {
        selectedOptionText = dropdown.options[dropdown.value].text;
        dropdown.ClearOptions();
        string[] availableClasses = new string[0];
        int classIndex = 0;

        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("ALL")));

        foreach (var kvp in buildingManager.buildingArrays)
        {
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                BuildingItemData itemData = item.GetComponent<BuildingItemData>();

                // Check if the item class is not already added to availableClasses
                if (!Array.Exists(availableClasses, element => element == itemData.itemProduct))
                {
                    // Resize the availableClasses array and add the item class
                    Array.Resize(ref availableClasses, availableClasses.Length + 1);
                    availableClasses[classIndex] = itemData.itemProduct;
                    classIndex++;
                }
            }
        }
        // Add each available item class as a dropdown option
        foreach (string itemProduct in availableClasses)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString(itemProduct)));
        }

        int selectedOptionIndex = Array.FindIndex(dropdown.options.ToArray(), option => option.text == selectedOptionText);
        if (selectedOptionIndex >= 0)
        {
            dropdown.value = selectedOptionIndex;
            dropdown.captionText.text = selectedOptionText;
        }
        else
        {
            dropdown.value = 0;
            selectedOptionText = dropdown.options[0].text;
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
            case "ALL":
                if (language == SystemLanguage.English)
                {
                    return "All items";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Все детали";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "所有项目";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Všetky veci";
                }
                break;
            case "BASIC":
                if (language == SystemLanguage.English)
                {
                    return "Basic";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Основные";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "基础产品";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Základné";
                }
                break;
            case "PROCESSED":
                if (language == SystemLanguage.English)
                {
                    return "Processed";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Обработанные";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "处理过的产品";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Spracované";
                }
                break;
            case "ENHANCED":
                if (language == SystemLanguage.English)
                {
                    return "Enhanced";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Улучшенные";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "强化产品";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Upravené";
                }
                break;
            case "ASSEMBLED":
                if (language == SystemLanguage.English)
                {
                    return "Assembled";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Собранные";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "组装产品";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Zostavené";
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
            case "All buildings":
            case "Все детали":
            case "所有项目":
            case "Všetky veci":
                BuildingManager.ShowBuildingCategories = "ALL";
                break;
            case "BASIC":
            case "Основные":
            case "基础产品":
            case "Základné":
                BuildingManager.ShowBuildingCategories = "BASIC";
                break;
            case "Processed":
            case "Обработанные":
            case "处理过的产品":
            case "Spracované":
                BuildingManager.ShowBuildingCategories = "PROCESSED";
                break;
            case "Enhanced":
            case "Улучшенные":
            case "强化产品":
            case "Upravené":
                BuildingManager.ShowBuildingCategories = "ENHANCED";
                break;
            case "Assembled":
            case "Собранные":
            case "组装产品":
            case "Zostavené":
                BuildingManager.ShowBuildingCategories = "ASSEMBLED";
                break;
        }
        buildingManager.ShowFilteredItems();
    }
}
