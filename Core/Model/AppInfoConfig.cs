using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace SoFunny.FunnySDK
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class AppInfoConfig
    {
        [JsonProperty("nppa_enable")]
        public bool NPPA;
        [JsonProperty("provide_user_info_enable")]
        public bool ProvideUserInfo;
        [JsonProperty("app_auth_item")]
        public List<AppAuthItem> AuthItems;

        internal bool EnableAutoGuest
        {
            get
            {
                AppAuthItem item = AuthItems.DefaultIfEmpty().FirstOrDefault(x => x.Category == 11);

                if (item is null)
                {
                    return false;
                }
                else
                {
                    return item.Enable;
                }
            }
        }

        internal HashSet<LoginProvider> GetLoginProviders()
        {
            HashSet<LoginProvider> providers = new HashSet<LoginProvider>();

            if (AuthItems is null) { return providers; }

            foreach (var item in AuthItems)
            {
                if (item.Enable)
                {
                    providers.Add(item.Provider);
                }
            }

            return providers;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    internal class AppAuthItem
    {
        [JsonProperty("category")]
        public int Category;
        [JsonProperty("enable")]
        public bool Enable;

        public LoginProvider Provider
        {
            get
            {
                switch (Category)
                {
                    case 1: return LoginProvider.Facebook;
                    case 2: return LoginProvider.Twitter;
                    case 3: return LoginProvider.Google;
                    case 4: return LoginProvider.Apple;
                    case 5: return LoginProvider.Guest;
                    case 6: return LoginProvider.Email;
                    case 7: return LoginProvider.WeChat;
                    case 8: return LoginProvider.QQ;
                    case 9: return LoginProvider.Phone;
                    case 10: return LoginProvider.TapTap;
                    default: return LoginProvider.Unknown;
                }
            }
        }
    }
}

