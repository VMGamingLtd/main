using TMPro;
using System;
using UnityEngine;
using RecipeManagement;
public class BuildingBlueprintTypes : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public RecipeManager recipeManager;
    public TranslationManager translationManager;

    void OnEnable()
    {
        dropdown.ClearOptions();
        string[] availableClasses = new string[0];
        int classIndex = 0;
        string allTypes = translationManager.Translate("ALLTYPES");
        dropdown.options.Add(new TMP_Dropdown.OptionData(allTypes));

        foreach (var kvp in recipeManager.itemRecipeArrays)
        {
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                RecipeItemData itemData = item.GetComponent<RecipeItemData>();

                // Check if the item class is not already added to availableClasses
                if (itemData.recipeProduct == "BUILDING" && !Array.Exists(availableClasses, element => element == itemData.recipeType))
                {
                    // Resize the availableClasses array and add the item class
                    Array.Resize(ref availableClasses, availableClasses.Length + 1);
                    availableClasses[classIndex] = itemData.recipeType;
                    classIndex++;
                }
            }
        }
        foreach (string itemType in availableClasses)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate(itemType)));
        }

        int selectedOptionIndex = Array.FindIndex(dropdown.options.ToArray(), option => option.text == allTypes);
        if (selectedOptionIndex >= 0)
        {
            dropdown.value = selectedOptionIndex;
            dropdown.captionText.text = allTypes;
        }
        else
        {
            dropdown.value = 0;
            allTypes = dropdown.options[0].text;
            dropdown.captionText.text = allTypes;
        }
    }
    public void OnDropdownValueChanged()
    {
        TMP_Dropdown.OptionData selectedOption = dropdown.options[dropdown.value];
        string optionText = selectedOption.text;
        CoreTranslations matchedEntry = translationManager.FindEntryBySubstring(optionText);
        if (matchedEntry != null)
        {
            // Set InventoryManager.ShowItemTypes to the value of matchedEntry.identifier
            RecipeManager.ShowRecipeTypes = matchedEntry.identifier;
        }
        else
        {
            Debug.LogError("BuildingBlueprintTypes.cs 'matchedEntry' value is null");
        }
        recipeManager.ShowFilteredItems();
    }
}
