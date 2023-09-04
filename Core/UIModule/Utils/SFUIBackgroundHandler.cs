using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    [RequireComponent(typeof(Image))]
    public class SFUIBackgroundHandler : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent onClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }

    }
}


