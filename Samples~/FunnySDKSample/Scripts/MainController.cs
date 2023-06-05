using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.Linq;
using SoFunny.Tools;
using SoFunny;
using SoFunny.Utils;

public class MainController : MonoBehaviour {

    public Image userIconImage;
    public Text displayNameText;
    public Text statusMessageText;
    public Text rawJsonText;
    public VerticalLayoutGroup layoutGroup;

    private bool launchValue = false;
    private void Awake() {
        /// 初始化 SDK 
        //FunnySDK.InitializeSDK("900000000");
        FunnySDK.OnConfirmProtocolEvent += FunnySDK_OnConfirmProtocolEvent;
        FunnyLaunch.Show(launchValue, () =>
        {
            Debug.Log("开屏动画完成");
            FunnySDK.ShowProtocol();
        });
    }

    private void initSDKAfterProtocol()
    {
        FunnySDK.Initialize();
        /// SDK 事件监听
        FunnySDK.OnLogoutEvent += OnLogoutEvent;

        FunnySDK.OnLoginEvent += OnLoginEvent;

        FunnySDK.OnOpenUserCenterEvent += OnOpenUserCenterEvent;
        FunnySDK.OnCloseUserCenterEvent += OnCloseUserCenterEvent;

        FunnySDK.OnGuestDidBindEvent += OnGuestDidBindEvent;
        FunnySDK.OnSwitchAccountEvent += OnSwitchAccountEvent;
        FunnySDK.OnOpenBillboardEvent += FunnySDK_OnOpenBillboardEvent;
        FunnySDK.OnCloseBillboardEvent += FunnySDK_OnCloseBillboardEvent;
        FunnySDK.OnOpenFeedbackEvent += FunnySDK_OnOpenFeedbackEvent;
        FunnySDK.OnCloseFeedbackEvent += FunnySDK_OnCloseFeedbackEvent;
    }

    private void FunnySDK_OnCloseFeedbackEvent()
    {
        FunnyUtils.ShowToast("反馈面板被关闭了");
    }

    private void FunnySDK_OnOpenFeedbackEvent()
    {
        FunnyUtils.ShowToast("反馈面板被打开了");
    }

    private void FunnySDK_OnCloseBillboardEvent()
    {
        FunnyUtils.ShowToast("公告面板被关闭了");
    }

    private void FunnySDK_OnOpenBillboardEvent()
    {
        FunnyUtils.ShowToast("公告面板被打开了");
    }

    private void FunnySDK_OnConfirmProtocolEvent(bool isSuccess)
    {
        Debug.Log("receive value: " + isSuccess);
        if (isSuccess)
        {
            FunnyUtils.ShowToast("用户同意了协议");
            initSDKAfterProtocol();
        } else
        {
            FunnyUtils.ShowToast("同意协议对话框加载失败！");
        }
    }

    private void OnSwitchAccountEvent(AccessToken token) {
        FunnyUtils.ShowToast("切换到新账号了");
        GetProfile();
    }

    private void OnLogoutEvent() {
        FunnyUtils.ShowToast("账号登出了");
        ResetProfile();
    }

    private void OnGuestDidBindEvent(AccessToken token) {
        FunnyUtils.ShowToast("当前游客用户已绑定至新账号");
        Debug.Log($"当前游客用户已绑定");
    }

    private void OnOpenUserCenterEvent() {
        Debug.Log("用户中心被打开了");
    }

    private void OnCloseUserCenterEvent() {
        Debug.Log("用户中心被关闭了");
    }

    private void OnLoginEvent(AccessToken token) {
        FunnyUtils.ShowToast("账号已登录");
        GetProfile();

        //try
        //{
        //    var privacy = await FunnySDK.AuthPrivacyProfile();

        //}
        //catch (PrivacyProfileCancelledException)
        //{
        //    FunnyUtils.ShowToast("用户已取消授权");
        //}
        //catch (PrivacyProfileDisableException)
        //{
        //    FunnyUtils.ShowToast("授权功能未开启");
        //}
        //catch (FunnySDKException ex)
        //{
        //    FunnyUtils.ShowTipsAlert("提示", $"发生错误 - {ex.Message} : {ex.Code}");
        //}
    }

    private void Start() {
#if UNITY_IOS
        layoutGroup.padding.top = (int)(Screen.safeArea.y / 2.5);
#else
        layoutGroup.padding.top = 20;
#endif
    }

    public async void Login() {
        
        try {
            await FunnySDK.Login();
        }
        catch (LoginCancelledException) {
            FunnyUtils.ShowToast("登录被取消了");
        }
        catch (FunnySDKException error) {
            rawJsonText.text = error.Message;
        }
        
    }

    public void OpenUserCenter() {
        FunnySDK.OpenUserCenterUI();
    }

    //public void LoginWithUGUI()
    //{
    //    SoFunny.FunnySDK.UIModule.LoginUIService.OpenLoginSelectView();
    //}

