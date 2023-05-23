using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace SoFunny.Tools {

    public enum FadeType {
        FadeInOut,
        FadeOutIn
    }

    public class FunnyLaunchController : MonoBehaviour {

        [Tooltip("渐显形式")]
        [SerializeField] private FadeType fadeType;

        [Tooltip("渐显对象组")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Tooltip("LOGO 展现时间")]
        [Range(0.1f, 2f)]
        [SerializeField] private float fadeTime = 0.6f;

        [SerializeField] private VerticalLayoutGroup logoGroup;
        [SerializeField] private Image mainland;
        [SerializeField] private Image oversea;
        [SerializeField] private GridLayoutGroup tipsGroup;

        private void Awake() {
            canvasGroup.alpha = 0;
            mainland.gameObject.SetActive(false);
            oversea.gameObject.SetActive(false);

#if UNITY_EDITOR
            tipsGroup.gameObject.SetActive(FunnyLaunch.isMainland);
#else
            tipsGroup.gameObject.SetActive(FunnyLaunch.isMainland);
#endif

        }

        private void Start() {

            if (FunnyLaunch.isMainland) {
                mainland.gameObject.SetActive(true);
                logoGroup.padding.top = -80;
            }
            else {
                oversea.gameObject.SetActive(true);
                logoGroup.padding.top = 0;
            }

            UpdateTipsStyle();

            switch (fadeType) {
                case FadeType.FadeInOut:
                    StartCoroutine(FadeInAndOut());
                    break;
                case FadeType.FadeOutIn:
                    StartCoroutine(FadeOutAndIn());
                    break;
            }
        }

        private void Update() {
            UpdateTipsStyle();
        }

        private void UpdateTipsStyle() {
#if UNITY_EDITOR
            // 编辑器模式下处理
            if (Screen.width > Screen.height) {
                // 横屏样式
                logoGroup.padding.left = 300;
                logoGroup.padding.right = 300;

                tipsGroup.constraintCount = 2;
                tipsGroup.padding.bottom = 10;
            }
            else {
                // 竖屏样式
                logoGroup.padding.left = 100;
                logoGroup.padding.right = 100;

                tipsGroup.constraintCount = 4;
                tipsGroup.padding.bottom = 40;
            }
#else
        switch (Screen.orientation) {
            case ScreenOrientation.LandscapeLeft:
            case ScreenOrientation.LandscapeRight:
                    
                logoGroup.padding.left = 300;
                logoGroup.padding.right = 300;

                tipsGroup.constraintCount = 2;
                tipsGroup.padding.bottom = 10;

                break;
            case ScreenOrientation.Portrait:
                    
                logoGroup.padding.left = 100;
                logoGroup.padding.right = 100;
                    
                tipsGroup.constraintCount = 4;
                tipsGroup.padding.bottom = 40;

                break;
            default:
                tipsGroup.constraintCount = 4;
                tipsGroup.padding.bottom = 40;
                break;
            }
#endif

        }

        private IEnumerator FadeInAndOut() {

            yield return new WaitForSeconds(0.2f);

            for (float i = 0; i <= 1; i += Time.deltaTime) {
                canvasGroup.alpha = i;
                yield return null;
            }

            yield return new WaitForSeconds(fadeTime);

            for (float i = 1; i >= 0; i -= Time.deltaTime) {
                canvasGroup.alpha = i;
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);

            yield return CompletionHandler();
        }

        private IEnumerator FadeOutAndIn() {

            yield return new WaitForSeconds(0.2f);

            for (float i = 1; i >= 0; i -= Time.deltaTime) {
                canvasGroup.alpha = i;
                yield return null;
            }

            yield return new WaitForSeconds(fadeTime);

            for (float i = 0; i <= 1; i += Time.deltaTime) {
                canvasGroup.alpha = i;
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);

            yield return CompletionHandler();
        }

        private IEnumerator CompletionHandler() {
            if (SceneManager.sceneCount > 1) {
                var unload = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(FunnyLaunch.sceneName));
                while (!unload.isDone) {
                    yield return null;
                }
            }
            else {
                FunnyLaunch.CallFinish();
            }
        }

    }

}