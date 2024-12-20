using InputUtils;
using Matchstick.Events;
using Zenject;

namespace Tutorial.States
{
    public class MatchstickTutorialState : TutorialState
    {
        protected override string Hint => Strings.MatchControlsTutorialHint;

        private PlayerInputFlags _previousInputFlags;

        public MatchstickTutorialState(TutorialController controller, SignalBus signalBus) : base(controller, signalBus)
        {
        }

        public override void Enter()
        {
            base.Enter();
            SignalBus.Subscribe<MatchLitEvent>(OnMatchLit);
            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Look |
                                                PlayerInputFlags.Move | PlayerInputFlags.Matchstick;
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