using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    public class SFSmsCodeButtonTimerHandler : MonoBehaviour
    {
        public Button smsButton;
        public Text smsText;
        public int timerSeconds = 60;

        private Timer timer;
        private int seconds;
        private string smsTextValue;
        private float inBackgroundSeconds = 0f;

        private void Awake()
        {
            seconds = timerSeconds;
            smsTextValue = smsText.text;

            timer = Timer.Register(1f, OnTimerUpdate, isLooped: true, useRealTime: true);
            timer.Pause();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (timer.isPaused || timer.isCancelled) { return; }

            if (focus)
            {
                // 应用回到前台
                inBackgroundSeconds = Time.realtimeSinceStartup - inBackgroundSeconds;

                if (inBackgroundSeconds >= seconds)
                {
                    ResetTimer();
                }
                else
                {
                    seconds = (int)(seconds - inBackgroundSeconds);
                }

                inBackgroundSeconds = 0;
            }
            else
            {
                // 应用切换至后台
                // 记录时间
                inBackgroundSeconds = Time.realtimeSinceStartup;
            }
        }

        private void OnTimerUpdate()
        {
            seconds--;
            if (seconds <= 0)
            {
                ResetTimer();
            }
            else
            {
                smsText.text = $"{seconds}s";
            }
        }

        public void SendingStatus()
        {
            smsButton.interactable = false;
            smsTextValue = smsText.text;
            smsText.text = Locale.LoadText("form.button.sendingCode");//"发送中";
        }

        public void ResetTimer()
        {
            timer.Pause();
            seconds = timerSeconds;
            smsButton.interactable = true;
            smsText.text = smsTextValue;
        }

        public void StartTimer()
        {
            smsButton.interactable = false;
            smsText.text = $"{seconds}s";
            timer.Resume();
        }

        private void OnDestroy()
        {
            Timer.Cancel(timer);
        }
    }
}


