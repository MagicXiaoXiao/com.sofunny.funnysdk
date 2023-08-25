using System;

namespace SoFunny.FunnySDK
{
    public interface IFunnyBillboardAPI
    {
        /// <summary>
        /// 公告面板被打开时触发
        /// </summary>
        event Action OnOpenEvents;

        /// <summary>
        /// 公告面板被关闭后触发
        /// </summary>
        event Action OnCloseEvents;

        /// <summary>
        /// 打开公告面板
        /// </summary>
        void Open();

        /// <summary>
        /// 是否有公告内容消息
        /// </summary>
        /// <param name="action"></param>
        void FetchAny(Action<bool> action);
    }
}

