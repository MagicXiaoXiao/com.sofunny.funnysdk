using System;

namespace SoFunny.FunnySDK.UIModule
{
    internal interface IServiceLoginView
    {
        void Open(ILoginViewEvent loginViewEvent);

        void JumpTo(UILoginPageState pageState);

        void TimerSending(UILoginPageState pageState);
        void TimerStart(UILoginPageState pageState);
        void TimerReset(UILoginPageState pageState);
    }
}

