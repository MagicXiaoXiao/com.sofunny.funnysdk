using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUIToast : MonoBehaviour
    {
        [Header("UI References :")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
        [SerializeField] private Text label;

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

        public void Init(string text, float duration, TextAnchor position = TextAnchor.MiddleCenter)
        {
            label.text = (text.Length > maxTextLength) ? text.Substring(0, maxTextLength) + "..." : text;

            verticalLayoutGroup.childAlignment = position;

            Dismiss();
            StartCoroutine(FadeInOut(duration, fadeDuration));
        }


        public void Dismiss()
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

