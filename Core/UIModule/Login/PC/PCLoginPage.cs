using System;

namespace SoFunny.FunnySDK.UIModule
{
    internal class PCLoginPage
    {

        internal readonly UILoginPageState PageState;

        internal readonly string Message;

        private PCLoginPage(UILoginPageState pageState, string message = "")
        {
            PageState = pageState;
            Message = message;
        }

        internal static PCLoginPage LoginWithPassword(string message = "")
        {
            return new PCLoginPage(UILoginPageState.PwdLoginPage, message);
        }

        internal static PCLoginPage LoginWithCode(string message = "")
        {
            return new PCLoginPage(UILoginPageState.CodeLoginPage, message);
        }

        internal static PCLoginPage Register()
        {
            return new PCLoginPage(UILoginPageState.RegisterPage);
        }

        internal static PCLoginPage Retrieve()
        {
            return new PCLoginPage(UILoginPageState.RetrievePage);
        }

        internal static PCLoginPage ActCode()
        {
            return new PCLoginPage(UILoginPageState.ActivationKeyPage);
        }

        internal static PCLoginPage AccountLimit(string message)
        {
            return new PCLoginPage(UILoginPageState.LoginLimitPage, message);
        }

        internal static PCLoginPage AccountCooldown(string message)
        {
            return new PCLoginPage(UILoginPageState.CoolDownTipsPage, message);
        }

    }
}



