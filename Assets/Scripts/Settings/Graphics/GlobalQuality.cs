using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    namespace Graphics
    {
        public class GlobalQuality : SettingsInt
        {
            List<string> _qualitySettingsNames;

            protected override void Start()
            {
                base.Start();
                _qualitySettingsNames = new();
                _qualitySettingsNames.AddRange(QualitySettings.names);
            }

            protected override void ApplyChanges()
            {
                QualitySettings.SetQualityLevel(_value);
            }

            public string[] QualitySettingsName
            {
                get => _qualitySettingsNames.ToArray();
            }
        }
    }
}
