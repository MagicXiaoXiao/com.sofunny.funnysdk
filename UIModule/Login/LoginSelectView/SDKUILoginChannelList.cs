using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoFunny.FunnySDK.Internal;

namespace SoFunny.FunnySDK.UIModule
{
    public class SDKUILoginChannelList : MonoBehaviour
    {
#pragma warning disable 0649

        [SerializeField] private Button sofunnyLoginButton;
        [SerializeField] private Button guestLoginButton;

        [SerializeField] private GameObject otherLoginContainer;
        [SerializeField] private Button facebookButton;
        [SerializeField] private Button twitterButton;
        [SerializeField] private Button googleButton;
        [SerializeField] private Button appleButton;
        [SerializeField] private Button wechatButton;
        [SerializeField] private Button qqButton;
        [SerializeField] private Button taptapButton;

        [SerializeField] private GameObject protocolContainer;
        [SerializeField] private Toggle protocolToggle;
        [SerializeField] private Button agreementButton;
        [SerializeField] private Button privacyButton;

#pragma warning restore 0649

        private void Awake()
        {

            if (ConfigService.Config.IsMainland)
            {
                protocolContainer.SetActive(true);
                guestLoginButton.gameObject.SetActive(false);
                facebookButton.gameObject.SetActive(false);
                twitterButton.gameObject.SetActive(false);
                googleButton.gameObject.SetActive(false);
                appleButton.gameObject.SetActive(false);
                wechatButton.gameObject.SetActive(true);
                qqButton.gameObject.SetActive(true);
                taptapButton.gameObject.SetActive(true);
            }
            else
            {
                protocolContainer.SetActive(false);
                guestLoginButton.gameObject.SetActive(true);
                facebookButton.gameObject.SetActive(true);
                twitterButton.gameObject.SetActive(true);
                googleButton.gameObject.SetActive(true);
                appleButton.gameObject.SetActive(true);
                wechatButton.gameObject.SetActive(false);
                qqButton.gameObject.SetActive(false);
                taptapButton.gameObject.SetActive(false);
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

    }
}


