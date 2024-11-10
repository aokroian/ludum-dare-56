using R3;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

namespace InputUtils
{
    public class InputDeviceService
    {
        public readonly ReactiveProperty<InputDevice> CurrentDevice = new();

        public InputDeviceService()
        {
            if (Application.isPlaying)
                InputSystem.onAnyButtonPress.Call(control => CurrentDevice.Value = control.device);
        }
    }
}