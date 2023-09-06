using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDropdownProducts : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public RecipeManager recipeManager;
    public GameObject buildingTypeDropdown;
    public GameObject itemTypeDropdown;
    public GameObject itemClassDropdown;

    private void Awake()
    {
        // Add options to the dropdown
        dropdown.options.Clear();
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("ALL")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("BASIC")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("PROCESSED")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("ENHANCED")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("ASSEMBLED")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("BUILDINGS")));

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
            case "ENHANCED":
                if (language == SystemLanguage.English)
                {
                    return "Enhanced";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Улучшенный";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "强化产品";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Upravený";
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
            case "BUILDINGS":
                if (language == SystemLanguage.English)
                {
                    return "Buildings";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Здания";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "建筑物";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Budovy";
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
                buildingTypeDropdown.SetActive(false);
                itemTypeDropdown.SetActive(true);
                itemClassDropdown.SetActive(true);
                break;
            case 1:
                RecipeManager.ShowRecipeProducts = "BASIC";
                buildingTypeDropdown.SetActive(false);
                itemTypeDropdown.SetActive(true);
                itemClassDropdown.SetActive(true);
                break;
            case 2:
                RecipeManager.ShowRecipeProducts = "PROCESSED";
                buildingTypeDropdown.SetActive(false);
                itemTypeDropdown.SetActive(true);
                itemClassDropdown.SetActive(true);
                break;
            case 3:
                RecipeManager.ShowRecipeProducts = "ENHANCED";
                buildingTypeDropdown.SetActive(false);
                itemTypeDropdown.SetActive(true);
                itemClassDropdown.SetActive(true);
                break;
            case 4:
                RecipeManager.ShowRecipeProducts = "ASSEMBLED";
                buildingTypeDropdown.SetActive(false);
                itemTypeDropdown.SetActive(true);
                itemClassDropdown.SetActive(true);
                break;
            case 5:
                RecipeManager.ShowRecipeProducts = "BUILDINGS";
                buildingTypeDropdown.SetActive(true);
                itemTypeDropdown.SetActive(false);
                itemClassDropdown.SetActive(false);
                break;
        }
        recipeManager.ShowFilteredItems();
    }
}
