using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDropdownProducts : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public RecipeManager recipeManager;

    private void Awake()
    {
        // Add options to the dropdown
        dropdown.options.Clear();
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("ALL")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("RAW")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("PROCESSED")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("REFINED")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("ASSEMBLED")));

        // Set the default value of the dropdown
        dropdown.value = 0;
        dropdown.RefreshShownValue();
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
                    return "All Products";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Все продукты";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "所有产品";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Všetky produkty";
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
                    return "Základný";
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
                    return "Spracovaný";
                }
                break;
            case "REFINED":
                if (language == SystemLanguage.English)
                {
                    return "Refined";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Отрефинированные";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "精制产品";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Opracovaný";
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
                    return "Zostavený";
                }
                break;
        }

        return key;
    }
    public void OnDropdownValueChanged()
    {
        int selectedValue = dropdown.value;
        switch (selectedValue)
        {
            case 0:
                RecipeManager.ShowRecipeProducts = "ALL";
                break;
            case 1:
                RecipeManager.ShowRecipeProducts = "BASIC";
                break;
            case 2:
                RecipeManager.ShowRecipeProducts = "PROCESSED";
                break;
            case 3:
                RecipeManager.ShowRecipeProducts = "REFINED";
                break;
            case 4:
                RecipeManager.ShowRecipeProducts = "ASSEMBLED";
                break;
        }
        recipeManager.ShowFilteredItems();
    }
}
