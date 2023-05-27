using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TranslateText : MonoBehaviour
{
    public string EnglishText;
    public string RussianText;
    public string ChineseText;
    public string SlovakText;
    private TextMeshProUGUI myText;

    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();

        if (Application.systemLanguage == SystemLanguage.English)
        {
            myText.text = EnglishText;
        }
        else if (Application.systemLanguage == SystemLanguage.Russian)
        {
            myText.text = RussianText;
        }
        else if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            myText.text = ChineseText;
        }
        else if (Application.systemLanguage == SystemLanguage.Slovak)
        {
            myText.text = SlovakText;
        }
    }

}
