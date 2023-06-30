using System;

namespace SoFunny.FunnySDK.UIModule
{
    internal interface IServiceAdditionalInfoView
    {
        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="infoDelegate"></param>
        void Open(IAdditionalInfoDelegate infoDelegate);

        /// <summary>
        /// 关闭界面
        /// </summary>
        void Close();

        /// <summary>
        /// 设置日期
        /// </summary>
        /// <param name="date"></param>
        void SetDateValue(string date);
    }
}

