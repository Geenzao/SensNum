using Settings.Audio;
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

    //[Header("Dropdown")]
    //[SerializeField] private TMPro.TMP_Dropdown _qualityDropdown;

    //private GlobalQuality _paramGlobalQuality;
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

    // Start is called before the first frame update
    void Start()
    {
        var settingsGlobalQuality = Settings.SettingsManager.GetSettingOfType<GlobalQuality>();
        var settingsMusicVolume = Settings.SettingsManager.GetSettingOfType<VolumeMusic>();
        var settingsSoundVolume = Settings.SettingsManager.GetSettingOfType<VolumeSounds>();

        //On this UI changes
        volumeMusicScrollbar.onValueChanged.AddListener(HandleMusicVolumeScrollbarValueChanged);
        volumeSoundScrollbar.onValueChanged.AddListener(HandleButtonVolumeScrollbarValueChanged);

        //Init music volume
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
        List<CTuple<string, string>> languages = LanguageManager.Instance.GetLanguages();
        languageDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentLanguageIndex = 0;

        for (int i = 0; i < languages.Count; i++)
        {
            CTuple<string, string> language = languages[i];
            options.Add(language.Item1);

            if (language.Item2 == LanguageManager.Instance.GetCurrentLanguage())
            {
                currentLanguageIndex = i;
            }
        }
        languageDropdown.AddOptions(options);
        languageDropdown.value = currentLanguageIndex;
        languageDropdown.RefreshShownValue();
    }

    private void OnLanguageDropdownValueChanged(int index)
    {
        var selectedLanguage = languageDropdown.options[index].text;
        var languages = LanguageManager.Instance.GetLanguages();
        foreach (var language in languages)
        {
            if (language.Item1 == selectedLanguage)
            {
                LanguageManager.Instance.SetLanguage(language.Item2);
                break;
            }
        }
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
        title.text = LanguageManager.Instance.GetText("settings");
        music.text = LanguageManager.Instance.GetText("music");
        sound.text = LanguageManager.Instance.GetText("sound");
    }
}
