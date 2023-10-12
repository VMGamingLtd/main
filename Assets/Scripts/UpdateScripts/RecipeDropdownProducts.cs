using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDropdownProducts : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    public RecipeManager recipeManager;
    public TranslationManager translationManager;
    public GameObject buildingTypeDropdown;
    public GameObject itemTypeDropdown;
    public GameObject itemClassDropdown;

    private void Awake()
    {
        // Add options to the dropdown
        dropdown.options.Clear();
        dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate("ALLBLUEPRINTS")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate("BASIC")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate("PROCESSED")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate("ENHANCED")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate("ASSEMBLED")));
        dropdown.options.Add(new TMP_Dropdown.OptionData(translationManager.Translate("BUILDINGS")));

        // Set the default value of the dropdown
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    public void OnDropdownValueChanged()
    {
        int selectedValue = dropdown.value;
        switch (selectedValue)
        {
            case 0:
                RecipeManager.ShowRecipeProducts = "ALLBLUEPRINTS";
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
