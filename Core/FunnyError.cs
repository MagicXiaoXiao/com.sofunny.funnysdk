using System;
using UnityEngine;

namespace SoFunny.FunnySDK
{

    [Serializable]
    internal class FunnyError {

        [SerializeField]
        internal int code;
        [SerializeField]
        internal string message;

        internal FunnyError(int code, string message) {
            this.code = code;
            this.message = message;

        }

        internal FunnySDKException MatchException() {
            FunnySDKException funnySDKException = new FunnySDKException(code, message);
            if (Enum.TryParse<FunnyErrorCode>(code.ToString(), out var funnyEnum)) {
                switch (funnyEnum) {
                    case FunnyErrorCode.LOGIN_INVALID_OR_EXPIRED:
                    case FunnyErrorCode.ACCESSTOKEN_INVALID:
                        return new AccessTokenInvalidException(code, message);
                    case FunnyErrorCode.LOGIN_CANCEL:
                        return new LoginCancelledException(code, message);
                    case FunnyErrorCode.NOT_LOGIN:
                        return new NotLoggedInException(code, message);
                    case FunnyErrorCode.PRIVACY_PROFILE_CANCEL:
                        return new PrivacyProfileCancelledException(code, message);
                    case FunnyErrorCode.PRIVACY_PROFILE_NOT_ENABLE:
                        return new PrivacyProfileDisableException(code, message);
                    case FunnyErrorCode.PRIVACY_PROFILE_ERROR:
                        return new PrivacyProfilePlatformException(code, message);
                    default:
                        return funnySDKException;
                }
            }
            else {
                return funnySDKException;
            }
        }

    }

    #region PrivacyProfile

    /// <summary>
    /// 隐私信息授权已被用户取消
    /// </summary>
    public class PrivacyProfileCancelledException : FunnySDKException {
        internal PrivacyProfileCancelledException(int code, string message) : base(code, message) { }
    }

    /// <summary>
    /// 隐私信息授权功能未开启
    /// </summary>
    public class PrivacyProfileDisableException : FunnySDKException {
        internal PrivacyProfileDisableException(int code, string message) : base(code, message) { }
    }

    /// <summary>
    /// 隐私信息授权平台出错
    /// </summary>
    public class PrivacyProfilePlatformException : FunnySDKException {
        internal PrivacyProfilePlatformException(int code, string message) : base(code, message) { }
    }

    #endregion

    #region Login

    /// <summary>
    /// 登录流程已被取消
    /// </summary>
    public class LoginCancelledException : FunnySDKException {
        internal LoginCancelledException(int code, string message) : base(code, message) { }
    }

    /// <summary>
    /// 未登录状态，无法使用服务
    /// </summary>
    public class NotLoggedInException : FunnySDKException {
        internal NotLoggedInException(int code, string message) : base(code, message) { }
    }

    /// <summary>
    /// AccessToken 状态已过期或无效
    /// </summary>
    public class AccessTokenInvalidException : FunnySDKException {
        internal AccessTokenInvalidException(int code, string message) : base(code, message) { }
    }

    #endregion

    /// <summary>
    /// FunnySDK 主要异常父类
    /// </summary>
    public class FunnySDKException : Exception {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code;

        internal FunnySDKException(int code, string message) : base(message) {
            this.Code = code;
        }
    }

    /// <summary>
    /// 登录过程被用户取消
    /// </summary>
    ///
    [Obsolete("该异常已无效，请将其替换为 LoginCancelledException", true)]
    public class FunnyErrorLoginCancel : FunnySDKException {
        public FunnyErrorLoginCancel(int code, string message) : base(code, message) { }
    }

    /// <summary>
    /// 未登录账号
    /// </summary>
    ///
    [Obsolete("该异常已无效，请将其替换为 NotLoggedInException", true)]
    public class FunnyErrorNotLoggedIn : FunnySDKException {
        public FunnyErrorNotLoggedIn(int code, string message) : base(code, message) { }
    }

    /// <summary>
    /// AccessToken 无效或已过期
    /// </summary>
    ///
    [Obsolete("该异常已无效，请将其替换为 AccessTokenInvalidException")]
    public class FunnyErrorTokenInvalid : FunnySDKException {
        public FunnyErrorTokenInvalid(int code, string message) : base(code, message) { }
    }

    [Obsolete("该异常已无效，请将其替换为 FunnySDKException")]
    public class FunnyErrorLoginException : Exception {
        public int Code;
        public FunnyErrorLoginException(int code, string message) : base(message) {
            this.Code = code;
        }
    }

}

