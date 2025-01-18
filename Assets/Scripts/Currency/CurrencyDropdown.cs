using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown Dropdown;
    [SerializeField] WalletManager WalletManager;  

    private static int MAX_DROPDOWN_LIST_ITEWMS_COUNT = 10;


    /*
    private void Start()
    {
        // Add options to the dropdown
        Dropdown.options.Clear();
        Dropdown.options.Add(new TMP_Dropdown.OptionData("Friend 1"));

        // Set the default value of the dropdown
        Dropdown.value = 0;
        Dropdown.RefreshShownValue();
    }
    */

    public void OnEnable()
    {
        Init().Forget();
    }

    private async UniTaskVoid Init()
    {
        Gaos.Routes.Model.FriendJson.GetMyFriendsResponse response = await Gaos.Friends.GetMyFriends.CallAsync("", MAX_DROPDOWN_LIST_ITEWMS_COUNT);
        Dropdown.options.Clear();
        for (int i = 0; i < response.Users.Length; i++)
        {
            Dropdown.options.Add(new TMP_Dropdown.OptionData(response.Users[i].UserName));
        }

        Dropdown.value = 0;
        Dropdown.RefreshShownValue();
    }

    public void OnDropdownValueChanged()
    {
        WalletManager.UpdateRecipient(Dropdown.itemText.text);
    }
}
