using UnityEngine;
using UnityEngine.InputSystem;

namespace InputUtils
{
    public class PlayerInputsService : MonoBehaviour
    {
        public InputState CurrentState => _innerState;

        public bool cursorLocked = true;

        public PlayerInput playerInput;

        public bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        private InputState _innerState = new();
        private bool _isInputEnabled = true;

        public void EnableInput()
        {
            _isInputEnabled = true;
        }

        public void DisableInput()
        {
            _isInputEnabled = false;
            _innerState.Reset();
        }

        public void OnFire(InputValue value)
        {
            FireInput(value.isPressed);
        }

        public void OnMatchstick(InputValue value)
        {
            MatchstickInput(value.isPressed);
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            LookInput(value.Get<Vector2>());
        }

        public void FireInput(bool value)
        {
            if (_isInputEnabled)
            {
                _innerState.fire = value;
            }
        }

        public void MatchstickInput(bool value)
        {
            if (_isInputEnabled)
            {
                _innerState.matchstick = value;
            }
        }

        public void MoveInput(Vector2 value)
        {
            if (_isInputEnabled)
            {
                _innerState.move = value;
            }
        }

        public void LookInput(Vector2 value)
        {
            if (_isInputEnabled)
            {
                _innerState.look = value;
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}