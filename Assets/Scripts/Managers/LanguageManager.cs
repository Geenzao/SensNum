using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LanguageManager : Singleton<LanguageManager>
{
    private Dictionary<string, Dictionary<string, string>> _localizedTexts;
    [SerializeField] private string _currentLanguage;
    private string filePath = "Assets/Data/xmllanguages.xml";

    protected override void Awake()
    {
        base.Awake();
        _localizedTexts = new Dictionary<string, Dictionary<string, string>>();
    }

    private void Start()
    {
        LoadLanguageFile();
    }

    public void LoadLanguageFile()
    {
        _localizedTexts.Clear();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filePath);

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
                return key; // Retourne la clé si le texte n'est pas trouvé pour la langue actuelle
            }
        }
        else
        {
            Debug.LogWarning($"Text key '{key}' not found");
            return key; // Retourne la clé si le texte n'est pas trouvé
        }
    }

    public void SetLanguage(string language)
    {
        _currentLanguage = language;
        // Recharger les textes si nécessaire
    }
}
