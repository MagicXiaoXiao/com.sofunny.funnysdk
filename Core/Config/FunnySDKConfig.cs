﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    public class FunnySDKConfig : ScriptableObject
    {
        /// <summary>
        /// 游戏应用 ID
        /// </summary>
        public string AppID;
        /// <summary>
        /// 是否国内版本, 默认 true
        /// </summary>
        public bool IsMainland = true;

        #region 第三方平台配置

        public GoogleConfig Google;

        public FacebookConfig Facebook;

        public TwitterConfig Twitter;

        public AppleSignIn Apple;

        public WeChatConfig WeChat;

        public TencentQQConfig QQ;

        public TapTapConfig TapTap;

        #endregion

    }
}


