using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerInputsService: MonoBehaviour
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
            Debug.Log("Fire input: " + value.isPressed);
            if (_isInputEnabled)
            {
                _innerState.fire = value.isPressed;
            }
        }
        
        public void OnMatchstick(InputValue value)
        {
            Debug.Log("Matchstick input: " + value.isPressed);
            if (_isInputEnabled)
            {
                _innerState.matchstick = value.isPressed;
            }
        }

        public void OnMove(InputValue value)
        {
            if (_isInputEnabled)
            {
                _innerState.move = value.Get<Vector2>();
            }
        }

        public void OnLook(InputValue value)
        {
            if (_isInputEnabled)
            {
                _innerState.look = value.Get<Vector2>();
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