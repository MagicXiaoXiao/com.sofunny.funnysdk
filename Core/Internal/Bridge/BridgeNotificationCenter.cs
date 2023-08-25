using System;
using System.Collections.Generic;

#if UNITY_IOS && !UNITY_EDITOR

using AOT;
using System.Runtime.InteropServices;

#endif

namespace SoFunny.FunnySDK.Internal
{
    /// <summary>
    /// 桥接通知中心模块
    /// </summary>
    internal class BridgeNotificationCenter : IBridgeNotificationCenter
    {

        private static readonly object _lock = new object();
        private static BridgeNotificationCenter _instance;

        internal static BridgeNotificationCenter Default
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new BridgeNotificationCenter();
                    }
                }

                return _instance;
            }
        }


        private Dictionary<string, Dictionary<int, Action<BridgeValue>>> _actions;

        internal BridgeNotificationCenter()
        {
            _actions = new Dictionary<string, Dictionary<int, Action<BridgeValue>>>();

#if UNITY_IOS && !UNITY_EDITOR
            FSDK_NotificationCenter(PostNotificationHandler);
#endif
        }

        public void AddObserver(object observer, string name, Action action)
        {
            AddObserver(observer, name, (_) =>
            {
                action?.Invoke();
            });
        }

        public void AddObserver(object observer, string name, Action<BridgeValue> valueAction)
        {
            if (string.IsNullOrEmpty(name))
            {
                Logger.LogWarning("AddObserver - name 不可为空。");
                return;
            }

            if (!_actions.ContainsKey(name))
            {
                // 没有则创建
                _actions.Add(name, new Dictionary<int, Action<BridgeValue>>());
            }

            int hash = observer.GetHashCode();

            _actions[name][hash] = valueAction;
        }

        public void RemoveObserver(object observer)
        {
            if (observer is null) { return; }

            int hash = observer.GetHashCode();

            foreach (var item in _actions.Values)
            {
                if (item.ContainsKey(hash))
                {
                    item.Remove(hash);
                }
            }

        }

        public void RemoveObserver(object observer, string name)
        {
            if (observer is null) { return; }

            if (_actions.ContainsKey(name))
            {
                int hash = observer.GetHashCode();
                _actions[name].Remove(hash);
            }

        }

        public void Post(string name, BridgeValue value = null)
        {
            Logger.Log($"NotificationCenter - Post:{name} - Value:{value}");

            if (!_actions.ContainsKey(name)) { return; }

            if (value is null)
            {
                value = BridgeValue.Empty;
            }

            foreach (var action in _actions[name].Values)
            {
                action.Invoke(value);
            }
        }

#if UNITY_IOS && !UNITY_EDITOR

        private delegate void NotificationMessage(string name, string jsonString);

        [DllImport("__Internal")]
        private static extern void FSDK_NotificationCenter(NotificationMessage message);

        [MonoPInvokeCallback(typeof(NotificationMessage))]
        protected static void PostNotificationHandler(string name, string jsonString)
        {
            if (string.IsNullOrEmpty(name))
            {
                Logger.LogWarning("native event name is empty!");
                return;
            }
            BridgeNotificationCenter.Default.Post(name, BridgeValue.Create(jsonString));
        }

#endif
    }
}

