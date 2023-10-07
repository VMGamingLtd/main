using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using RecipeManagement;
using System;

[System.Serializable]
public class TranslationDataArrayWrapper
{
    public TranslationData[] translations;
}

[System.Serializable]
public class TranslationData
{
    public string EnglishText;
    public string RussianText;
    public string ChineseText;
    public string SlovakText;
}
[Serializable]
public class CoreTranslations
{
    public string identifier;
    public string english;
    public string russian;
    public string chinese;
    public string slovak;

}
public class TranslationManager : MonoBehaviour
{
    public GameObject[] translationTexts;
    private List<CoreTranslations> coreTranslationList;

    [Serializable]
    private class JsonArray
    {
        public List<CoreTranslations> words;
    }

    void Awake()
    {
        string filePath = Path.Combine(Application.dataPath, "Scripts/Models/CoreTranslations.json");

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            JsonArray jsonArray = JsonUtility.FromJson<JsonArray>(jsonText);
            if (jsonArray != null)
            {
                coreTranslationList = jsonArray.words;
            }
        }
        else
        {
            Debug.LogError("CoreTranslations.json not found at: " + filePath);
        }
    }

    public string Translate(string identifier)
    {
        CoreTranslations entry = FindTranslationEntry(identifier);

        if (entry != null)
        {
            return Application.systemLanguage switch
            {
                SystemLanguage.English => entry.english,
                SystemLanguage.Russian => entry.russian,
                SystemLanguage.Chinese => entry.chinese,
                SystemLanguage.Slovak => entry.slovak,
                _ => entry.english,
            };
        }
        else
        {
            return "???";
        }
    }
    CoreTranslations FindTranslationEntry(string identifier)
    {
        foreach (CoreTranslations entry in coreTranslationList)
        {
            if (entry.identifier == identifier)
            {
                return entry;
            }
        }
        return null;
    }
    public void PushTextsIntoFile()
    {
        List<TranslationData> translationDataList = new();

        for (int i = 0; i < translationTexts.Length; i++)
        {
            GameObject translationTextObject = translationTexts[i];
            
            if (translationTextObject.TryGetComponent<TranslateText>(out var translateTextScript))
            {
                TranslationData translationData = new()
                {
                    EnglishText = translateTextScript.EnglishText,
                    RussianText = translateTextScript.RussianText,
                    ChineseText = translateTextScript.ChineseText,
                    SlovakText = translateTextScript.SlovakText
                };

                translationDataList.Add(translationData);
            }
        }

        TranslationDataArrayWrapper dataArrayWrapper = new()
        {
            translations = translationDataList.ToArray()
        };

        string jsonData = JsonUtility.ToJson(dataArrayWrapper, true);

        string filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Translations.json");

        File.WriteAllText(filePath, jsonData);

        Debug.Log("Translations pushed into file: " + filePath);
    }
    public void PullTextsFromFile()
    {
        string filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Translations.json");

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);

            TranslationDataArrayWrapper dataArrayWrapper = JsonUtility.FromJson<TranslationDataArrayWrapper>(jsonData);

            if (dataArrayWrapper != null)
            {
                TranslationData[] translationDataArray = dataArrayWrapper.translations;

                // Iterate through each translationTexts object and update the translation data
                for (int i = 0; i < translationTexts.Length && i < translationDataArray.Length; i++)
                {
                    GameObject translationTextObject = translationTexts[i];
                    
                    if (translationTextObject.TryGetComponent<TranslateText>(out var translateTextScript))
                    {
                        translateTextScript.EnglishText = translationDataArray[i].EnglishText;
                        translateTextScript.RussianText = translationDataArray[i].RussianText;
                        translateTextScript.ChineseText = translationDataArray[i].ChineseText;
                        translateTextScript.SlovakText = translationDataArray[i].SlovakText;
                    }
                }

                Debug.Log("Translations pulled from file: " + filePath);
            }
            else
            {
                Debug.LogWarning("Failed to parse translation data from file: " + filePath);
            }
        }
        else
        {
            Debug.LogWarning("Translation file not found: " + filePath);
        }
    }
}
