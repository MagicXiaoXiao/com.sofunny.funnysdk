using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    [RequireComponent(typeof(CanvasScaler))]
    public class SFCanvasScalerAutoMatch : MonoBehaviour
    {
        private CanvasScaler canvasScaler;

        void Awake()
        {
            canvasScaler = GetComponent<CanvasScaler>();

#if UNITY_IOS || UNITY_ANDROID
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f;
#endif

#if UNITY_STANDALONE || UNITY_STANDALONE_OSX
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
#endif
        }
    }
}


