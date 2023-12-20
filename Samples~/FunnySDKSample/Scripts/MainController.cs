using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SoFunny.FunnySDK;
using SoFunny.FunnySDK.UIModule;
//using SoFunny.FunnySDK.IAP;
using System.Text;


public class MainController : MonoBehaviour
{

    public Image userIconImage;
    public Text displayNameText;
    public Text statusMessageText;
    public Text rawJsonText;
    public VerticalLayoutGroup layoutGroup;
    public Dropdown languageDropdown;

    private bool launchValue = true;

    private void Awake()
    {
        Funny.Initialize();

        Funny.Account.OnLoginEvents += Account_OnLoginEvents;
        Funny.Account.OnLogoutEvents += Account_OnLogoutEvents;
        Funny.Account.OnSwitchAccountEvents += Account_OnSwitchAccountEvents;
        Funny.Agreement.OnComfirmProtocolEvent += Agreement_OnComfirmProtocolEvent;
    }



    private void Agreement_OnComfirmProtocolEvent()
    {
        Toast.ShowSuccess("您已真有趣同意协议");
    }

    //private void Service_OnMissReceiptHandlerEvents(IAPReceipt[] obj)
    //{
    //    Toast.Show($"含有 {obj.Length} 条遗留凭据");
    //}

    private void Account_OnSwitchAccountEvents(AccessToken obj)
    {
        // 账号被切换事件
        StartCoroutine(UpdateProfile(Funny.Account.GetUserProfile()));
    }

    private void Account_OnLogoutEvents()
    {
        // 登出事件
        ResetProfile();
    }

    private void Account_OnLoginEvents(SoFunny.FunnySDK.AccessToken obj)
    {
        // 登录事件
        StartCoroutine(UpdateProfile(Funny.Account.GetUserProfile()));
    }

    private void Start()
    {
        OnUpdateLanguage();

        var token = Funny.Account.GetCurrentAccessToken();

        if (token != null)
        {
            StartCoroutine(UpdateProfile(Funny.Account.GetUserProfile()));
        }

#if UNITY_IOS
        layoutGroup.padding.top = (int)(Screen.safeArea.y / 2.5);
#else
        layoutGroup.padding.top = 20;
#endif
    }

    private SystemLanguage language = SystemLanguage.Chinese;

    public void SwitchLanguage()
    {
        if (language == SystemLanguage.Chinese)
        {
            language = SystemLanguage.English;
        }
        else
        {
            language = SystemLanguage.Chinese;
        }

        Funny.SetLanguage(language);

        rawJsonText.text = "切换成功！当前语言为 - " + language;

    }

    public void ShowNewLoginView()
    {
        // 发起登录
        Funny.Account.Login(OnLoginSuccess, OnLoginFailure, OnLoginCancel);
    }

    public void Logout()
    {
        Funny.Account.Logout();
    }

    public void OpenAgreeProtocolView()
    {
        Funny.Agreement.Open();
    }

    public void BindEmail()
    {
        if (Funny.Account.GetCurrentAccessToken() is null)
        {
            Alert.Show("提示", "当前未登录账号");
            return;
        }

        Funny.Account.Bind(BindingType.Email, () =>
        {
            rawJsonText.text = "绑定邮箱成功";
            Toast.ShowSuccess("绑定成功");
        }, (error) =>
        {
            rawJsonText.text = $"绑定失败：{error.Code} - {error.Message}";
            Toast.ShowFail("绑定失败");
        }, () =>
        {
            rawJsonText.text = "绑定已被取消";
            Toast.Show("取消绑定");
        });
    }

    public void BindAppleID()
    {
        if (Funny.Account.GetCurrentAccessToken() is null)
        {
            Alert.Show("提示", "当前未登录账号");
            return;
        }

        Loader.ShowIndicator();

        Funny.Account.Bind(BindingType.AppleID, () =>
        {
            Loader.HideIndicator();

            rawJsonText.text = "绑定 AppleID 成功";
            Toast.ShowSuccess("绑定成功");
        }, (error) =>
        {
            Loader.HideIndicator();

            rawJsonText.text = $"绑定失败： {error.Code} - {error.Message}";
            Toast.ShowFail("绑定失败");
        }, () =>
        {
            Loader.HideIndicator();

            rawJsonText.text = "绑定已被取消";
            Toast.Show("取消绑定");
        });
    }

