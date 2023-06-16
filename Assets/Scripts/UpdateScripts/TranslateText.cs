using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslateText : MonoBehaviour
{
    [TextArea(3, 10)]
    public string EnglishText;
    [TextArea(3, 10)]
    public string RussianText;
    [TextArea(3, 10)]
    public string ChineseText;
    [TextArea(3, 10)]
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