    public void GetCurrentToken() {
        var accessToken = FunnySDK.GetCurrentAccessToken();
        UpdateRawSection(accessToken);
    }

    public async void AuthPrivacyProfile() {
        try {
            var profile = await FunnySDK.AuthPrivacyProfile();
            FunnyUtils.ShowToast("已获取到授权信息");
            UpdateRawSection(profile);
        }
        catch (PrivacyProfileCancelledException) {
            FunnyUtils.ShowToast("用户已取消授权");
        }
        catch (PrivacyProfileDisableException) {
            FunnyUtils.ShowToast("授权功能未开启");
        }
        catch (FunnySDKException ex) {
            FunnyUtils.ShowTipsAlert("提示", $"发生错误 - {ex.Message} : {ex.Code}");
        }
        
    }

    public async void Logout() {
        var success = await FunnySDK.Logout();
        if (success) {
            ResetProfile();
        }
    }

    public void CopyTokenValue() {
        var token = FunnySDK.GetCurrentAccessToken();
        if (token != null) {
            GUIUtility.systemCopyBuffer = token.Value;
            FunnyUtils.ShowTipsAlert("提示", "已复制到粘贴板");
        }
        else {
            FunnyUtils.ShowTipsAlert("提示", "未登录，无法复制");
        }
    }

    public async void GetProfile() {
        try {
            var profile = await FunnySDK.GetProfile();
            StartCoroutine(UpdateProfile(profile));
            UpdateRawSection(profile);
        }
        catch (NotLoggedInException) {
            FunnyUtils.ShowTipsAlert("", "未登陆账号");
        }
        catch (AccessTokenInvalidException) {
            FunnyUtils.ShowTipsAlert("","Token 已失效，请重新登陆");
        }
    }

    public void ShowProtocol()
    {
        FunnySDK.ShowProtocol();
    }
        

    public void GetIPAddress() {
        try {
            IPAddress[] iPs;
            iPs = Dns.GetHostAddresses(Dns.GetHostName());
            var local_ip_list = iPs.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                   .Select(ip => ip.ToString()).ToArray();
            rawJsonText.text = string.Join(" | ", local_ip_list);
        }
        catch (System.Exception) {
            rawJsonText.text = "获取 IP 失败";
        }
    }

    public void ShowBillboardH() {
        FunnySDK.OpenBillboardUI(BillboardStyle.horizontal);
    }

    public void ShowBillboardV() {
        FunnySDK.OpenFeedbackUI();
    }

    public async void HasBillMessage() {
        bool has = await FunnySDK.AnyBillMessage();
        FunnyUtils.ShowTipsAlert("消息", has ? "有公告内容" : "无公告内容");
    }

    public void CleanDatasView() {
        rawJsonText.text = "";
    }

    public void ShowLaunchScreen() {
        launchValue = !launchValue;
        FunnyLaunch.Show(launchValue, () =>
        {
            Debug.Log("开屏动画完成2");
        });
    }

    IEnumerator UpdateProfile(UserProfile profile) {
        if (profile.PictureUrl != null) {
            var www = UnityWebRequestTexture.GetTexture(profile.PictureUrl);
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
                    break;
                default:
                    Debug.LogError(www.error);
                    break;
            }
#else
            if (www.isDone && !www.isNetworkError) {
                var texture = DownloadHandlerTexture.GetContent(www);
                userIconImage.color = Color.white;
                userIconImage.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0, 0));
            }
            else {
                Debug.LogError(www.error);
            }
#endif
        }
        else {
            yield return null;
        }

        displayNameText.text = profile.DisplayName;
        statusMessageText.text = profile.StatusMessage;

        switch (profile.loginChannel) {
            case LoginType.SoFunny:
                displayNameText.text += " (真有趣)";
                break;
            case LoginType.Guest:
                displayNameText.text += " (游客)";
                break;
            case LoginType.Apple:
                displayNameText.text += " (Apple)";
                break;
            case LoginType.Facebook:
                displayNameText.text += " (Facebook)";
                break;
            case LoginType.Twitter:
                displayNameText.text += " (Twitter)";
                break;
            case LoginType.GooglePlay:
                displayNameText.text += " (GooglePlay)";
                break;
            case LoginType.QQ:
                displayNameText.text += " (QQ)";
                break;
            case LoginType.WeChat:
                displayNameText.text += " (WeChat)";
                break;
        }
    }

    void ResetProfile() {
        userIconImage.color = Color.gray;
        userIconImage.sprite = null;
        displayNameText.text = "Display Name";
        statusMessageText.text = "Status Message";
        rawJsonText.text = "";
    }

    void UpdateRawSection(object obj) {
        if (obj == null) {
            rawJsonText.text = "null";
            return;
        }
        
        var text = JsonUtility.ToJson(obj, true);
        if (text == null) {
            rawJsonText.text = "Invalid Object";
            return;
        }
        rawJsonText.text = text;
        var scrollContentTransform = (RectTransform)rawJsonText.gameObject.transform.parent;
        scrollContentTransform.localPosition = Vector3.zero;
    }
}
