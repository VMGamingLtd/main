using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using ItemManagement;
using System.Transactions;

public class InventoryProducts : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public InventoryManager inventoryManager;
    public TranslationManager translationManager;

    void OnEnable()
    {
        dropdown.ClearOptions();
        string[] availableItems = new string[0];
        int classIndex = 0;

        string allItems = translationManager.Translate("ALLITEMS");
        dropdown.options.Add(new TMP_Dropdown.OptionData(allItems));

        foreach (var kvp in inventoryManager.itemArrays)
        {
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                ItemData itemData = item.GetComponent<ItemData>();

                // Check if the item class is not already added to availableProducts
                if (!Array.Exists(availableItems, element => element == itemData.itemProduct))
                {
                    // Resize the availableClasses array and add the item class
                    Array.Resize(ref availableItems, availableItems.Length + 1);
                    availableItems[classIndex] = itemData.itemProduct;
                    classIndex++;
                }
            }
        }
        // Add each available item product as a dropdown option
        foreach (string itemProduct in availableItems)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate(itemProduct)));
        }

        int selectedOptionIndex = Array.FindIndex(dropdown.options.ToArray(), option => option.text == allItems);
        if (selectedOptionIndex >= 0)
        {
            dropdown.value = selectedOptionIndex;
            dropdown.captionText.text = allItems;
        }
        else
        {
            dropdown.value = 0;
            allItems = dropdown.options[0].text;
            dropdown.captionText.text = allItems;
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
            InventoryManager.ShowItemProducts = matchedEntry.identifier;
        }
        else
        {
            Debug.LogError("InventoryProducts.cs 'matchedEntry' value is null");
        }
        inventoryManager.ShowFilteredItems();       
    }
}
