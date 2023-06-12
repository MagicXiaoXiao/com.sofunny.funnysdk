using System;
using UnityEngine;

namespace SoFunny.FunnySDK.UIModule
{
    public class AlertActionItem
    {
        public string label;
        public Action clickAction;

        public AlertActionItem(string label, Action clickAction = null)
        {
            this.label = label;
            this.clickAction = clickAction;
        }

    }
}

