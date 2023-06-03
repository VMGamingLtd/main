using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using RecipeManagement;

public class RecipeDropdownClasses : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public RecipeManager recipeManager;
    private string selectedOptionText = "All classes";

    void OnEnable()
    {
        selectedOptionText = dropdown.options[dropdown.value].text;
        dropdown.ClearOptions();
        string[] availableClasses = new string[0];
        int classIndex = 0;

        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("All classes")));

        // Search each itemRecipeArrays for item classes;
        foreach (var kvp in recipeManager.itemRecipeArrays)
        {
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                RecipeItemData itemData = item.GetComponent<RecipeItemData>();

                // Check if the item class is not already added to availableClasses
                if (!Array.Exists(availableClasses, element => element == itemData.itemClass))
                {
                    // Resize the availableClasses array and add the item class
                    Array.Resize(ref availableClasses, availableClasses.Length + 1);
                    availableClasses[classIndex] = itemData.itemClass;
                    classIndex++;
                }
            }
        }

        // Add each available item class as a dropdown option
        foreach (string itemClass in availableClasses)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString(itemClass)));
        }
        // Check if the previously selected option is still available
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
            case "All classes":
                if (language == SystemLanguage.English)
                {
                    return "All classes";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Все классы";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "所有等级";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Všetky triedy";
                }
                break;
            case "CLASS-F":
                if (language == SystemLanguage.English)
                {
                    return "Class-F";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Класс-F";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "等级-F";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Trieda-F";
                }
                break;
            case "CLASS-E":
                if (language == SystemLanguage.English)
                {
                    return "Class-E";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Класс-E";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "等级-E";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Trieda-E";
                }
                break;
            case "CLASS-D":
                if (language == SystemLanguage.English)
                {
                    return "Class-D";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Класс-D";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "等级-D";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Trieda-D";
                }
                break;
            case "CLASS-C":
                if (language == SystemLanguage.English)
                {
                    return "Class-C";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Класс-C";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "等级-C";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Trieda-C";
                }
                break;
            case "CLASS-B":
                if (language == SystemLanguage.English)
                {
                    return "Class-B";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Класс-B";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "等级-B";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Trieda-B";
                }
                break;
            case "CLASS-A":
                if (language == SystemLanguage.English)
                {
                    return "Class-A";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Класс-A";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "等级-A";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Trieda-A";
                }
                break;
            case "CLASS_S":
                if (language == SystemLanguage.English)
                {
                    return "Class-S";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Класс-S";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "等级-S";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Trieda-S";
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
            case "All classes":
            case "Все классы":
            case "所有等级":
            case "Všetky triedy":
                RecipeManager.ShowRecipeClass = "ALL";
                break;
            case "CLASS-F":
            case "Класс-F":
            case "等级-F":
            case "Trieda-F":
                RecipeManager.ShowRecipeClass = "CLASS-F";
                break;
            case "CLASS-E":
            case "Класс-E":
            case "等级-E":
            case "Trieda-E":
                RecipeManager.ShowRecipeClass = "CLASS-E";
                break;
            case "CLASS-D":
            case "Класс-D":
            case "等级-D":
            case "Trieda-D":
                RecipeManager.ShowRecipeClass = "CLASS-D";
                break;
            case "CLASS-C":
            case "Класс-C":
            case "等级-C":
            case "Trieda-C":
                RecipeManager.ShowRecipeClass = "CLASS-C";
                break;
            case "CLASS-B":
            case "Класс-B":
            case "等级-B":
            case "Trieda-B":
                RecipeManager.ShowRecipeClass = "CLASS-B";
                break;
            case "CLASS-A":
            case "Класс-A":
            case "等级-A":
            case "Trieda-A":
                RecipeManager.ShowRecipeClass = "CLASS-A";
                break;
            case "CLASS-S":
            case "Класс-S":
            case "等级-S":
            case "Trieda-S":
                RecipeManager.ShowRecipeClass = "CLASS-S";
                break;
        }

        recipeManager.ShowFilteredItems();
    }

}
