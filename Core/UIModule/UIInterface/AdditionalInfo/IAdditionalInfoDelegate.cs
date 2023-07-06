using System;

namespace SoFunny.FunnySDK.UIModule
{
    internal interface IAdditionalInfoDelegate
    {
        /// <summary>
        /// 选择日期
        /// </summary>
        void OnShowDateView(string data);

        /// <summary>
        /// 提交信息
        /// </summary>
        /// <param name="sex">0=女,1=男</param>
        /// <param name="date">yyyy-MM-dd</param>
        void OnCommit(string sex, string date);

        /// <summary>
        /// 下次再说
        /// </summary>
        void OnNextTime();
    }
}

