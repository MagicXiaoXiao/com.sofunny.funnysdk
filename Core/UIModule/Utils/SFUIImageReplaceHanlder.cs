using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoFunny.FunnySDK.UIModule
{
    [RequireComponent(typeof(Image))]
    internal class SFUIImageReplaceHanlder : MonoBehaviour
    {
        private Image _image;
        private Sprite _mySprite;

        public string ImgName;

        void Awake()
        {
            _image = GetComponent<Image>();
            _mySprite = Resources.Load<Sprite>($"FunnyCustomRes/{ImgName}");

            if (_mySprite is null) return;

            _image.sprite = _mySprite;

        }

        private void OnDestroy()
        {
            if (_mySprite is null) return;

            Resources.UnloadAsset(_mySprite);
        }

    }
}


