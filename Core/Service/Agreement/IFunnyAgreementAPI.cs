using System;

namespace SoFunny.FunnySDK
{
    public interface IFunnyAgreementAPI
    {
        /// <summary>
        /// 用户选择同意时触发
        /// </summary>
        event Action OnComfirmProtocolEvent;

        /// <summary>
        /// 打开 SoFunny 用户协议与隐私政策提示界面
        /// </summary>
        void Open();
    }
}

