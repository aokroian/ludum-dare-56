using InputUtils;
using UnityEngine;

namespace Tutorial.States
{
    public class MovementTutorialState : TutorialState
    {
        private PlayerInputFlags _previousInputFlags;
        private const float ProgressSpeed = .5f;

        public MovementTutorialState(TutorialController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Move;
            Controller.InputService.EnableInputs(inputFlags);

            Debug.Log($"Let's train movement!");
        }

        public override void Exit()
        {
            base.Exit();
            Controller.InputService.EnableInputs(_previousInputFlags);
            Debug.Log($"You've learned how to move!");
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            if (Controller.InputService.CurrentState.move.magnitude > 0)
            {
                Progress += ProgressSpeed * deltaTime;
                Debug.Log($"Movement progress: {Progress}");
            }
        }
    }
}