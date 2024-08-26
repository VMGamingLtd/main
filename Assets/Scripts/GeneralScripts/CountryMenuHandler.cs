using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountryMenuHandler : MonoBehaviour
{
    [SerializeField] GameObject flagTemplate;
    private readonly IList<string> countryNames = new List<string>() { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Argentina", "Armenia", "Australia", "Austria",
                                                                       "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Bolivia",
                                                                       "BosniaAndHerzegovina", "Botswana", "Brazil", "Bulgaria", "Cambodia", "Canada", "CentralAfricanRepublic",
                                                                       "Chad", "Chile", "China", "Colombia", "CostaRica", "Croatia", "Cuba", "Cyprus", "CzechRepublic",
                                                                       "DemocraticRepublicOfCongo", "Denmark", "DominicanRepublic", "Ecuador", "Egypt", "Eritrea", "Estonia",
                                                                       "Ethiopia", "Fiji", "Finland", "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece",
                                                                       "Grenada", "Guatemala", "Haiti", "Honduras", "HongKong", "Hungary", "Iceland", "India", "Indonesia", "Iran",
                                                                       "Iraq", "Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati",
                                                                       "Kosovo", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein",
                                                                       "Lithuania", "Luxembourg", "Madagascar", "Malaysia", "Malta", "Mexico", "Moldova", "Mongolia", "Montenegro",
                                                                       "Morocco", "Mozambique", "Myanmar", "Namibia", "Nepal", "Netherlands", "NewZealand", "Nicaragua", "Nigeria",
                                                                       "NorthKorea", "NorthMacedonia", "Norway", "Oman", "Pakistan", "Palau", "Palestine", "Panama", "PapuaNewGuinea",
                                                                       "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Quatar", "RepublicOfCongo", "Romania", "Russia",
                                                                       "Rwanda", "SaudiArabia", "Scotland", "Senegal", "Serbia", "Singapore", "Slovakia", "Slovenia", "Somalia",
                                                                       "SouthAfrica", "SouthKorea", "Spain", "SriLanka", "Sudan", "Suriname", "Sweden", "Switzerland", "Syria",
                                                                       "Taiwan", "Tajikistan", "Thailand", "Togo", "TrinidadAndTobago", "Tunisia", "Turkey", "Turkmenistan", "Uganda",
                                                                       "Ukraine", "UnitedArabEmirates", "UnitedKingdom", "UnitedStates", "Uruguay", "Uzbekistan", "Venezuela",
                                                                       "Vietnam", "Wales", "Yemen", "Zambia", "Zimbabwe"};

    void Start()
    {
        if (Application.isPlaying)
        {
            var container = gameObject.transform.Find("ScrollView/Viewport/Content").transform;

            foreach (var country in countryNames)
            {
                GameObject newCountry = Instantiate(flagTemplate, container);
                newCountry.AddComponent<FlagNameUpdater>();
                newCountry.name = country;
                newCountry.transform.Find("Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignFlagSpriteToSlot(country);

                if (country == "BosniaAndHerzegovina")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Bosnia and Herzegovina";
                }
                else if (country == "CentralAfricanRepublic")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Central African Republic";
                }
                else if (country == "CzechRepublic")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Czech Republic";
                }
                else if (country == "DemocraticRepublicOfCongo")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Democratic Republic of Congo";
                }
                else if (country == "DominicanRepublic")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Dominican Republic";
                }
                else if (country == "NorthKorea")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "North Korea";
                }
                else if (country == "NorthMacedonia")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "North Macedonia";
                }
                else if (country == "PapuaNewGuinea")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Papua New Guinea";
                }
                else if (country == "RepublicOfCongo")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Republic of Congo";
                }
                else if (country == "SaudiArabia")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Saudi Arabia";
                }
                else if (country == "SouthAfrica")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "South Africa";
                }
                else if (country == "SouthKorea")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "South Korea";
                }
                else if (country == "SriLanka")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Sri Lanka";
                }
                else if (country == "TrinidadAndTobago")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "Trinidad and Tobago";
                }
                else if (country == "UnitedArabEmirates")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "United Arab Emirates";
                }
                else if (country == "UnitedKingdom")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "United Kingdom";
                }
                else if (country == "UnitedStates")
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = "United States";
                }
                else
                {
                    newCountry.GetComponentInChildren<TextMeshProUGUI>().text = country;
                }
            }
        }
    }
}
