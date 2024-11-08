using Settings.Audio;
using Settings.Graphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button quitButton;

    [Header("Scrollbar")]
    [SerializeField] private Scrollbar volumeSoundScrollbar;
    [SerializeField] private Scrollbar volumeMusicScrollbar;

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


        //Init graphics dropdown
        //_qualityDropdown.ClearOptions();
        //List<TMPro.TMP_Dropdown.OptionData> tabOptions = new();
        //foreach (string preset in settingsGlobalQuality.QualitySettingsName)
        //{
        //    tabOptions.Add(new TMPro.TMP_Dropdown.OptionData(preset));
        //}
        //_qualityDropdown.AddOptions(tabOptions);
        //_qualityDropdown.value = settingsGlobalQuality.GetParam();
        //_qualityDropdown.onValueChanged.AddListener(HandleQualityDropdownValueChanged);
    }

    //private void HandleGlobalQualityChanged(int newVal)
    //{
    //    _qualityDropdown.value = newVal;
    //}

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

    //private void HandleQualityDropdownValueChanged(int val)
    //{
    //    if (!_qualityDropdown.IsActive())
    //        return;
    //    _paramGlobalQuality.UpdateParam(val);
    //}

    private void OnQuitButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
