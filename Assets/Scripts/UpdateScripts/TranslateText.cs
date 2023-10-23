using System.Collections;
using TMPro;
using UnityEngine;


public class TranslateText : MonoBehaviour
{
    private TextMeshProUGUI myText;
    private TranslationManager translationManager;
    public string identifier;

    void Awake()
    {
        StartCoroutine(FindTranslationManager());
    }

    private IEnumerator FindTranslationManager()
    {
        yield return null;
        translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
        myText = GetComponent<TextMeshProUGUI>();
        myText.text = translationManager.Translate(identifier);
    }
}
