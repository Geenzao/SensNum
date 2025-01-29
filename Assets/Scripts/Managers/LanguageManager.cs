using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;


[Serializable]
public class LanguageEntry
{
    public string key;
    public string value;
}

[Serializable]
public class LanguageData
{
    public List<LanguageEntry> entries;
}

public class LanguageManager : Singleton<LanguageManager>
{
    public event Action OnLanguageChanged;

    private List<List< string>> currentLanguageDictionary = new List<List<string>>();
    private string currentLanguage = "en"; // Langue par défaut
    private readonly string languageFolderPath = "Languages"; // Dossier où sont stockés les JSON



    private void Start()
    {
        LoadLanguage(currentLanguage);
    }

    public void ChangeLanguage(string newLanguage)
    {
        if (newLanguage != currentLanguage)
        {
            if (LoadLanguage(newLanguage))
            {
                currentLanguage = newLanguage;
                OnLanguageChanged?.Invoke(); // Déclenchement de l'événement
            }
        }
    }

    public bool LoadLanguage(string languageCode)
    {
        string path = Path.Combine(Application.streamingAssetsPath, languageFolderPath, languageCode + ".json");
        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                LanguageData data = JsonUtility.FromJson<LanguageData>(json);
                currentLanguageDictionary.Clear();
                foreach (var entry in data.entries)
                {

                    currentLanguageDictionary.Add(new List<string> { entry.key, entry.value });
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error loading language file: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Language file not found: " + path);
        }
        return false;
    }

    public string GetText(string key)
    {
        //print("GetText : " + key);
        ////return currentLanguageDictionary.TryGetValue(key, out string value) ? value : $"[{key}]";
        ////si la clé existe dane le dictionnnaire
        foreach (var entry in currentLanguageDictionary)
        {
            if (entry[0] == key)
            {
                return entry[1];
            }
        }
        return key;
    }

    public List<string> GetLanguages()
    {
        List<string> languages = new List<string>();
        string directoryPath = Path.Combine(Application.streamingAssetsPath, languageFolderPath);
        if (Directory.Exists(directoryPath))
        {
            foreach (var file in Directory.GetFiles(directoryPath, "*.json"))
            {
                languages.Add(Path.GetFileNameWithoutExtension(file));
            }
        }
        else
        {
            Debug.LogError("Languages directory not found: " + directoryPath);
        }
        return languages;
    }

    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }
}
