using System;
using SoFunny.FunnySDK.Promises;

namespace SoFunny.FunnySDK.Internal
{
    internal interface IBridgeServiceBind
    {

        /// <summary>
        /// 获取当前用户绑定信息
        /// </summary>
        /// <param name="handler"></param>
        void FetchBindInfo(ServiceCompletedHandler<BindInfo> handler);

        Promise<BindInfo> FetchBindInfo();

        void Binding(IBindable bindable, ServiceCompletedHandler<VoidObject> handler);

    }
}

