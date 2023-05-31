using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{
    [Serializable]
    public class LimitConfig
    {
#pragma warning disable 0649
        [SerializeField] private bool account_enable;

        [SerializeField] private bool account_pass;

        [SerializeField] private bool ip_enable;

        [SerializeField] private bool ip_pass;

        [SerializeField] private bool zone_enable;

        [SerializeField] private bool zone_pass;
#pragma warning restore 0649

        public LimitConfig()
        {

        }

        /// <summary>
        /// 是否开启账号白名单
        /// </summary>
        public bool AccountEnable { get { return account_enable; } }
        /// <summary>
        /// 账号白名单是否效验通过
        /// </summary>
        public bool AccountPass { get { return account_pass; } }
        /// <summary>
        /// 是否开启 IP 白名单
        /// </summary>
        public bool IpEnable { get { return ip_enable; } }
        /// <summary>
        /// IP 白名单效验是否通过
        /// </summary>
        public bool IpPass { get { return ip_pass; } }
        /// <summary>
        /// 是否开启区域限制
        /// </summary>
        public bool ZoneEnable { get { return zone_enable; } }
        /// <summary>
        /// 区域限制是否通过
        /// </summary>
        public bool ZonePass { get { return zone_pass; } }

        /// <summary>
        /// 任意限制效验是否通过
        /// </summary>
        public bool IsPassAny
        {
            get
            {
                if (!account_enable && !ip_enable && !zone_enable)
                {
                    return true;
                }
                else if (account_enable && account_pass)
                {
                    return true;
                }
                else if (ip_enable && ip_pass)
                {
                    return true;
                }
                else if (zone_enable && zone_pass)
                {
                    return true;
                }

                return false;
            }
        }

    }
}

