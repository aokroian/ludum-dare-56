using UnityEngine;

namespace InputUtils
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [Header("Output")]
        public PlayerInputsService playerInput;

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