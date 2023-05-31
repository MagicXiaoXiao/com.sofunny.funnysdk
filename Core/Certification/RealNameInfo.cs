using System;
using UnityEngine;

namespace SoFunny.FunnySDK {

    [Serializable]
    public class RealNameInfo {
#pragma warning disable 0649

        [SerializeField] private bool adult;

        [SerializeField] private RealNameStatus certification_status;

        [SerializeField] private string pi;

        [SerializeField] private int remainTime;

#pragma warning restore 0649

        public RealNameInfo() {

        }

        /// <summary>
        /// 是否成年 true = 成年，false = 未成年
        /// </summary>
        public bool Adult { get { return adult; } }

        /// <summary>
        /// 认证状态
        /// </summary>
        public RealNameStatus Status { get { return certification_status; } }

        /// <summary>
        /// 中宣部唯一标识
        /// </summary>
        public string PI { get { return pi; } }

        /// <summary>
        /// 剩余可游玩时间(秒)
        /// </summary>
        public int RemainTime { get { return remainTime; } }

    }
}

