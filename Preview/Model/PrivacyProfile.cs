using System;
using UnityEngine;

namespace SoFunny.FunnySDKPreview
{

    [Serializable]
    public class PrivacyProfile
    {

#pragma warning disable 0649
        [SerializeField] private string platform;
        [SerializeField] private string birthday;
        [SerializeField] private string sex;
#pragma warning restore 0649

        private PrivacyProfile()
        {
        }

        /// <summary>
        /// 授权目标平台
        /// </summary>
        public string AuthPlatform { get { return platform; } }

        /// <summary>
        /// 用户出生日期
        /// </summary>
        public string UserBirthday { get { return birthday; } }

        /// <summary>
        /// 用户性别
        /// </summary>
        public string UserSex { get { return sex; } }

    }
}

