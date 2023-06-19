using System;
using System.Collections;
using System.Collections.Generic;

namespace SoFunny.FunnySDK.UIModule
{
    internal interface IServiceLoginView
    {
        void SetupLoginConfig(ILoginViewEvent loginViewEvent, HashSet<LoginProvider> providers);

        void Open();

        void CloseView();

        void JumpTo(UILoginPageState pageState, object param = null);

        void TimerSending(UILoginPageState pageState);
        void TimerStart(UILoginPageState pageState);
        void TimerReset(UILoginPageState pageState);
    }
}

