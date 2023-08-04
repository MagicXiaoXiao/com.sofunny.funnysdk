using System;
namespace SoFunny.FunnySDK.Internal
{
    public enum CodeAction
    {
        /// <summary>
        /// 登录
        /// </summary>
        Login = 1,
        /// <summary>
        /// 注册
        /// </summary>
        Signup = 2,
        /// <summary>
        /// 绑定邮箱
        /// </summary>
        BindEmail = 10,
        /// <summary>
        /// 修改绑定手机
        /// </summary>
        ChangePhone = 11,
        /// <summary>
        /// 修改密码
        /// </summary>
        ChangePassword = 12,
        /// <summary>
        /// 确认手机
        /// </summary>
        ConfirmPhone = 13,
        /// <summary>
        /// 确认邮箱
        /// </summary>
        ConfirmEmail = 15,
        /// <summary>
        /// 游客绑定邮箱
        /// </summary>
        GuestBindEmail = 16,
        /// <summary>
        /// 永久删除账号
        /// </summary>
        DeleteAccount = 17,
        /// <summary>
        /// 第三方登录绑定
        /// </summary>
        LoginBindAccount = 18
    }
}

