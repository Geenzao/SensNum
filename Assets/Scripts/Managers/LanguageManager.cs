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
}


public class LanguageManager : Singleton<LanguageManager>
{
    public event Action OnLanguageChanged;

    private List<List<string>> currentLanguageDictionary = new List<List<string>>();
    private string currentLanguage = "en"; // Langue par défaut
    private readonly string languageFolderPath = "Languages"; // Dossier où sont stockés les JSON

    private void Start()
    {
        StartCoroutine(LoadLanguage(currentLanguage));
    }

    public void ChangeLanguage(string newLanguage)
    {
        if (newLanguage != currentLanguage)
        {
            StartCoroutine(LoadLanguage(newLanguage, success =>
            {
                if (success)
                {
                    currentLanguage = newLanguage;
                    Debug.LogWarning("Language changed to: " + currentLanguage);
                    OnLanguageChanged?.Invoke(); // Déclenchement de l'événement
                }
            }));
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
                callback(data.languages);
            }
            else
            {
                Debug.LogError("Error loading language list: " + request.error);
                callback(new List<string> { "en" });
            }
        }
    }


    public string GetCurrentLanguage()
    {
        return currentLanguage;
    }
}
