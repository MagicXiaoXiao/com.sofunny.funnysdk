using System;

namespace SoFunny.FunnySDKPreview
{

    public class FunnyUtils
    {
        private FunnyUtils()
        {
        }

        /// <summary>
        /// 显示系统弹窗提示
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public static void ShowTipsAlert(string title, string content)
        {
            Native.GetInstance().OpenAlertView(title, content);
        }

        /// <summary>
        /// 展示 Toast 提示
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="time">持续时间</param>
        public static void ShowToast(string message, float time = 0)
        {
            Native.GetInstance().ShowToast(message, time);
        }

    }
}

