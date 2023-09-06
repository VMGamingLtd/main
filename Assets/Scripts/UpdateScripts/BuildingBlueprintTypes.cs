using TMPro;
using System;
using UnityEngine;
using RecipeManagement;
public class BuildingBlueprintTypes : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public RecipeManager recipeManager;
    private string selectedOptionText = "All types";

    void OnEnable()
    {
        selectedOptionText = dropdown.options[dropdown.value].text;
        dropdown.ClearOptions();
        string[] availableClasses = new string[0];
        int classIndex = 0;

        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("ALL")));

        foreach (var kvp in recipeManager.itemRecipeArrays)
        {
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                RecipeItemData itemData = item.GetComponent<RecipeItemData>();

                // Check if the item class is not already added to availableClasses
                if (itemData.itemProduct == "BUILDING" && !Array.Exists(availableClasses, element => element == itemData.itemType))
                {
                    // Resize the availableClasses array and add the item class
                    Array.Resize(ref availableClasses, availableClasses.Length + 1);
                    availableClasses[classIndex] = itemData.itemType;
                    classIndex++;
                }
            }
        }
        foreach (string itemType in availableClasses)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString(itemType)));
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
                    return "All Types";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Все типы";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "所有类型";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Všetky typy";
                }
                break;
            case "AGRICULTURE":
                if (language == SystemLanguage.English)
                {
                    return "Agriculture";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Cельское хозяйство";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "农业设施";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Poľnohospodárstvo";
                }
                break;
            case "PUMPINGFACILITY":
                if (language == SystemLanguage.English)
                {
                    return "Pumping facility";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Насосная установка";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "抽水设施";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Čerpacie zariadenie";
                }
                break;
            case "FACTORY":
                if (language == SystemLanguage.English)
                {
                    return "Factory";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Фабрика";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "工厂";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Fabrika";
                }
                break;
            case "COMMFACILITY":
                if (language == SystemLanguage.English)
                {
                    return "Comm facility";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Средства связи";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "通讯设施";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Komunikačné zariadenie";
                }
                break;
            case "STORAGEHOUSE":
                if (language == SystemLanguage.English)
                {
                    return "Storage house";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Складское помещение";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "储藏室";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Skladový dom";
                }
                break;
            case "NAVALFACILITY":
                if (language == SystemLanguage.English)
                {
                    return "Naval facility";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Военно-морской объект";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "海军设施";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Námorné zariadenie";
                }
                break;
            case "OXYGENFACILITY":
                if (language == SystemLanguage.English)
                {
                    return "Oxygen facility";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Кислородная установка";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "制氧设施";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Kyslíkové zariadenie";
                }
                break;
            case "AVIATIONFACILITY":
                if (language == SystemLanguage.English)
                {
                    return "Aviation facility";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Авиационный объект";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "航空设施";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Letecké zariadenie";
                }
                break;
            case "HEATINGFACILITY":
                if (language == SystemLanguage.English)
                {
                    return "Heating facility";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Отопительная установка";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "供暖设施";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Vykurovacie zariadenie";
                }
                break;
            case "COOLINGFACILITY":
                if (language == SystemLanguage.English)
                {
                    return "Cooling facility";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Охлаждающая установка";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "冷却设施";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Chladiace zariadenie";
                }
                break;
            case "POWERPLANT":
                if (language == SystemLanguage.English)
                {
                    return "Powerplant";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Электростанция";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "发电厂";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Elektráreň";
                }
                break;
            case "OXYGENSTATION":
                if (language == SystemLanguage.English)
                {
                    return "Oxygen station";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Кислородная станция";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "氧气站";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Kyslíková stanica";
                }
                break;
            case "MININGRIG":
                if (language == SystemLanguage.English)
                {
                    return "Mining rig";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Горная установка";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "采矿设施";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Ťažobné zariadenie";
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
            case "All Types":
            case "Все типы":
            case "所有类型":
            case "Všetky typy":
                RecipeManager.ShowRecipeProducts = "ALL";
                break;
            case "Agriculture":
            case "Cельское хозяйство":
            case "农业设施":
            case "Poľnohospodárstvo":
                RecipeManager.ShowRecipeProducts = "AGRICULTURE";
                break;
            case "Pumping facility":
            case "Насосная установка":
            case "抽水设施":
            case "Čerpacie zariadenie":
                RecipeManager.ShowRecipeProducts = "PUMPINGFACILITY";
                break;
            case "Factory":
            case "Фабрика":
            case "工厂":
            case "Fabrika":
                RecipeManager.ShowRecipeProducts = "FACTORY";
                break;
            case "Comm facility":
            case "Средства связи":
            case "金属":
            case "Komunikačné zariadenie":
                RecipeManager.ShowRecipeProducts = "COMMFACILITY";
                break;
            case "Storage house":
            case "Складское помещение":
            case "储藏室":
            case "Skladový dom":
                RecipeManager.ShowRecipeProducts = "STORAGEHOUSE";
                break;
            case "Naval facility":
            case "Военно-морской объект":
            case "海军设施":
            case "Námorné zariadenie":
                RecipeManager.ShowRecipeProducts = "NAVALFACILITY";
                break;
            case "Oxygen facility":
            case "Кислородная установка":
            case "制氧设施":
            case "Kyslíkové zariadenie":
                RecipeManager.ShowRecipeProducts = "OXYGENFACILITY";
                break;
            case "Aviation facility":
            case "Авиационный объект":
            case "航空设施":
            case "Letecké zariadenie":
                RecipeManager.ShowRecipeProducts = "AVIATIONFACILITY";
                break;
            case "Heating facility":
            case "Отопительная установка":
            case "供暖设施":
            case "Vykurovacie zariadenie":
                RecipeManager.ShowRecipeProducts = "HEATINGFACILITY";
                break;
            case "Cooling facility":
            case "Охлаждающая установка":
            case "冷却设施":
            case "Chladiace zariadenie":
                RecipeManager.ShowRecipeProducts = "COOLINGFACILITY";
                break;
            case "Powerplant":
            case "Электростанция":
            case "发电厂":
            case "Elektráreň":
                RecipeManager.ShowRecipeProducts = "POWERPLANT";
                break;
            case "Oxygen station":
            case "Кислород":
            case "氧气站":
            case "Kyslíková stanica":
                RecipeManager.ShowRecipeProducts = "OXYGENSTATION";
                break;
            case "Mining rig":
            case "Кислородная станция":
            case "采矿设施":
            case "Ťažobné zariadenie":
                RecipeManager.ShowRecipeProducts = "MININGRIG";
                break;
        }
        recipeManager.ShowFilteredItems();
    }
}
