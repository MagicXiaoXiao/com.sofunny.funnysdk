using System;

namespace SoFunny.FunnySDK
{
    public interface IFunnyUserCenterAPI
    {
        /// <summary>
        /// 用户中心打开时触发事件
        /// </summary>
        event Action OnOpenEvents;

        /// <summary>
        /// 用户中心关闭后触发事件
        /// </summary>
        event Action OnCloseEvents;

        /// <summary>
        /// 打开用户中心
        /// </summary>
        void Open();
    }
}

