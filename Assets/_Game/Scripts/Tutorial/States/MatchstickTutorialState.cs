using InputUtils;
using Matchstick.Events;
using Zenject;

namespace Tutorial.States
{
    public class MatchstickTutorialState : TutorialState
    {
        public override string Name => "Light Matchstick";
        
        private PlayerInputFlags _previousInputFlags;

        public MatchstickTutorialState(TutorialController controller, SignalBus signalBus) : base(controller, signalBus)
        {
        }

        public override void Enter()
        {
            base.Enter();
            SignalBus.Subscribe<MatchLitEvent>(OnMatchLit);
            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Matchstick;
            Controller.InputService.EnableInputs(inputFlags);
        }

        public override void Exit()
        {
            base.Exit();
            SignalBus.Unsubscribe<MatchLitEvent>(OnMatchLit);
            Controller.InputService.EnableInputs(_previousInputFlags);
        }

        private void OnMatchLit()
        {
            Progress += 1f;
        }
    }
}