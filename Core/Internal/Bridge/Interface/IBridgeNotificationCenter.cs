using System;

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 桥接通知接口
    /// </summary>
    internal interface IBridgeNotificationCenter
    {
        /// <summary>
        /// 添加通知观察者
        /// </summary>
        /// <param name="observer">观察者对象</param>
        /// <param name="name">通知名</param>
        /// <param name="action">触发函数</param>
        void AddObserver(object observer, string name, Action action);

        /// <summary>
        /// 添加通知观察者
        /// </summary>
        /// <param name="observer">观察者对象</param>
        /// <param name="name">通知名</param>
        /// <param name="action">触发函数(带参形式)</param>
        void AddObserver(object observer, string name, Action<BridgeValue> action);

        /// <summary>
        /// 移除对应观察对象的所有通知
        /// </summary>
        /// <param name="observer"></param>
        void RemoveObserver(object observer);

        /// <summary>
        /// 移除通知
        /// </summary>
        /// <param name="observer">观察者对象</param>
        /// <param name="name">通知名</param>
        void RemoveObserver(object observer, string name);

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="name">通知名</param>
        /// <param name="value">携带参数</param>
        void Post(string name, BridgeValue value = null);
    }
}

