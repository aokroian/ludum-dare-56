using InputUtils;

namespace Tutorial.States
{
    public class MovementTutorialState : TutorialState
    {
        private PlayerInputFlags _previousInputFlags;
        private const float ProgressSpeed = 1f;

        public MovementTutorialState(TutorialController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Move;
            Controller.InputService.EnableInputs(inputFlags);
        }

        public override void Exit()
        {
            base.Exit();
            Controller.InputService.EnableInputs(_previousInputFlags);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            if (Controller.InputService.CurrentState.move.magnitude > 0)
                Progress += ProgressSpeed * deltaTime;
        }
    }
}