    public void BindGoogleAccount()
    {
        if (Funny.Account.GetCurrentAccessToken() is null)
        {
            Alert.Show("提示", "当前未登录账号");
            return;
        }

        Loader.ShowIndicator();

        Funny.Account.Bind(BindingType.Google, () =>
        {
            Loader.HideIndicator();

            rawJsonText.text = "绑定 Google 成功";
            Toast.ShowSuccess("绑定成功");
        }, (error) =>
        {
            Loader.HideIndicator();

            rawJsonText.text = $"绑定失败： {error.Code} - {error.Message}";
            Toast.ShowFail("绑定失败");
        }, () =>
        {
            Loader.HideIndicator();

            rawJsonText.text = "绑定已被取消";
            Toast.Show("取消绑定");
        });
    }

    public void BindQQAccount()
    {
        if (Funny.Account.GetCurrentAccessToken() is null)
        {
            Alert.Show("提示", "当前未登录账号");
            return;
        }

        Loader.ShowIndicator();

        Funny.Account.Bind(BindingType.QQ, () =>
        {
            Loader.HideIndicator();

            rawJsonText.text = "绑定 QQ 成功";
            Toast.ShowSuccess("绑定成功");
        }, (error) =>
        {
            Loader.HideIndicator();

            rawJsonText.text = $"绑定失败： {error.Code} - {error.Message}";
            Toast.ShowFail("绑定失败");
        }, () =>
        {
            Loader.HideIndicator();

            rawJsonText.text = "绑定已被取消";
            Toast.Show("取消绑定");
        });
    }

    public void BindWechatAccount()
    {
        if (Funny.Account.GetCurrentAccessToken() is null)
        {
            Alert.Show("提示", "当前未登录账号");
            return;
        }

        Loader.ShowIndicator();

        Funny.Account.Bind(BindingType.WeChat, () =>
        {
            Loader.HideIndicator();

            rawJsonText.text = "绑定 WeChat 成功";
            Toast.ShowSuccess("绑定成功");
        }, (error) =>
        {
            Loader.HideIndicator();

            rawJsonText.text = $"绑定失败： {error.Code} - {error.Message}";
            Toast.ShowFail("绑定失败");
        }, () =>
        {
            Loader.HideIndicator();

            rawJsonText.text = "绑定已被取消";
            Toast.Show("取消绑定");
        });
    }

    public void BindStatusData()
    {
        if (Funny.Account.GetCurrentAccessToken() is null)
        {
            Alert.Show("提示", "当前未登录账号");
            return;
        }

        var status = Funny.Account.GetBindStatus();
        var sb = new StringBuilder();

        foreach (var item in status)
        {
            string flag = item.IsBind ? "<color=green>已绑定</color>" : "<color=red>未绑定</color>";
            sb.Append($"类型：{item.Type} | {flag}");
            sb.AppendLine();
        }

        Alert.Show("绑定状态", sb.ToString());
    }

    public void CopyTokenValue()
    {
        var token = Funny.Account.GetCurrentAccessToken();

        if (token is null)
        {
            rawJsonText.text = "当前未登录，无法进行复制";
        }
        else
        {
            GUIUtility.systemCopyBuffer = token.Value;

            rawJsonText.text = "已复制到粘贴板 - " + token.Value;
        }
    }

    public void ShowBillboardH()
    {
        Funny.Billboard.Open();
    }

    public void ShowBillboardV()
    {
        Funny.Feedback.Open();
    }

    public void HasBillMessage()
    {
        Funny.Billboard.FetchAny((value) =>
        {
            rawJsonText.text = value ? "有公告内容" : "无公告内容";
        });
    }

    public void CleanDatasView()
    {
        rawJsonText.text = "";
    }

    public void ShowLaunchScreen()
    {
        launchValue = !launchValue;
        FunnyLaunch.Show(launchValue, () =>
        {
            Debug.Log("开屏动画完成");
        });
    }

    public void OpenUserCenter()
    {
        Funny.UserCenter.Open();
    }

    public void AuthPrivacyProfile()
    {
        Funny.Account.AuthPrivateUserInfo(OnConsentAuthPrivateInfo, OnNextTime);
    }

