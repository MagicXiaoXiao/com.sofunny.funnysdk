using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{

    internal static class LoginView
    {
        internal static bool isLoaded = false;

        private static HashSet<LoginProvider> _providers;
        private static SDKUILoginController _controller;

        private static void Prepare()
        {
            if (!isLoaded)
            {
                var prefab = Resources.Load<GameObject>("FunnySDK/UI/LoginView/LoginController");
                GameObject instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = "FunnyLoginView";
                _controller = instance.GetComponent<SDKUILoginController>();
                _controller.SetLoginProviders(_providers);
                isLoaded = true;
            }
        }


        internal static Action<UILoginPageState, UILoginPageState> OnOpenViewAction;
        internal static Action<string, string> OnLoginWithPasswordAction;
        internal static Action<string, string> OnLoginWithCodeAction;
        internal static Action<LoginProvider> OnLoginWithProviderAction;

        internal static Action<string, string, string> OnRegisterAccountAction;
        internal static Action<string, string, string> OnRetrievePasswordAction;

        internal static Action<string> OnCommitActivationAction;
        internal static Action OnReCallDeleteAction;

        internal static Action<UILoginPageState> OnCancelAction;
        internal static Action OnSwitchOtherAction;
        internal static Action<UILoginPageState, ServiceError> OnSendVerifcationCodeAction;

        internal static Action OnClickUserAgreenment;
        internal static Action OnClickPriacyProtocol;
        internal static Action OnClickContactUS;

        internal static void SetProviders(HashSet<LoginProvider> providers)
        {
            _providers = providers;
        }

        internal static void Open()
        {
            Prepare();
            _controller?.OpenPage(UILoginPageState.LoginSelectPage);
        }

        internal static void JumpTo(UILoginPageState pageState, object param = null)
        {
            Prepare();
            _controller?.OpenPage(pageState, param);
        }

        internal static void Close()
        {
            if (isLoaded)
            {
                _controller?.CloseLoginController();
                _controller = null;
            }
        }
    }
}

