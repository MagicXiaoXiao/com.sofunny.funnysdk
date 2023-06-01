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

        private void Awake()
        {
            seconds = timerSeconds;
            smsTextValue = smsText.text;

            timer = Timer.Register(1f, OnTimerUpdate, isLooped: true, useRealTime: true);
            timer.Pause();
        }

        private void OnTimerUpdate()
        {
            seconds--;
            if (seconds <= 0)
            {
                timer.Pause();
                seconds = timerSeconds;
                smsButton.interactable = true;
                smsText.text = smsTextValue;
            }
            else
            {
                smsText.text = $"{seconds}s";
            }
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


