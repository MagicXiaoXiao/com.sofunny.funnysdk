using System;

namespace SoFunny.FunnySDK
{
    public interface IFunnyFeedbackAPI
    {
        /// <summary>
        /// 问题反馈打开后触发事件
        /// </summary>
        event Action OnOpenEvents;

        /// <summary>
        /// 问题反馈关闭后触发事件
        /// </summary>
        event Action OnCloseEvents;

        /// <summary>
        /// 打开问题反馈
        /// </summary>
        void Open(string id = "");
    }
}

