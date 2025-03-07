using Settings.Audio;
using Settings.Graphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class Options : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI music;
    [SerializeField] private TextMeshProUGUI sound;
    [SerializeField] private TextMeshProUGUI language;

    [Header("Button")]
    [SerializeField] private Button quitButton;

    [Header("Scrollbar")]
    [SerializeField] private Scrollbar volumeSoundScrollbar;
    [SerializeField] private Scrollbar volumeMusicScrollbar;

    [Header("Dropdown")]
    [SerializeField] private TMP_Dropdown languageDropdown;

    [Header("Flags")]
    [SerializeField] private List<Sprite> languageFlags;
    [SerializeField] private Image currentLanguageFlag;

    private VolumeMusic _paramVolumeMusic;
    private VolumeSounds _paramVolumeSounds;

    private void Awake()
    {
        _paramVolumeMusic = Settings.SettingsManager.GetSettingOfType<VolumeMusic>();
        _paramVolumeSounds = Settings.SettingsManager.GetSettingOfType<VolumeSounds>();

        if (_paramVolumeMusic != null)
        {
            HandleMusicVolumeChanged(_paramVolumeMusic.GetParam());
            _paramVolumeMusic.OnUpdate.AddListener(HandleMusicVolumeChanged);
        }
        else
        {
            Debug.LogError("VolumeMusic setting is not initialized.");
        }

        if (_paramVolumeSounds != null)
        {
            _paramVolumeSounds.OnUpdate.AddListener(HandleSoundVolumeChanged);
        }
        else
        {
            Debug.LogError("VolumeSounds setting is not initialized.");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
        else
        {
            Debug.LogError("Quit button is not assigned.");
        }

        if (languageDropdown != null)
        {
            languageDropdown.onValueChanged.AddListener(OnLanguageDropdownValueChanged);
        }
        else
        {
            Debug.LogError("Language dropdown is not assigned.");
        }

        if (LanguageManager.Instance != null)
        {
            // S'abonner à l'événement de changement de langue
            LanguageManager.OnLanguageChanged += UpdateTexts;
        }
        else
        {
            Debug.LogError("LanguageManager instance is not initialized.");
        }
        
    }


    void Start()
    {
        var settingsGlobalQuality = Settings.SettingsManager.GetSettingOfType<GlobalQuality>();
        var settingsMusicVolume = Settings.SettingsManager.GetSettingOfType<VolumeMusic>();
        var settingsSoundVolume = Settings.SettingsManager.GetSettingOfType<VolumeSounds>();

        // On this UI changes
        volumeMusicScrollbar.onValueChanged.AddListener(HandleMusicVolumeScrollbarValueChanged);
        volumeSoundScrollbar.onValueChanged.AddListener(HandleButtonVolumeScrollbarValueChanged);

        // Init music volume
        volumeMusicScrollbar.value = 0.3f;
        volumeSoundScrollbar.value = 0.3f;

        // Charger les textes en fonction de la langue sélectionnée
        if (LanguageManager.Instance != null)
        {
            UpdateTexts();
        }
        else
        {
            Debug.LogError("LanguageManager instance is not initialized.");
        }

        InitializeLanguageDropdown();
    }

    private void InitializeLanguageDropdown()
    {
        // Attendre le chargement de la liste des langues
        StartCoroutine(LoadLanguagesDropdown());
    }

    private IEnumerator LoadLanguagesDropdown()
    {
        // Attendre la liste des langues depuis LanguageManager
        yield return LanguageManager.Instance.LoadLanguagesList(languages =>
        {
            // Vider les options actuelles du dropdown
            languageDropdown.ClearOptions();

            // Ajouter les langues disponibles au dropdown
            languageDropdown.AddOptions(languages);

            // Vérifier si le nombre de drapeaux correspond au nombre de langues
            if (languageFlags.Count < languages.Count)
            {
                Debug.LogError("Le nombre de drapeaux ne correspond pas au nombre de langues disponibles");
                return;
            }

            // Afficher les drapeaux
            for (int i = 0; i < languages.Count; i++)
            {
                if (i < languageFlags.Count) // Vérification supplémentaire
                {
                    languageDropdown.options[i].image = languageFlags[i];
                }
            }

            // Définir la langue actuelle comme sélectionnée dans le dropdown
            string currentLanguage = LanguageManager.Instance.GetCurrentLanguage();
            int currentLanguageIndex = languages.IndexOf(currentLanguage);

            if (currentLanguageIndex >= 0 && currentLanguageIndex < languageFlags.Count)
            {
                languageDropdown.value = currentLanguageIndex;
                languageDropdown.RefreshShownValue();
                currentLanguageFlag.sprite = languageFlags[currentLanguageIndex];
                currentLanguageFlag.gameObject.GetComponent<Image>().enabled = true;
            }
        });
    }


    private void OnLanguageDropdownValueChanged(int index)
    {
        // Récupérer le nom complet de la langue
        string selectedLanguage = languageDropdown.options[index].text;
        
        // Changer la langue en utilisant le code correspondant
        LanguageManager.Instance.ChangeLanguage(selectedLanguage);

        // Mettre à jour le drapeau de la langue actuelle
        currentLanguageFlag.sprite = languageFlags[index];
        currentLanguageFlag.gameObject.GetComponent<Image>().enabled = true;
    }

    private void HandleMusicVolumeChanged(float newVal)
    {
        volumeMusicScrollbar.value = newVal;
    }

    private void HandleSoundVolumeChanged(float newVal)
    {
        volumeSoundScrollbar.value = newVal;
    }

    private void HandleMusicVolumeScrollbarValueChanged(float val)
    {
        if (!volumeMusicScrollbar.IsActive())
            return;
        _paramVolumeMusic.UpdateParam(val);
    }

    private void HandleButtonVolumeScrollbarValueChanged(float val)
    {
        if (!volumeSoundScrollbar.IsActive())
            return;
        _paramVolumeSounds.UpdateParam(val);
    }

    private void OnQuitButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffet(AudioType.UIButton);
        gameObject.SetActive(false);
    }

    private void UpdateTexts()
    {
        // Mettre à jour les textes en fonction de la langue actuelle
        title.text = LanguageManager.Instance.GetText("settings");
        music.text = LanguageManager.Instance.GetText("music");
        sound.text = LanguageManager.Instance.GetText("sound");
        language.text = LanguageManager.Instance.GetText("language");
    }
}
