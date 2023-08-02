using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    internal abstract class LocalizeBase : MonoBehaviour
    {
        #region Public Fields

        public string localizationKey;

        #endregion Public Fields

        public abstract void UpdateLocale();


        protected virtual void Start()
        {
            if (!Locale.CurrentLanguageHasBeenSet)
            {
                Locale.CurrentLanguageHasBeenSet = true;
                Locale.SetCurrentLanguage(Locale.PlayerLanguage);
            }

            UpdateLocale();
        }

    }
}

