using Settings.Audio;
using Settings.Graphics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OptionsMenu : Menu
{
    [Header("Button")]
    [SerializeField] private Button quitButton;

    [Header("Scrollbar")]
    [SerializeField] private Scrollbar volumeSoundScrollbar;
    [SerializeField] private Scrollbar volumeMusicScrollbar;

    [Header("Panel")]
    [SerializeField] private GameObject panelOptions;

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
    }

    protected override void TriggerVisibility(bool visible)
    {
        base.TriggerVisibility(visible);
        panelOptions.SetActive(visible);
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
        UIManager.Instance.UpdateMenuState(UIManager.MenuState.None);
    }
}
