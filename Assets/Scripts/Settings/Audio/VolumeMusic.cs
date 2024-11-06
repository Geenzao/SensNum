using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Settings
{
    namespace Audio
    {
        public class VolumeMusic : SettingsFloat
        {
            [SerializeField] private AudioMixer _audioMixer;
            protected override void ApplyChanges()
            {
                if (_value == 0)
                    _audioMixer.SetFloat("volumeMusic", -80);
                else
                    _audioMixer.SetFloat("volumeMusic", Mathf.Log10(_value) * 20);
            }
        }
    }
}