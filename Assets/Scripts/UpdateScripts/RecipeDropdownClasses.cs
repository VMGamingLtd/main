using RecipeManagement;
using System;
using TMPro;
using UnityEngine;

public class RecipeDropdownClasses : MonoBehaviour
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

        string allClasses = translationManager.Translate("ALLCLASSES");
        dropdown.options.Add(new TMP_Dropdown.OptionData(allClasses));

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
            dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate(itemClass)));
        }
        // Check if the previously selected option is still available
        int selectedOptionIndex = Array.FindIndex(dropdown.options.ToArray(), option => option.text == allClasses);
        if (selectedOptionIndex >= 0)
        {
            dropdown.value = selectedOptionIndex;
            dropdown.captionText.text = allClasses;
        }
        else
        {
            dropdown.value = 0;
            allClasses = dropdown.options[0].text;
            dropdown.captionText.text = allClasses;
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
            RecipeManager.ShowRecipeClass = matchedEntry.identifier;
        }
        else
        {
            Debug.LogError("RecipeDropdownClasses.cs 'matchedEntry' value is null");
        }
        recipeManager.ShowFilteredItems();
    }
}
