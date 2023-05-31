using System;
using UnityEngine;


namespace SoFunny.FunnySDK
{
    [Serializable]
    public class LoginLocalRecord
    {

#pragma warning disable 0649
        [SerializeField]
        internal string identifier;
        [SerializeField]
        internal string nickName;
        [SerializeField]
        internal UserProfile userProfile;
#pragma warning restore 0649

        internal LoginLocalRecord()
        {

        }

        /// <summary>
        /// 第三方平台昵称
        /// </summary>
        public string Nickname { get { return nickName; } }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserProfile Profile { get { return userProfile; } }
    }
}

