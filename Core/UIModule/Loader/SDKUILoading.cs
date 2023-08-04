using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUILoading : MonoBehaviour
    {
        public Image circleView;
        public float rotationSpeed = 200f;

        private Coroutine rotationCoroutine;
        private bool isShow = false;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Loader.isLoaded = false;
        }

        public void Show()
        {
            if (isShow) { return; }

            gameObject.SetActive(true);
            rotationCoroutine = StartCoroutine(Rotate());
            isShow = true;
        }

        public void Dismiss()
        {
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
            }
            isShow = false;
            gameObject.SetActive(false);
        }

        private IEnumerator Rotate()
        {
            while (true)
            {
                circleView.transform.Rotate(-Vector3.forward, rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}


