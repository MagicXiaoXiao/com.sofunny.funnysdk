using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SoFunny.FunnySDK
{

    internal class NativeParameter {
        private Dictionary<string, object> mainDic;
        private Dictionary<string, object> parameters;

        internal NativeParameter() {
            this.mainDic = new Dictionary<string, object>();
            this.parameters = new Dictionary<string, object>();
            mainDic["parameters"] = parameters;
        }

        internal static NativeParameter Builder() {
            return new NativeParameter();
        }

        internal NativeParameter TaskID(string id) {
            mainDic["taskID"] = id;
            return this;
        }

        internal NativeParameter Add(string key, object value) {
            parameters.Add(key, value);
            return this;
        }

        internal NativeParameter Add(Dictionary<string, object> dic) {
            foreach (var item in dic) {
                parameters[item.Key] = item.Value;
            }
            return this;
        }

        internal string GetTaskID() {
            return mainDic["taskID"] as string;
        }

        internal string ToJSON() {
            return Json.Serialize(mainDic);
        }

        internal Dictionary<string, object> getMainDic() {
            return mainDic;
        }
    }
}

