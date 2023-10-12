using TMPro;
using System;
using UnityEngine;
using ItemManagement;
public class InventoryTypes : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public InventoryManager inventoryManager;
    public TranslationManager translationManager;

    void OnEnable()
    {
        dropdown.ClearOptions();
        string[] availableClasses = new string[0];
        int classIndex = 0;
        string allTypes = translationManager.Translate("ALLTYPES");
        dropdown.options.Add(new TMP_Dropdown.OptionData(allTypes));

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
            InventoryManager.ShowItemTypes = matchedEntry.identifier;
        }
        else
        {
            Debug.LogError("InventoryTypes.cs 'matchedEntry' value is null");
        }
        inventoryManager.ShowFilteredItems();
    }
}