    IEnumerator UpdateProfile(SoFunny.FunnySDK.UserProfile profile)
    {
        if (!string.IsNullOrEmpty(profile.PictureURL))
        {
            var www = UnityWebRequestTexture.GetTexture(profile.PictureURL);
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            switch (www.result)
            {
                case UnityWebRequest.Result.Success:
                    var texture = DownloadHandlerTexture.GetContent(www);
                    userIconImage.color = Color.white;
                    userIconImage.sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0, 0));

                    rawJsonText.text = "获取用户信息成功";
                    break;
                default:
                    Debug.LogError(www.error);
                    rawJsonText.text = "获取用户信息失败：" + www.error;
                    break;
            }
#else
            if (www.isDone && !www.isNetworkError)
            {
                var texture = DownloadHandlerTexture.GetContent(www);
                userIconImage.color = Color.white;
                userIconImage.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0, 0));

                rawJsonText.text = "获取用户信息成功";
            }
            else
            {
                Debug.LogError(www.error);
                rawJsonText.text = "获取用户信息失败：" + www.error;
            }
#endif
        }
        else
        {
            rawJsonText.text = "获取用户信息成功";
            yield return null;
        }

        displayNameText.text = profile.DispalyName;
        statusMessageText.text = profile.StatusMessage;

    }

    void ResetProfile()
    {
        userIconImage.color = Color.gray;
        userIconImage.sprite = null;
        displayNameText.text = "Display Name";
        statusMessageText.text = "Status Message";
        rawJsonText.text = "";
    }

    void UpdateRawSection(object obj)
    {
        if (obj == null)
        {
            rawJsonText.text = "null";
            return;
        }

        var text = JsonUtility.ToJson(obj, true);
        if (text == null)
        {
            rawJsonText.text = "Invalid Object";
            return;
        }
        rawJsonText.text = text;
        var scrollContentTransform = (RectTransform)rawJsonText.gameObject.transform.parent;
        scrollContentTransform.localPosition = Vector3.zero;
    }

    public void OnUpdateLanguage()
    {
        switch (languageDropdown.value)
        {
            case 0:
                Funny.SetLanguage(SystemLanguage.ChineseSimplified);
                break;
            case 1:
                Funny.SetLanguage(SystemLanguage.ChineseTraditional);
                break;
            case 2:
                Funny.SetLanguage(SystemLanguage.English);
                break;
            case 3:
                Funny.SetLanguage(SystemLanguage.Thai);
                break;
            case 4:
                Funny.SetLanguage(SystemLanguage.Vietnamese);
                break;
            case 5:
                Funny.SetLanguage(SystemLanguage.Indonesian);
                break;
            default:
                break;
        }
    }

    public void OnLoginSuccess(SoFunny.FunnySDK.AccessToken accessToken)
    {
        rawJsonText.text = "登录成功 token 值为" + accessToken.Value;
    }

    public void OnLoginCancel()
    {
        rawJsonText.text = "用户已取消登录";
    }

    public void OnLoginFailure(ServiceError error)
    {
        rawJsonText.text = "登录失败 - " + error.Message;
    }

    public void OnConsentAuthPrivateInfo(UserPrivateInfo userInfo)
    {
        Alert.Show("提示", $"填写成功！性别：{userInfo.Gender} ，生日：{userInfo.Birthday}");
    }

    public void OnNextTime(bool enableService)
    {
        if (enableService)
        {
            Toast.Show("下次一定填");
        }
        else
        {
            Toast.Show("隐私授权服务未开启");
        }
    }

    public void ExecutePayment()
    {

        //IAPProduct product = new IAPProduct("funny.product.item1", "Unity 测试商品");
        //IAPPayer payer = new IAPPayer("0002", "Unity 测试员", "测试区");
        //IAPOrder funnyOrder = IAPOrder.Create(product, payer, PaymentType.AppleInAppPurchase, 1);

        //Loader.ShowIndicator();

        //FunnyIAP.Service.Execute(
        //    funnyOrder,
        //    (receipt, order) =>
        //    {
        //        Loader.HideIndicator();

        //        Alert.Show("购买成功", $"支付凭据编号：{receipt.Id}");

        //        rawJsonText.text = $"支付凭据编号：{receipt.Id}";
        //    },
        //    () =>
        //    {
        //        Loader.HideIndicator();
        //        Alert.Show("提示", $"购买已被取消");
        //        rawJsonText.text = "购买已被取消";
        //    },
        //    (error) =>
        //    {
        //        Loader.HideIndicator();
        //        Alert.Show("购买失败", $"购买失败：{error.Code} - {error.Message}");
        //        rawJsonText.text = $"购买失败：{error.Code} - {error.Message}";
        //    }
        //);
    }

    public void CheckMissReceipt()
    {
        //FunnyIAP.Service.CheckMissReceiptQueue();
    }

}
