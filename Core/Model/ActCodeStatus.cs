using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{

    [Serializable]
    public class ActCodeStatus {

        public ActCodeStatus() {
        }

#pragma warning disable 0649
        [SerializeField]
        private bool activation_enable;
        [SerializeField]
        private bool user_activation;
#pragma warning restore 0649
        /// <summary>
        /// 是否开启邀请码
        /// </summary>
        public bool Enable { get { return activation_enable; } }
        /// <summary>
        /// 激活码有效状态
        /// </summary>
        public bool IsValid { get { return user_activation; } }
        /// <summary>
        /// 是否需要填写邀请码
        /// </summary>
        public bool NeedSet {
            get {
                if (activation_enable) {
                    if (user_activation) {
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return false;
                }
            }
        }

    }
}

