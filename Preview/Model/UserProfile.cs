using System;
using UnityEngine;

namespace SoFunny.FunnySDKPreview
{
    /// <summary>
    /// Represents a user profile used in FunnySDK.
    /// </summary>
    ///
    [Serializable]
    public class UserProfile
    {

#pragma warning disable 0649
        [SerializeField] private string user_id;

        [SerializeField] private string display_name;

        [SerializeField] private string picture_url;

        [SerializeField] private string status_message;

        [SerializeField] private string account;

        [SerializeField] private string amr;

        [SerializeField] private string platform;
#pragma warning restore 0649
        /// <summary>
        /// 用户 ID
        /// </summary>
        public string UserId { get { return user_id; } }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string DisplayName { get { return display_name; } }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string StatusMessage { get { return status_message; } }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string PictureUrl { get { return picture_url; } }

        /// <summary>
        /// 登录渠道
        /// </summary>
        public LoginType loginChannel
        {
            get
            {
                switch (platform)
                {

                    case "zyq":
                        {
                            if (amr == "gu")
                            {
                                return LoginType.Guest;
                            }
                            else
                            {
                                return LoginType.SoFunny;
                            }
                        }
                    case "ai": return LoginType.Apple;
                    case "fb": return LoginType.Facebook;
                    case "tw": return LoginType.Twitter;
                    case "gp": return LoginType.GooglePlay;
                    case "we": return LoginType.WeChat;
                    case "qq": return LoginType.QQ;
                    default: return LoginType.SoFunny;
                }
            }
        }

        /// <summary>
        /// 账号可显示的 Email 或手机号
        /// </summary>
        public string DisplayEmailOrPhone { get { return account; } }
    }
}

