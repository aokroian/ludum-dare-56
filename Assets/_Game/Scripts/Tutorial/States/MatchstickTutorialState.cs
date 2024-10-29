using InputUtils;
using Zenject;

namespace Tutorial.States
{
    public class MatchstickTutorialState : TutorialState
    {
        private PlayerInputFlags _previousInputFlags;

        public MatchstickTutorialState(TutorialController controller, SignalBus signalBus) : base(controller, signalBus)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Matchstick;
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
            if (Controller.InputService.PreviousState.matchstick)
                Progress += .5f;
        }
    }
}