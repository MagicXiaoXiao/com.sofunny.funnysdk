using System;
using UnityEngine;

namespace SoFunny.FunnySDKPreview
{

    [Serializable]
    internal class WrapperArray<T>
    {
#pragma warning disable 0649
        [SerializeField]
        internal T[] array;
#pragma warning restore 0649
    }
}

