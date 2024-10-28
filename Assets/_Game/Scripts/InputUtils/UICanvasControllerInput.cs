using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InputUtils
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [Header("Output")]
        public PlayerInputsService playerInput;

        [Inject] private InputDeviceService _inputDeviceService;

        private void Start()
        {
            _inputDeviceService.CurrentDevice
                .Subscribe(device => { gameObject.SetActive(device is Touchscreen); }).AddTo(this);
        }

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            playerInput.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            playerInput.LookInput(virtualLookDirection);
        }

        public void VirtualFireInput(bool virtualJumpState)
        {
            playerInput.FireInput(virtualJumpState);
        }

        public void VirtualMatchstickInput(bool virtualSprintState)
        {
            playerInput.MatchstickInput(virtualSprintState);
        }
    }
}