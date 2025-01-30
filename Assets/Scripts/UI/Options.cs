﻿using Settings.Audio;
using Settings.Graphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
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
        // Récupérer la liste des langues disponibles
        List<string> languages = LanguageManager.Instance.GetLanguages();

        // Vider les options actuelles du dropdown
        languageDropdown.ClearOptions();

        // Créer une liste d'options avec les drapeaux
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < languages.Count; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = languages[i];
            option.image = languageFlags[i]; // Assurez-vous que l'ordre des drapeaux correspond à l'ordre des langues
            options.Add(option);
        }

        // Ajouter les options au dropdown
        languageDropdown.AddOptions(options);

        // Définir la langue actuelle comme sélectionnée dans le dropdown
        string currentLanguage = LanguageManager.Instance.GetCurrentLanguage();
        int currentLanguageIndex = languages.IndexOf(currentLanguage);

        if (currentLanguageIndex >= 0)
        {
            languageDropdown.value = currentLanguageIndex;
            languageDropdown.RefreshShownValue();
            currentLanguageFlag.sprite = languageFlags[currentLanguageIndex]; // Mettre à jour le drapeau de la langue actuelle
        }
    }

    private void OnLanguageDropdownValueChanged(int index)
    {
        // Récupérer la langue sélectionnée dans le dropdown
        string selectedLanguage = languageDropdown.options[index].text;

        // Changer la langue
        LanguageManager.Instance.ChangeLanguage(selectedLanguage);

        // Mettre à jour le drapeau de la langue actuelle
        currentLanguageFlag.sprite = languageFlags[index];
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
