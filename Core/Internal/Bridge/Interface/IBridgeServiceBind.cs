using System;
namespace SoFunny.FunnySDK.Internal
{
    internal interface IBridgeServiceBind
    {

        /// <summary>
        /// 获取当前用户绑定信息
        /// </summary>
        /// <param name="handler"></param>
        void FetchBindInfo(ServiceCompletedHandler<BindInfo> handler);

        void Binding(IBindable bindable, ServiceCompletedHandler<VoidObject> handler);

    }
}

