using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    internal static class PCLoginView
    {
        internal static bool isLoaded = false;
        private static PCUILoginViewController _controller;
        private static LoginProvider[] _providers = new LoginProvider[] { };

        private static void Prepare()
        {
            if (!isLoaded)
            {
                var prefab = Resources.Load<GameObject>("FunnySDK/UI/LoginView/PCLoginController");
                GameObject instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = "FunnyLoginView";
                _controller = instance.GetComponent<PCUILoginViewController>();
                _controller.ExitAll();
                _controller.providers = _providers;
                isLoaded = true;
            }
        }

        internal static Action<string, string, bool> OnLoginWithPasswordAction;
        internal static Action<string, string> OnLoginWithCodeAction;
        internal static Action<LoginAccountRecord, bool> OnLoginWithRecordAction;
        internal static Action OnInvaidTokenResultAction;

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

        internal static void SetProviders(LoginProvider[] loginProviders)
        {
            _providers = loginProviders;
        }

        internal static void Open()
        {
            Open(PCLoginPage.LoginWithPassword());
        }

        internal static void Open(PCLoginPage page)
        {
            Prepare();
            _controller.Enter(page);
        }

        internal static void Close()
        {
            if (!isLoaded) return;

            _controller.Close();
            _controller = null;
        }
    }
}


