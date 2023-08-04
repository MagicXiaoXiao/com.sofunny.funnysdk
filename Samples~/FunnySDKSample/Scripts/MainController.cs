using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoFunny.FunnySDKPreview;
using UnityEngine.UI;
using UnityEngine.Networking;
using SoFunny.FunnySDK;
using SoFunny.FunnySDK.UIModule;

public class MainController : MonoBehaviour, ILoginServiceDelegate, IUserServiceDelegate, IPrivateUserInfoDelegate
{

    public Image userIconImage;
    public Text displayNameText;
    public Text statusMessageText;
    public Text rawJsonText;
    public VerticalLayoutGroup layoutGroup;

    private bool launchValue = true;

    private void Awake()
    {

        Funny.Initialize();
        FunnySDK.Initialize();

        Funny.SetLanguage(SystemLanguage.English);
    }

    private void Start()
    {
#if UNITY_IOS
        layoutGroup.padding.top = (int)(Screen.safeArea.y / 2.5);
#else
        layoutGroup.padding.top = 20;
#endif
    }

    public void ShowNewLoginView()
    {
        // 发起登录
        Funny.Account.Login(this);
    }

    public void Logout()
    {
        Funny.Account.Logout();
        Toast.Show("账号已登出");
        ResetProfile();
    }

    public void CopyTokenValue()
    {
        var token = FunnySDK.GetCurrentAccessToken();
        if (token != null)
        {
            GUIUtility.systemCopyBuffer = token.Value;
            FunnyUtils.ShowTipsAlert("提示", "已复制到粘贴板");
        }
        else
        {
            FunnyUtils.ShowTipsAlert("提示", "未登录，无法复制");
        }
    }

    public void ShowBillboardH()
    {
        FunnySDK.OpenBillboardUI(BillboardStyle.horizontal);
    }

    public void ShowBillboardV()
    {
        FunnySDK.OpenFeedbackUI();
    }

    public async void HasBillMessage()
    {
        bool has = await FunnySDK.AnyBillMessage();
        FunnyUtils.ShowTipsAlert("消息", has ? "有公告内容" : "无公告内容");
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
        FunnySDK.OpenUserCenterUI();
    }

    public void AuthPrivacyProfile()
    {
        Funny.Account.GetPrivateUserInfo(this);
    }

    IEnumerator UpdateProfile(SoFunny.FunnySDK.UserProfile profile)
    {
        if (profile.PictureURL != null)
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

    public void OnLoginSuccess(SoFunny.FunnySDK.AccessToken accessToken)
    {
        rawJsonText.text = "登录成功 token 值为" + accessToken.Value;
        Funny.Account.GetUserProfile(this);
    }

    public void OnLoginCancel()
    {
        rawJsonText.text = "用户已取消登录";
    }

    public void OnLoginFailure(ServiceError error)
    {
        rawJsonText.text = "登录失败 - " + error.Message;
    }

    public void OnUserProfileSuccess(SoFunny.FunnySDK.UserProfile profile)
    {
        rawJsonText.text = "正在获取用户信息中....";

        StartCoroutine(UpdateProfile(profile));
    }

    public void OnUserProfileFailure(ServiceError error)
    {
        rawJsonText.text = error.Message;
    }

    public void OnConsentAuthPrivateInfo(UserPrivateInfo userInfo)
    {
        Alert.Show("提示", $"填写成功！性别：{userInfo.Gender} ，生日：{userInfo.Birthday}");
    }

    public void OnNextTime()
    {
        Toast.Show("下次一定填");
    }

    public void OnUnenabledService()
    {
        Toast.Show("隐私授权服务未开启");
    }

    public void OnPrivateInfoFailure(ServiceError error)
    {
        Toast.Show("隐私授权失败" + error.Message);
    }
}
