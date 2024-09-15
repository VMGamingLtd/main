using System;
using System.Collections.Generic;
using UnityEngine;

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
    private List<CoreTranslations> coreTranslationList;
    private string? language;

    [Serializable]
    private class JsonArray
    {
        public List<CoreTranslations> words;
    }

    void Awake()
    {
        language = Gaos.Context.Authentication.GetLanguage();

        if (language == null)
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    language = "english";
                    break;
                case SystemLanguage.Russian:
                    language = "russian";
                    break;
                case SystemLanguage.Chinese:
                    language = "chinese";
                    break;
                case SystemLanguage.Slovak:
                    language = "slovak";
                    break;
                default:
                    language = "english";
                    break;
            }

            Gaos.Context.Authentication.SetLanguage(language);
        }

        string jsonText = Assets.Scripts.Models.CoreTranslationsJson.json;
        JsonArray jsonArray = JsonUtility.FromJson<JsonArray>(jsonText);
        if (jsonArray != null)
        {
            coreTranslationList = jsonArray.words;
        }
    }

    public string Translate(string identifier)
    {
        CoreTranslations entry = FindTranslationEntry(identifier);
        if (entry != null)
        {
            if (language != null)
            {
                return language switch
                {
                    "english" => entry.english,
                    "russian" => entry.russian,
                    "chinese" => entry.chinese,
                    "slovak" => entry.slovak,
                    _ => entry.english,
                };
            }
            else
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

    public CoreTranslations FindEntryBySubstring(string searchString)
    {
        foreach (CoreTranslations entry in coreTranslationList)
        {
            if (entry.identifier.Contains(searchString) ||
                entry.english.Contains(searchString) ||
                entry.russian.Contains(searchString) ||
                entry.chinese.Contains(searchString) ||
                entry.slovak.Contains(searchString))
            {
                return entry;
            }
        }
        return null;
    }
}
