using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{

    internal abstract class SDKUILoginBase : MonoBehaviour
    {

        protected SDKUILoginController Controller;

        private void Awake()
        {
            Controller = GetComponentInParent<SDKUILoginController>();
            Init();
        }

        private void OnDestroy()
        {
            DeInit();
        }

        protected abstract void Init();
        protected virtual void DeInit() { }

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


