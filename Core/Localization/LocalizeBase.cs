using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    internal abstract class LocalizeBase : MonoBehaviour
    {
        #region Public Fields

        public string localizationKey;
        public string enKey;
        public string zhKey;

        #endregion Public Fields

        public abstract void UpdateLocale();


        protected virtual void Start()
        {
            if (!Locale.CurrentLanguageHasBeenSet)
            {
                Locale.CurrentLanguageHasBeenSet = true;
                Locale.SetCurrentLanguage(Locale.PlayerLanguage);
            }

            string key = BridgeConfig.IsMainland ? zhKey : enKey;

            if (!string.IsNullOrEmpty(key))
            {
                localizationKey = key;
            }

            UpdateLocale();
        }

    }
}

