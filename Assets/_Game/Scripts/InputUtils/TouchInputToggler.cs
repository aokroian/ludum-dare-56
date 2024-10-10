using UnityEngine;

namespace InputUtils
{
    public class TouchInputToggler : MonoBehaviour
    {
        public UICanvasControllerInput touchInputUI;

        private void Start()
        {
#if UNITY_ANDROID || UNITY_IOS
            touchInputUI.gameObject.SetActive(true);
#else
            touchInputUI.gameObject.SetActive(false);
#endif
        }
    }
}