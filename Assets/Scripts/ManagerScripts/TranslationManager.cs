using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

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
public class TranslationManager : MonoBehaviour
{
    public GameObject[] translationTexts;

    public void PushTextsIntoFile()
    {
        List<TranslationData> translationDataList = new List<TranslationData>();

        // Iterate through each translationTexts object
        for (int i = 0; i < translationTexts.Length; i++)
        {
            GameObject translationTextObject = translationTexts[i];
            TranslateText translateTextScript = translationTextObject.GetComponent<TranslateText>();

            if (translateTextScript != null)
            {
                TranslationData translationData = new TranslationData();
                translationData.EnglishText = translateTextScript.EnglishText;
                translationData.RussianText = translateTextScript.RussianText;
                translationData.ChineseText = translateTextScript.ChineseText;
                translationData.SlovakText = translateTextScript.SlovakText;

                translationDataList.Add(translationData);
            }
        }

        TranslationDataArrayWrapper dataArrayWrapper = new TranslationDataArrayWrapper();
        dataArrayWrapper.translations = translationDataList.ToArray();

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
                    TranslateText translateTextScript = translationTextObject.GetComponent<TranslateText>();

                    if (translateTextScript != null)
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
