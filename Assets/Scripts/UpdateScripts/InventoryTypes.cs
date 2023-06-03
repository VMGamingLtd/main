using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using ItemManagement;
public class InventoryTypes : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public InventoryManager inventoryManager;
    private string selectedOptionText = "All types";

    void OnEnable()
    {
        selectedOptionText = dropdown.options[dropdown.value].text;
        dropdown.ClearOptions();
        string[] availableClasses = new string[0];
        int classIndex = 0;

        dropdown.options.Add(new TMP_Dropdown.OptionData(GetLocalizedString("ALL")));

        foreach (var kvp in inventoryManager.itemArrays)
        {
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                ItemData itemData = item.GetComponent<ItemData>();

                // Check if the item class is not already added to availableClasses
                if (!Array.Exists(availableClasses, element => element == itemData.itemType))
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
            case "PLANTS":
                if (language == SystemLanguage.English)
                {
                    return "Plants";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Растения";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "植物";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Rastliny";
                }
                break;
            case "LIQUID":
                if (language == SystemLanguage.English)
                {
                    return "Liquid";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Жидкость";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "液体";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Tekutina";
                }
                break;
            case "MINERALS":
                if (language == SystemLanguage.English)
                {
                    return "Minerals";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Минералы";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "矿物质";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Minerály";
                }
                break;
            case "METALS":
                if (language == SystemLanguage.English)
                {
                    return "Metals";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Металлы";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "金属";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Kovy";
                }
                break;
            case "FOAM":
                if (language == SystemLanguage.English)
                {
                    return "Foam";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Пена";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "泡沫";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Pena";
                }
                break;
            case "POWDERS":
                if (language == SystemLanguage.English)
                {
                    return "Powders";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Порошки";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "粉末";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Prášky";
                }
                break;
            case "FLESH":
                if (language == SystemLanguage.English)
                {
                    return "Flesh";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Плоть";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "肉体";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Mäso";
                }
                break;
            case "WEAPON":
                if (language == SystemLanguage.English)
                {
                    return "Weapon";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Оружие";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "武器";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Zbraň";
                }
                break;
            case "ARMOR":
                if (language == SystemLanguage.English)
                {
                    return "Armor";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Броня";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "盔甲";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Zbroj";
                }
                break;
            case "ENERGY":
                if (language == SystemLanguage.English)
                {
                    return "Energy";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Энергия";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "能源";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Energia";
                }
                break;
            case "OXYGEN":
                if (language == SystemLanguage.English)
                {
                    return "Oxygen";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Кислород";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "氧气";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Kyslík";
                }
                break;
            case "MINING":
                if (language == SystemLanguage.English)
                {
                    return "Mining";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Добыча";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "采矿";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Ťažba";
                }
                break;
            case "BAG":
                if (language == SystemLanguage.English)
                {
                    return "Bag";
                }
                else if (language == SystemLanguage.Russian)
                {
                    return "Сумка";
                }
                else if (language == SystemLanguage.Chinese)
                {
                    return "袋子";
                }
                else if (language == SystemLanguage.Slovak)
                {
                    return "Taška";
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
                InventoryManager.ShowItemTypes = "ALL";
                break;
            case "Plants":
            case "Растения":
            case "植物":
            case "Rastliny":
                InventoryManager.ShowItemTypes = "PLANTS";
                break;
            case "Liquid":
            case "Жидкость":
            case "液体":
            case "Tekutina":
                InventoryManager.ShowItemTypes = "LIQUID";
                break;
            case "Minerals":
            case "Минералы":
            case "矿物质":
            case "Minerály":
                InventoryManager.ShowItemTypes = "MINERALS";
                break;
            case "Metals":
            case "Металлы":
            case "金属":
            case "Kovy":
                InventoryManager.ShowItemTypes = "METALS";
                break;
            case "Foam":
            case "Пена":
            case "泡沫":
            case "Pena":
                InventoryManager.ShowItemTypes = "FOAM";
                break;
            case "Powders":
            case "Порошки":
            case "粉末":
            case "Prášky":
                InventoryManager.ShowItemTypes = "POWDERS";
                break;
            case "Flesh":
            case "Плоть":
            case "肉体":
            case "Mäso":
                InventoryManager.ShowItemTypes = "FLESH";
                break;
            case "Weapon":
            case "Оружие":
            case "武器":
            case "Zbraň":
                InventoryManager.ShowItemTypes = "WEAPON";
                break;
            case "Armor":
            case "Броня":
            case "盔甲":
            case "Zbroj":
                InventoryManager.ShowItemTypes = "ARMOR";
                break;
            case "Energy":
            case "Энергия":
            case "能源":
            case "Energia":
                InventoryManager.ShowItemTypes = "ENERGY";
                break;
            case "Oxygen":
            case "Кислород":
            case "氧气":
            case "Kyslík":
                InventoryManager.ShowItemTypes = "OXYGEN";
                break;
            case "Mining":
            case "Добыча":
            case "采矿":
            case "Ťažba":
                InventoryManager.ShowItemTypes = "MINING";
                break;
            case "Bag":
            case "Сумка":
            case "袋子":
            case "Taška":
                InventoryManager.ShowItemTypes = "BAG";
                break;
        }
        inventoryManager.ShowFilteredItems();
    }
}
