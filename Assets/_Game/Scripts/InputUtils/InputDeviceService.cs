using R3;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InputUtils
{
    public class InputDeviceService
    {
        public readonly ReactiveProperty<InputDevice> CurrentDevice = new();

        public InputDeviceService()
        {
            InputSystem.onAnyButtonPress.Call(control => CurrentDevice.Value = control.device);
        }
    }
}