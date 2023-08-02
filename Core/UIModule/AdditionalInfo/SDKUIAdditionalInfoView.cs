using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    internal class SDKUIAdditionalInfoView : MonoBehaviour
    {
        public Toggle maleToggle;
        public Toggle femaleToggle;
        public Button dateButton;
        public Text dateText;
        public Button commitButton;
        public Button nextTimeButton;

        internal IAdditionalInfoDelegate InfoDelegate;

        void Awake()
        {
            dateButton.onClick.AddListener(OnShowDateSelectAction);
            commitButton.onClick.AddListener(OnCommitAction);
            nextTimeButton.onClick.AddListener(OnNextTimeAction);
        }

        void OnDestroy()
        {
            InfoDelegate = null;

            dateButton.onClick.RemoveAllListeners();
            commitButton.onClick.RemoveAllListeners();
            nextTimeButton.onClick.RemoveAllListeners();
        }

        internal void SetGender(string genderValue)
        {
            if (string.IsNullOrEmpty(genderValue)) { return; }

            switch (genderValue)
            {
                case "MALE":
                    maleToggle.isOn = true;
                    break;
                case "FEMALE":
                    femaleToggle.isOn = true;
                    break;
                default: return;
            }
        }

        internal void SetDateValue(string date)
        {
            if (string.IsNullOrEmpty(date)) { return; }

            dateText.text = date;
        }

        private bool VerifyContent()
        {
            if (maleToggle.isOn == false && femaleToggle.isOn == false)
            {
                Toast.ShowFail(Locale.LoadText("page.userInfo.tips.sex"));
                return false;
            }

            string value = dateText.text.Trim();

            if (string.IsNullOrEmpty(value))
            {
                Toast.ShowFail(Locale.LoadText("page.userInfo.tips.birthday"));
                return false;
            }

            return true;
        }

        private void OnShowDateSelectAction()
        {
            string value = dateText.text.Trim();
            InfoDelegate?.OnShowDateView(value);
        }

        private void OnCommitAction()
        {
            if (!VerifyContent()) { return; }

            string sexFlag = maleToggle.isOn ? "MALE" : "FEMALE";
            string dateValue = dateText.text.Trim();

            InfoDelegate?.OnCommit(sexFlag, dateValue);
        }

        private void OnNextTimeAction()
        {
            InfoDelegate?.OnNextTime();
        }
    }
}


