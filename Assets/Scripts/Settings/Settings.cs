using System.Collections;
using UnityEngine;

namespace Settings
{
    public abstract class Settings<T> : MonoBehaviour
    {
        [SerializeField] protected string _key;
        protected T _value;

        [SerializeField] protected T _defaultValue;

        protected virtual void Start()
        {
            UpdateParam(GetParam());
        }

        public virtual void UpdateParam(T param)
        {
            _value = param;
            WriteToPlayerPref();
            ApplyChanges();
        }

        public abstract T GetParam();
        protected abstract void WriteToPlayerPref();
        protected abstract void ApplyChanges();
    }

    public abstract class SettingsInt : Settings<int>
    {
        public Events.Settings.OnSettingIntUpdate OnUpdate = new();

        public override void UpdateParam(int param)
        {
            base.UpdateParam(param);
            OnUpdate.Invoke(param);
        }

        public override int GetParam()
        {
            //Returns defaultValue if key is not stored in the playerprefs
            return _value = PlayerPrefs.GetInt(_key, _defaultValue);
        }
        protected override void WriteToPlayerPref()
        {
            PlayerPrefs.SetInt(_key, _value);
            PlayerPrefs.Save();
        }
    }

    public abstract class SettingsFloat : Settings<float>
    {
        public Events.Settings.OnSettingFloatUpdate OnUpdate = new();

        public override void UpdateParam(float param)
        {
            base.UpdateParam(param);
            OnUpdate.Invoke(param);
        }

        public override float GetParam()
        {
            //Returns defaultValue if key is not stored in the playerprefs
            return PlayerPrefs.GetFloat(_key, _defaultValue);
        }
        protected override void WriteToPlayerPref()
        {
            PlayerPrefs.SetFloat(_key, _value);
            PlayerPrefs.Save();
        }

        public IEnumerator FadeCoroutine(float desiredParam, float smoothInSec = 0)
        {
            float startValue = GetParam();
            float time = 0;
            if (smoothInSec == 0)
            {
                UpdateParam(desiredParam);
                yield break;
            }
            while (time < smoothInSec)
            {
                UpdateParam(Mathf.Lerp(startValue, desiredParam, time / smoothInSec));
                time += Time.deltaTime;
                yield return null;
            }
        }
    }

    public abstract class SettingsString : Settings<string>
    {
        public Events.Settings.OnSettingStringUpdate OnUpdate = new();

        public override void UpdateParam(string param)
        {
            base.UpdateParam(param);
            OnUpdate.Invoke(param);
        }

        public override string GetParam()
        {
            //Returns defaultValue if key is not stored in the playerprefs
            return PlayerPrefs.GetString(_key, _defaultValue);
        }
        protected override void WriteToPlayerPref()
        {
            PlayerPrefs.SetString(_key, _value);
            PlayerPrefs.Save();
        }
    }
}
