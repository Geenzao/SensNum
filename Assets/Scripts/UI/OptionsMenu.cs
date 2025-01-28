using Settings.Audio;
using Settings.Graphics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : Menu
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

    [Header("Panel")]
    [SerializeField] private GameObject panelOptions;

    private VolumeMusic _paramVolumeMusic;
    private VolumeSounds _paramVolumeSounds;

    private void Awake()
    {
        //_paramGlobalQuality = Settings.SettingsManager.GetSettingOfType<GlobalQuality>();
        _paramVolumeMusic = Settings.SettingsManager.GetSettingOfType<VolumeMusic>();
        _paramVolumeSounds = Settings.SettingsManager.GetSettingOfType<VolumeSounds>();

        HandleMusicVolumeChanged(_paramVolumeMusic.GetParam());
        //\todo [buttons volume]

        quitButton.onClick.AddListener(OnQuitButtonClicked);

        //On Settings update
        _paramVolumeMusic.OnUpdate.AddListener(HandleMusicVolumeChanged);
        _paramVolumeSounds.OnUpdate.AddListener(HandleSoundVolumeChanged);
        //_paramGlobalQuality.OnUpdate.AddListener(HandleGlobalQualityChanged);

        languageDropdown.onValueChanged.AddListener(OnLanguageDropdownValueChanged);

        LanguageManager.Instance.OnLanguageChanged += UpdateTexts;
    }

    protected override void Start()
    {
        base.Start();

        if (UIManager.CurrentMenuState == UIManager.MenuState.PauseMenu)
        {
            TriggerVisibility(true);
        }

        var settingsGlobalQuality = Settings.SettingsManager.GetSettingOfType<GlobalQuality>();
        var settingsMusicVolume = Settings.SettingsManager.GetSettingOfType<VolumeMusic>();
        var settingsSoundVolume = Settings.SettingsManager.GetSettingOfType<VolumeSounds>();

        //On this UI changes
        volumeMusicScrollbar.onValueChanged.AddListener(HandleMusicVolumeScrollbarValueChanged);
        volumeSoundScrollbar.onValueChanged.AddListener(HandleButtonVolumeScrollbarValueChanged);

        //Init music volume
        volumeMusicScrollbar.value = 0.3f;
        volumeSoundScrollbar.value = 0.3f;

        // Charger les textes en fonction de la langue s�lectionn�e
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

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
        panelOptions.SetActive(visible);
    }

    private void InitializeLanguageDropdown()
    {
        // Récupérer la liste des langues disponibles
        List<string> languages = LanguageManager.Instance.GetLanguages();

        // Vider les options actuelles du dropdown
        languageDropdown.ClearOptions();

        // Ajouter les langues disponibles au dropdown
        languageDropdown.AddOptions(languages);

        // Définir la langue actuelle comme sélectionnée dans le dropdown
        string currentLanguage = LanguageManager.Instance.GetCurrentLanguage();
        int currentLanguageIndex = languages.IndexOf(currentLanguage);

        if (currentLanguageIndex >= 0)
        {
            languageDropdown.value = currentLanguageIndex;
            languageDropdown.RefreshShownValue();
        }
    }

    private void OnLanguageDropdownValueChanged(int index)
    {
        var selectedLanguage = languageDropdown.options[index].text;
        var languages = LanguageManager.Instance.GetLanguages();
        foreach (var language in languages)
        {
            if (language == selectedLanguage)
            {
                LanguageManager.Instance.ChangeLanguage(language);
                break;
            }
        }
    }


    protected override void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        base.HandleMenuStateChanged(newMS, oldMS);
        if (newMS == UIManager.MenuState.PauseMenu)
            TriggerVisibility(true); //true
        else
            TriggerVisibility(false);
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
        GameManager.Instance.UpdateGameState(GameManager.GameState.RUNNING);
    }

    private void UpdateTexts()
    {
        title.text = LanguageManager.Instance.GetText("settings");
        music.text = LanguageManager.Instance.GetText("music");
        sound.text = LanguageManager.Instance.GetText("sound");
        language.text = LanguageManager.Instance.GetText("language");
    }
}
