﻿using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    [Serializable]
    public struct GoogleConfig
    {
        [SerializeField]
        public string idToken;

        public GoogleConfig(string idToken)
        {
            this.idToken = idToken;
        }
        /// <summary>
        /// 是否设置了 Google 相关参数
        /// </summary>
        public bool Enable
        {
            get
            {
                return !string.IsNullOrEmpty(idToken);
            }
        }
    }
}
