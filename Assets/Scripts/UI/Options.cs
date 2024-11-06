using Settings.Audio;
using Settings.Graphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [Header("Scrollbar")]
    [SerializeField] private Scrollbar volumeButtonsScrollbar;
    [SerializeField] private Scrollbar volumeMusicScrollbar;

    [Header("Dropdown")]
    [SerializeField] private TMPro.TMP_Dropdown _qualityDropdown;

    private GlobalQuality _paramGlobalQuality;
    private VolumeMusic _paramVolumeMusic;

    private void Awake()
    {
        _paramGlobalQuality = Settings.SettingsManager.GetSettingOfType<GlobalQuality>();
        _paramVolumeMusic = Settings.SettingsManager.GetSettingOfType<VolumeMusic>();

        HandleMusicVolumeChanged(_paramVolumeMusic.GetParam());
        //\todo [buttons volume]

        //On Settings update
        _paramVolumeMusic.OnUpdate.AddListener(HandleMusicVolumeChanged);
        _paramGlobalQuality.OnUpdate.AddListener(HandleGlobalQualityChanged);
    }

    // Start is called before the first frame update
    void Start()
    {

        var settingsGlobalQuality = Settings.SettingsManager.GetSettingOfType<GlobalQuality>();
        var settingsMusicVolume = Settings.SettingsManager.GetSettingOfType<VolumeMusic>();

        //On this UI changes
        volumeMusicScrollbar.onValueChanged.AddListener(HandleMusicVolumeScrollbarValueChanged);
        volumeButtonsScrollbar.onValueChanged.AddListener(HandleButtonVolumeScrollbarValueChanged);

        //Init graphics dropdown
        _qualityDropdown.ClearOptions();
        List<TMPro.TMP_Dropdown.OptionData> tabOptions = new();
        foreach (string preset in settingsGlobalQuality.QualitySettingsName)
        {
            tabOptions.Add(new TMPro.TMP_Dropdown.OptionData(preset));
        }
        _qualityDropdown.AddOptions(tabOptions);
        _qualityDropdown.value = settingsGlobalQuality.GetParam();
        _qualityDropdown.onValueChanged.AddListener(HandleQualityDropdownValueChanged);
    }

    private void HandleGlobalQualityChanged(int newVal)
    {
        _qualityDropdown.value = newVal;
    }

    private void HandleMusicVolumeChanged(float newVal)
    {
        volumeMusicScrollbar.value = newVal;
    }

    private void HandleMusicVolumeScrollbarValueChanged(float val)
    {
        if (!volumeMusicScrollbar.IsActive())
            return;
        _paramVolumeMusic.UpdateParam(val);
    }

    private void HandleButtonVolumeScrollbarValueChanged(float val)
    {
        if (!volumeButtonsScrollbar.IsActive())
            return;
        Debug.Log(val);
    }

    private void HandleQualityDropdownValueChanged(int val)
    {
        if (!_qualityDropdown.IsActive())
            return;
        _paramGlobalQuality.UpdateParam(val);
    }
}
