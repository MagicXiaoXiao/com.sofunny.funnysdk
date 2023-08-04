using System;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK
{
    [RequireComponent(typeof(Text))]
    internal class FunnyLocalize : LocalizeBase
    {
        private Text _text;

        public override void UpdateLocale()
        {
            if (!_text) return; // catching race condition
            if (!string.IsNullOrEmpty(localizationKey) && Locale.CurrentLanguageStrings.ContainsKey(localizationKey))
                _text.text = Locale.CurrentLanguageStrings[localizationKey].Replace(@"\n", "" + '\n'); ;
        }

        protected override void Start()
        {
            _text = GetComponent<Text>();
            base.Start();
        }

    }
}

