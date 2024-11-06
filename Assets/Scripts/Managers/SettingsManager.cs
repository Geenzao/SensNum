using Settings;
using Settings.Audio;
using Settings.Graphics;
using Settings.Graphics.Rendering.Volume;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        [SerializeField] private GlobalQuality _globalQuality;
        [SerializeField] private VolumeMusic _volumeMusic;
        [SerializeField] private VignetteIntensity _vignetteIntensity;

        public static T GetSettingOfType<T>() where T : class
        {
            if (Instance == null)
            {
                Debug.LogError("SettingsManager is null");
                return null;
            }

            if (Instance._globalQuality.GetType() == typeof(T))
            {
                return Instance._globalQuality as T;
            }
            else if (Instance._volumeMusic.GetType() == typeof(T))
            {
                return Instance._volumeMusic as T;
            }
            else if (Instance._vignetteIntensity.GetType() == typeof(T))
            {
                return Instance._vignetteIntensity as T;
            }
            else
            {
                Debug.LogError("Setting not found");
                return null;
            }
        }
    }

}