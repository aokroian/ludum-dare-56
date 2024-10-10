using UnityEngine;

namespace InputUtils
{
    public class FramerateManager : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}