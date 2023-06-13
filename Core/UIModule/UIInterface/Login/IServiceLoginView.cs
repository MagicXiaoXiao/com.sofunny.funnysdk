using System;
using System.Collections;
using System.Collections.Generic;

namespace SoFunny.FunnySDK.UIModule
{
    internal interface IServiceLoginView
    {
        void Open(ILoginViewEvent loginViewEvent, HashSet<LoginProvider> providers);

        void CloseView();

        void JumpTo(UILoginPageState pageState);

        void TimerSending(UILoginPageState pageState);
        void TimerStart(UILoginPageState pageState);
        void TimerReset(UILoginPageState pageState);
    }
}

