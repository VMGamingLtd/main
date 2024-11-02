using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown Dropdown;
    [SerializeField] WalletManager WalletManager;  

    private void Start()
    {
        // Add options to the dropdown
        Dropdown.options.Clear();
        Dropdown.options.Add(new TMP_Dropdown.OptionData("Friend 1"));

        // Set the default value of the dropdown
        Dropdown.value = 0;
        Dropdown.RefreshShownValue();
    }

    public void OnDropdownValueChanged()
    {
        WalletManager.UpdateRecipient(Dropdown.itemText.text);
    }
}
