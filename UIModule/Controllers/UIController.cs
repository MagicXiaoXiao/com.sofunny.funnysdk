using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    public class UIController : MonoBehaviour
    {

        internal static UIController Instance;


        [SerializeField] internal GameObject UIContainer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            UIServicesInitializer.IsInitialized = false;
        }

    }
}

