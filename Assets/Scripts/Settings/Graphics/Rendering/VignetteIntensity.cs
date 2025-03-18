using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Settings.Graphics.Rendering.Volume
{
    public class VignetteIntensity : Settings.SettingsFloat
    {
        private UnityEngine.Rendering.Volume _volume;
        private Vignette _compVignette;

        //private float _startVignetteIntensity;

        private void Awake()
        {
            HandleLevelLoadingEnded(null);
            GameManager.Instance.OnLoadingEnded.AddListener(HandleLevelLoadingEnded);
        }

        private void HandleLevelLoadingEnded(string levelName)
        {
            _volume = FindFirstObjectByType<UnityEngine.Rendering.Volume>();
            if (_volume != null)
            {
                _volume.profile.TryGet<Vignette>(out _compVignette);
                /*Debug.Log(_volume.GetInstanceID());*/
                if (_compVignette == null)
                {
                    _compVignette = _volume.profile.Add<Vignette>();
                }
            }
        }

        protected override void ApplyChanges()
        {
            if (_volume != null && _compVignette != null)
                _compVignette.intensity.value = _value;
        }

        //public void UpdateVignetteIntensity(float intensity, float timeInSec = 3f)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(UpdateVignetteIntensityCoroutine(intensity, timeInSec));
        //}

        //public void RestoreVignetteIntensity(float timeInSec = 3f)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(UpdateVignetteIntensityCoroutine(_startVignetteIntensity, timeInSec));
        //}

        //private IEnumerator UpdateVignetteIntensityCoroutine(float intensity, float timeInSec)
        //{
        //    if (intensity > 1)
        //    {
        //        intensity = 1;
        //    }
        //    else if (intensity < 0)
        //    {
        //        intensity = 0;
        //    }

        //    float startIntensity = _compVignette.intensity.value;
        //    float time = 0;
        //    while (time < timeInSec)
        //    {
        //        _compVignette.intensity.value = Mathf.Lerp(startIntensity, intensity, time / timeInSec);
        //        time += Time.deltaTime;
        //        yield return null;
        //    }
        //    _compVignette.intensity.value = intensity;
        //}
    }
}