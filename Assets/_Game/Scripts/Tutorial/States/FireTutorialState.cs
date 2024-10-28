using InputUtils;

namespace Tutorial.States
{
    public class FireTutorialState : TutorialState
    {
        private PlayerInputFlags _previousInputFlags;

        public FireTutorialState(TutorialController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Fire;
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
            if (Controller.InputService.CurrentState.fire)
                Progress = 1;
        }
    }
}