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

        private readonly InputState _innerState = new();

        private PlayerInputFlags _inputFlags = PlayerInputFlags.All;

        public void EnableInputs(PlayerInputFlags flags)
        {
            _inputFlags = flags;
            if (_inputFlags == PlayerInputFlags.None)
            {
                _innerState.Reset();
            }
        }

        public void OnEscape(InputValue value)
        {
            EscapeInput(value.isPressed);
        }

        public void OnRestart(InputValue value)
        {
            RestartInput(value.isPressed);
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

        public void EscapeInput(bool value)
        {
            if (_inputFlags.HasFlag(PlayerInputFlags.Escape))
            {
                _innerState.escape = value;
            }
        }

        public void RestartInput(bool value)
        {
            if (_inputFlags.HasFlag(PlayerInputFlags.Restart))
            {
                _innerState.restart = value;
            }
        }

        public void FireInput(bool value)
        {
            if (_inputFlags.HasFlag(PlayerInputFlags.Fire))
            {
                _innerState.fire = value;
            }
        }

        public void MatchstickInput(bool value)
        {
            if (_inputFlags.HasFlag(PlayerInputFlags.Matchstick))
            {
                _innerState.matchstick = value;
            }
        }

        public void MoveInput(Vector2 value)
        {
            if (_inputFlags.HasFlag(PlayerInputFlags.Move))
            {
                _innerState.move = value;
            }
        }

        public void LookInput(Vector2 value)
        {
            if (_inputFlags.HasFlag(PlayerInputFlags.Look))
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