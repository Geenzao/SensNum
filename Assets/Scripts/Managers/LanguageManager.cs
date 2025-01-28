using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine;
using System.IO;

public class LanguageManager : Singleton<LanguageManager>
{
    public event Action OnLanguageChanged;

    private Dictionary<string, string> currentLanguageDictionary;
    private string currentLanguage = "en"; // Langue par défaut

    protected override void Awake()
    {
        base.Awake();
        LoadLanguage(currentLanguage);
    }

    public void ChangeLanguage(string newLanguage)
    {
        if (newLanguage != currentLanguage)
        {
            currentLanguage = newLanguage;
            LoadLanguage(currentLanguage);
            OnLanguageChanged?.Invoke(); // Déclenchement de l'événement
        }
    }

    private void LoadLanguage(string language)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, language + ".json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            currentLanguageDictionary = JsonUtility.FromJson<SerializableDictionary>(dataAsJson).ToDictionary();
        }
        else
        {
            Debug.LogError("Cannot find language file: " + filePath);
        }
    }

    public string GetText(string key)
    {
        if (currentLanguageDictionary != null && currentLanguageDictionary.ContainsKey(key))
        {
            return currentLanguageDictionary[key];
        }
        else
        {
            //Debug.LogWarning("Key not found in language dictionary: " + key);
            return "[" + key + "]";
        }
    }

    public List<string> GetLanguages()
    {
        List<string> languages = new List<string>();

        // Récupérer tous les fichiers JSON dans le dossier StreamingAssets
        string[] files = Directory.GetFiles(Application.streamingAssetsPath, "*.json");

        foreach (string file in files)
        {
            // Extraire le nom de la langue du nom du fichier (ex: "en.json" -> "en")
            string fileName = Path.GetFileNameWithoutExtension(file);
            languages.Add(fileName);
        }

        return languages;
    }

    [System.Serializable]
    private class SerializableDictionary
    {
        public List<LanguageEntry> entries;

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var entry in entries)
            {
                dict[entry.key] = entry.value;
            }
            return dict;
        }
    }

    [System.Serializable]
    private class LanguageEntry
    {
        public string key;
        public string value;
    }

    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }

}
