using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SoFunny.FunnySDK
{

    internal class Native
    {
        private static readonly object _lock = new object();

        private readonly NativeCallMethodInterface _bridge;
        private static Native _instance;

        internal static Native GetInstance()
        {

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Native();
                    _instance._bridge.RegisterListener();
                }
            }
            return _instance;
        }

        internal Native()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
#if UNITY_ANDROID
                    _bridge = AndroidBridge.GetInstance();
#endif
                    break;
                case RuntimePlatform.IPhonePlayer:
#if UNITY_IOS
                    _bridge = iOSBridge.GetInstance();
#endif
                    break;
                default:
                    _bridge = OtherBridge.GetInstance();
                    break;
            }
        }

        internal void SetupSDK(string appID)
        {
            if (!Application.isPlaying) { return; }
            var parameter = NativeParameter.Builder().Add("appID", appID);
            _bridge.Call("setupSDK", parameter);
        }

        internal void SetupSDK()
        {
            if (!Application.isPlaying) { return; }
            _bridge.Call("setupSDK");
        }

        private Task<WrapperArray<T>> CallWrapperArray<T>(string method)
        {
            var interopTask = InteropTasks.Create<WrapperArray<T>>(out var taskID);
            var parameter = NativeParameter.Builder().TaskID(taskID);
            _bridge.Call<WrapperArray<LoginLocalRecord>>(method, parameter);
            return interopTask.Task;
        }

        #region Login And Logout

        internal Task<AccessToken> Login(int type)
        {
            var interopTask = InteropTasks.Create<AccessToken>(out var taskID);

            var parameter = NativeParameter.Builder()
                .TaskID(taskID)
                .Add("type", type);

            _bridge.Call<AccessToken>("login", parameter);

            return interopTask.Task;
        }

        internal Task<bool> Logout()
        {
            var interopTask = InteropTasks.Create<bool>(out var taskID);
            var parameter = NativeParameter.Builder().TaskID(taskID);
            _bridge.Call<bool>("logout", parameter);
            return interopTask.Task;
        }

        #endregion

        #region AccessToken


        internal Task<UserProfile> GetProfile()
        {
            var interopTask = InteropTasks.Create<UserProfile>(out var taskID);
            var parameter = NativeParameter.Builder().TaskID(taskID);
            _bridge.Call<UserProfile>("getProfile", parameter);
            return interopTask.Task;
        }

        internal Task<PrivacyProfile> AuthPrivacyProfile()
        {
            var interopTask = InteropTasks.Create<PrivacyProfile>(out var taskID);
            var parameter = NativeParameter.Builder().TaskID(taskID);
            _bridge.Call<PrivacyProfile>("privacyProfile", parameter);
            return interopTask.Task;
        }


        internal AccessToken GetCurrentAccessToken()
        {
            return _bridge.CallReturn<AccessToken>("getCurrentAccessToken");
        }

        #endregion

        #region UserCenter

        internal void OpenUserCenter()
        {
            _bridge.Call("openUserCenter");
        }

        internal void CloseUserCenter()
        {
            _bridge.Call("closeUserCenter");
        }

        #endregion

        #region Billboard

        internal void OpenBillboard()
        {
            _bridge.Call("openBillboard");
        }

        internal Task<bool> AnyBillMessage()
        {
            var interopTask = InteropTasks.Create<bool>(out var taskID);
            var parameter = NativeParameter.Builder().TaskID(taskID);
            _bridge.Call<bool>("anyBillMessage", parameter);
            return interopTask.Task;
        }

        #endregion

        #region Feedback
        internal void OpenFeedback(string playerID)
        {
            var parameter = NativeParameter.Builder().Add("playerID", playerID);
            _bridge.Call("openFeedback", parameter);
        }
        #endregion

        internal void OpenAlertView(string title, string content)
        {
            var parameter = NativeParameter.Builder().Add("title", title).Add("content", content);
            _bridge.Call("showAlert", parameter);
        }

        internal void ShowToast(string message, float time = 0)
        {
            var parameter = NativeParameter.Builder().Add("message", message).Add("time", time);
            _bridge.Call("showToast", parameter);
        }
    }
}

