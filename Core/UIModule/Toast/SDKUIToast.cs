using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUIToast : MonoBehaviour
    {

        [Header("UI References :")]
        public CanvasGroup canvasGroup;
        public VerticalLayoutGroup verticalLayoutGroup;
        public Image failLogo;
        public Image successLogo;
        public Text label;

        [Header("Toast Fade In/Out Duration :")]
        [Range(.1f, .8f)]
        [SerializeField] private float fadeDuration = .3f;

        private int maxTextLength = 300;

        private void Awake()
        {
            canvasGroup.alpha = 0f;
        }

        private void OnDestroy()
        {
            Toast.isLoaded = false;
        }

        internal void Init(string text, float duration, TextAnchor position = TextAnchor.MiddleCenter, ToastStyle style = ToastStyle.Normal)
        {
            label.text = (text.Length > maxTextLength) ? text.Substring(0, maxTextLength) + "..." : text;

            verticalLayoutGroup.childAlignment = position;

            switch (style)
            {
                case ToastStyle.Normal:
                    successLogo.gameObject.SetActive(false);
                    failLogo.gameObject.SetActive(false);
                    break;
                case ToastStyle.Fail:
                    successLogo.gameObject.SetActive(false);
                    failLogo.gameObject.SetActive(true);
                    break;
                case ToastStyle.Success:
                    successLogo.gameObject.SetActive(true);
                    failLogo.gameObject.SetActive(false);
                    break;
                default: break;
            }

            Dismiss();
            StartCoroutine(FadeInOut(duration, fadeDuration));
        }

        internal void Dismiss()
        {
            StopAllCoroutines();
            canvasGroup.alpha = 0f;
        }


        private IEnumerator FadeInOut(float toastDuration, float fadeDuration)
        {
            yield return null;
            verticalLayoutGroup.CalculateLayoutInputHorizontal();
            verticalLayoutGroup.CalculateLayoutInputVertical();
            verticalLayoutGroup.SetLayoutHorizontal();
            verticalLayoutGroup.SetLayoutVertical();
            yield return null;
            // Anim start
            yield return Fade(canvasGroup, 0f, 1f, fadeDuration);
            yield return new WaitForSeconds(toastDuration);
            yield return Fade(canvasGroup, 1f, 0f, fadeDuration);
            // Anim end
        }

        private IEnumerator Fade(CanvasGroup cGroup, float startAlpha, float endAlpha, float fadeDuration)
        {
            float startTime = Time.time;
            float alpha = startAlpha;

            if (fadeDuration > 0f)
            {
                //Anim start
                while (alpha != endAlpha)
                {
                    alpha = Mathf.Lerp(startAlpha, endAlpha, (Time.time - startTime) / fadeDuration);
                    cGroup.alpha = alpha;

                    yield return null;
                }
            }

            cGroup.alpha = endAlpha;
        }
    }
}

