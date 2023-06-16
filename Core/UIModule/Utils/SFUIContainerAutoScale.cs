using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    public class SFUIContainerAutoScale : MonoBehaviour
    {
        void Awake()
        {
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(0.75f, 0.75f, 1f);
#endif
        }
    }
}


