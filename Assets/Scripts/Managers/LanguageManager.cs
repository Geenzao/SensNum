using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine;
using System.IO;

public class LanguageManager : Singleton<LanguageManager>
{
    public event Action OnLanguageChanged;

    private Dictionary<string, Dictionary<string, string>> _localizedTexts;
    [SerializeField] private string _currentLanguage;
    //private string filePath = "Assets/Data/xmllanguages.xml";
    private string filePath = Path.Combine(Application.streamingAssetsPath, "xmllanguages.xml");


    protected override void Awake()
    {
        base.Awake();
        _localizedTexts = new Dictionary<string, Dictionary<string, string>>();
    }

    private void Start()
    {
        LoadLanguageFile();
    }

    //public void LoadLanguageFile()
    //{
    //    _localizedTexts.Clear();
    //    XmlDocument xmlDocument = new XmlDocument();
    //    xmlDocument.Load(filePath);

    //    XmlNodeList wordNodes = xmlDocument.SelectNodes("/languages/words/word");
    //    foreach (XmlNode wordNode in wordNodes)
    //    {
    //        string key = wordNode.Attributes["id"].Value;
    //        var translations = new Dictionary<string, string>();

    //        foreach (XmlNode langNode in wordNode.ChildNodes)
    //        {
    //            translations[langNode.Name] = langNode.InnerText;
    //        }

    //        _localizedTexts[key] = translations;
    //    }
    //}
    public void LoadLanguageFile()
    {
        _localizedTexts.Clear();
        string fullPath = filePath;

        if (!File.Exists(fullPath))
        {
            Debug.LogError($"Language file not found at path: {fullPath}");
            return;
        }

        XmlDocument xmlDocument = new XmlDocument();
        try
        {
            File.SetLastWriteTimeUtc(fullPath, DateTime.UtcNow);
            xmlDocument.Load(fullPath);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load XML file: {ex.Message}");
            return;
        }

        XmlNodeList wordNodes = xmlDocument.SelectNodes("/languages/words/word");
        foreach (XmlNode wordNode in wordNodes)
        {
            string key = wordNode.Attributes["id"].Value;
            var translations = new Dictionary<string, string>();

            foreach (XmlNode langNode in wordNode.ChildNodes)
            {
                translations[langNode.Name] = langNode.InnerText;
            }

            _localizedTexts[key] = translations;
        }
    }


    public string GetText(string key)
    {
        if (_localizedTexts.TryGetValue(key, out Dictionary<string, string> translations))
        {
            if (translations.TryGetValue(_currentLanguage, out string value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning($"Text for language '{_currentLanguage}' not found for key '{key}'");
                return key; // Retourne la cl� si le texte n'est pas trouv� pour la langue actuelle
            }
        }
        else
        {
            //Debug.LogWarning($"Text key '{key}' not found");
            return key; // Retourne la cl� si le texte n'est pas trouv�
        }
    }

    public List<CTuple<string, string>> GetLanguages()
    {
        List<CTuple<string, string>> languages = new List<CTuple<string, string>>();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filePath);

        XmlNodeList languageNodes = xmlDocument.SelectNodes("/languages/language");
        foreach (XmlNode languageNode in languageNodes)
        {
            string code = languageNode.Attributes["code"].Value;
            string name = languageNode.Attributes["name"].Value;
            languages.Add(new CTuple<string, string>(name, code));
        }

        return languages;
    }

    public string GetCurrentLanguage()
    {
        return _currentLanguage;
    }

    public void SetLanguage(string language)
    {
        _currentLanguage = language;
        OnLanguageChanged?.Invoke(); // Déclenchement de l'événement

    }
}
