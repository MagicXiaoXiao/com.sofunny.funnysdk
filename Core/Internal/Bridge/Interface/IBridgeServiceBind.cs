using System;
using SoFunny.FunnySDK.Promises;

namespace SoFunny.FunnySDK.Internal
{
    internal interface IBridgeServiceBind
    {
        /// <summary>
        /// 获取当前用户绑定信息
        /// </summary>
        /// <returns></returns>
        Promise<BindInfo> FetchBindInfo();

        /// <summary>
        /// 绑定相关账号
        /// </summary>
        /// <param name="bindable"></param>
        /// <returns></returns>
        Promise Binding(IBindable bindable);

        /// <summary>
        /// 强制绑定
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="bindCode"></param>
        /// <returns></returns>
        Promise<LoginResult> ForedBind(IBindable bindable, string bindCode);

    }
}

