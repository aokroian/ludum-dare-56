using R3;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace InputUtils
{
    public class InputDeviceService
    {
        public readonly ReactiveProperty<InputDevice> CurrentDevice = new();

        public InputDeviceService()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
            InputSystem.onEvent += OnInputEvent;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (!Application.isPlaying)
                return;
            if (change is InputDeviceChange.Added or InputDeviceChange.Reconnected)
            {
                CurrentDevice.Value = device;
            }
        }

        private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (!Application.isPlaying)
                return;
            if (device is Mouse or Keyboard or Gamepad or Touchscreen)
            {
                CurrentDevice.Value = device;
            }
        }
    }
}