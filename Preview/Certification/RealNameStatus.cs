using System;

namespace SoFunny.FunnySDKPreview
{
    /// <summary>
    /// 实名认证状态
    /// </summary>
    public enum RealNameStatus
    {
        /// <summary>
        /// 未认证
        /// </summary>
        NotCertified = 0,
        /// <summary>
        /// 已认证
        /// </summary>
        Certified = 1,
        /// <summary>
        /// 认证当中等待结果
        /// </summary>
        Waiting = 2
    }
}

