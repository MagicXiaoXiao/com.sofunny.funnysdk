using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    public partial class SDKUILoginController : MonoBehaviour
    {
        [SerializeField] private List<SDKUILoginBase> LoginViewList;

        //public SDKUILoginSelectView loginSelectView;
        //public SDKUIRegisterAndRetrieveView registerAndRetrieveView;
        //public SDKUIEmailOrPhonePwdView emailOrPhonePwdView;
        //public SDKUILoginLimitView loginLimitView;
        //public SDKUICoolDownTipsView coolDownTipsView;
        //public SDKUIActivationKeyView activationKeyView;
        //public SDKUIAntiAddictionView antiAddictionView;

        private SDKUILoginBase CurrentView;
        //private UILoginPageState currentPageState;

        private SDKUILoginBase TryGetLoginView(UILoginViewType type)
        {
            var findView = LoginViewList.First((view) =>
            {
                return view.ViewType == type;
            });

            if (findView == null)
            {
                Logger.LogError($"获取 UI 视图出错 -> {type}");
            }

            return findView;
        }

        public void OpenPage(UILoginPageState pageState)
        {

            switch (pageState)
            {
                case UILoginPageState.LoginSelectPage:
                    break;
                default:
                    break;
            }
        }

        public void OpenLoginView(UILoginViewType viewType)
        {
            var view = TryGetLoginView(viewType);

            if (CurrentView == null)
            {
                view.Show();
            }
            else if (CurrentView.ViewType == viewType)
            {
                return;
            }
            else
            {
                CurrentView.Hide();
                view.Show();
            }

            CurrentView = view;
        }

        public void CloseLoginController()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            LoginUIService.display = false;
        }

    }
}


