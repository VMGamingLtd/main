using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountSettings : MonoBehaviour
{
    [SerializeField] GameObject username;
    [SerializeField] GameObject password;
    [SerializeField] GameObject email;
    [SerializeField] GameObject country;
    [SerializeField] GameObject language;

    void OnEnable()
    {
        TranslationManager translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();

        var isGuest = Gaos.Context.Authentication.GetIsGuest();

        var userName = Gaos.Context.Authentication.GetUserName();
        username.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = userName;

        if (isGuest)
        {
            username.transform.Find("Button").GetComponent<Image>().color = UIColors.YellowHalf;
        }
        else
        {
            username.transform.Find("Button").gameObject.SetActive(false);
            password.transform.Find("Block").gameObject.SetActive(false);
            email.transform.Find("Block").gameObject.SetActive(false);
            email.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Gaos.Context.Authentication.GetEmail();
        }

        var countryName = Gaos.Context.Authentication.GetCountry();
        country.transform.Find("Flag").GetComponent<Image>().sprite = AssetBundleManager.AssignFlagSpriteToSlot(countryName);
        country.transform.Find("CountryName").GetComponent<TextMeshProUGUI>().text = countryName;

        var languageName = Gaos.Context.Authentication.GetLanguage();
        var modName = char.ToUpper(languageName[0]) + languageName[1..];
        language.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(modName);
    }
}
