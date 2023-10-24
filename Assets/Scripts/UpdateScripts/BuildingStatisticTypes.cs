using BuildingManagement;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class BuildingStatisticTypes : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public BuildingManager buildingManager;
    public BuildingStatisticsManager buildingStatisticsManager;
    public TranslationManager translationManager;
    public static bool BuildingStatisticTypeChanged = false;

    void OnEnable()
    {
        dropdown.ClearOptions();
        string[] availableClasses = new string[0];
        int classIndex = 0;

        string allTypes = translationManager.Translate("ALLTYPES");
        dropdown.options.Add(new TMP_Dropdown.OptionData(allTypes));

        foreach (var kvp in buildingManager.buildingArrays)
        {
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                if (kvp.Key == "POWERPLANT")
                {
                    EnergyBuildingItemData itemDataEnergy = item.GetComponent<EnergyBuildingItemData>();
                    // Check if the item class is not already added to availableClasses
                    if (!Array.Exists(availableClasses, element => element == itemDataEnergy.buildingType))
                    {
                        // Resize the availableClasses array and add the item class
                        Array.Resize(ref availableClasses, availableClasses.Length + 1);
                        availableClasses[classIndex] = itemDataEnergy.buildingType;
                        classIndex++;
                    }
                }
                else
                {
                    BuildingItemData itemData = item.GetComponent<BuildingItemData>();
                    // Check if the item class is not already added to availableClasses
                    if (!Array.Exists(availableClasses, element => element == itemData.buildingType))
                    {
                        // Resize the availableClasses array and add the item class
                        Array.Resize(ref availableClasses, availableClasses.Length + 1);
                        availableClasses[classIndex] = itemData.buildingType;
                        classIndex++;
                    }
                }
            }
        }
        foreach (string buildingType in availableClasses)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate(buildingType)));
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
            BuildingStatisticsManager.BuildingStatisticType = matchedEntry.identifier;
        }
        else
        {
            Debug.LogError("BuildingStatisticTypes.cs 'matchedEntry' value is null");
        }
        BuildingStatisticTypeChanged = true;
        buildingStatisticsManager.LoadBuildingStatisticsAsync().Forget();
    }
}
