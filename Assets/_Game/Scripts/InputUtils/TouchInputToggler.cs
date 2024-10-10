using System;
using StarterAssets;
using UnityEngine;

namespace InputUtils
{
    public class TouchInputToggler : MonoBehaviour
    {
        public UICanvasControllerInput touchInputUI;

        private void Start()
        {
#if UNITY_ANDROID || UNITY_IOS
            touchInputUI.enabled = true;
#else
            touchInputUI.enabled = false;;
#endif
        }
    }
}