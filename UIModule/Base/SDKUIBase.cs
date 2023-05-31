using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    public abstract class SDKUIBase : MonoBehaviour
    {
        public abstract string UIType { get; }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

    }
}


