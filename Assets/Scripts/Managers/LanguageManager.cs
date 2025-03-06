using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

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

[Serializable]
public class LanguageList
{
    public List<string> languages;
    public List<string> fullname;
}


public class LanguageManager : Singleton<LanguageManager>
{
    public static event Action OnLanguageChanged;

    private List<List<string>> currentLanguageDictionary = new List<List<string>>();
    private string currentLanguage = "en"; // Langue par défaut
    private readonly string languageFolderPath = "Languages"; // Dossier où sont stockés les JSON

    public static void InvokeOnLanguageChange()
    {
        OnLanguageChanged?.Invoke();
    }

    private void Start()
    {
        StartCoroutine(LoadLanguage(currentLanguage));
    }

    public void ChangeLanguage(string newLanguage)
    {
        // Convertir le nom complet en code de langue
        string languageCode = GetLanguageCode(newLanguage);
        
        if (languageCode != currentLanguage)
        {
            StartCoroutine(LoadLanguage(languageCode, success =>
            {
                if (success)
                {
                    currentLanguage = languageCode;
                    Debug.LogWarning("Language changed to: " + currentLanguage);
                    OnLanguageChanged?.Invoke();
                }
            }));
        }
    }

    private string GetLanguageCode(string languageName)
    {
        // Cette méthode doit être implémentée pour convertir les noms complets en codes
        switch (languageName.ToLower())
        {
            case "english": return "en";
            case "français": return "fr";
            case "español": return "es";
            default: return languageName; // Si c'est déjà un code, on le retourne tel quel
        }
    }

    private IEnumerator LoadLanguage(string languageCode, Action<bool> callback = null)
    {
        string path = Path.Combine(Application.streamingAssetsPath, languageFolderPath, languageCode + ".json");

        if (path.Contains("://") || path.Contains(":\\"))
        {
            using (UnityWebRequest request = UnityWebRequest.Get(path))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error loading language file: " + request.error);
                    callback?.Invoke(false);
                    yield break;
                }

                string json = request.downloadHandler.text;
                ProcessLanguageData(json);
                callback?.Invoke(true);
            }
        }
        else if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ProcessLanguageData(json);
            callback?.Invoke(true);
        }
        else
        {
            Debug.LogError("Language file not found: " + path);
            callback?.Invoke(false);
        }
    }

    private void ProcessLanguageData(string json)
    {
        try
        {
            LanguageData data = JsonUtility.FromJson<LanguageData>(json);
            currentLanguageDictionary.Clear();
            foreach (var entry in data.entries)
            {
                currentLanguageDictionary.Add(new List<string> { entry.key, entry.value });
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error processing language file: " + ex.Message);
        }
    }

    public string GetText(string key)
    {
        foreach (var entry in currentLanguageDictionary)
        {
            if (entry[0] == key)
            {
                return entry[1];
            }
        }
        return key;
    }

    public void GetLanguages(Action<List<string>> callback)
    {
        StartCoroutine(LoadLanguagesList(callback));
    }


    public IEnumerator LoadLanguagesList(Action<List<string>> callback)
    {
        string path = Path.Combine(Application.streamingAssetsPath, languageFolderPath, "languages.json");

        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                LanguageList data = JsonUtility.FromJson<LanguageList>(request.downloadHandler.text);
                callback(data.fullname);
            }
            else
            {
                Debug.LogError("Error loading language list: " + request.error);
                callback(new List<string> { "english" });
            }
        }
    }


    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }
}
