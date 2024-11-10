using Enemy.Events;
using InputUtils;
using Matchstick.Events;
using Zenject;

namespace Tutorial.States
{
    public class MimicTutorialState : TutorialState
    {
        public override string Name => "Shoot the Mimic";
        private readonly TutorialPropController _propController;
        private PlayerInputFlags _previousInputFlags;

        public MimicTutorialState(
            TutorialController controller,
            SignalBus signalBus,
            TutorialPropController propController) : base(controller, signalBus)
        {
            _propController = propController;
        }

        public override void Enter()
        {
            base.Enter();
            _propController.ResetAll();
            _propController.SpawnFirstProp();
            SignalBus.Subscribe<MatchLitEvent>(OnMatchLit);
            SignalBus.Subscribe<MatchWentOutEvent>(OnMatchWentOut);
            SignalBus.Subscribe<EnemyGotHitEvent>(OnEnemyGotHit);

            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags =
                PlayerInputFlags.NonGameplay | PlayerInputFlags.Look | PlayerInputFlags.Move;
            Controller.InputService.EnableInputs(inputFlags);
        }

        public override void Exit()
        {
            base.Exit();
            _propController.ResetAll();
            Controller.InputService.EnableInputs(_previousInputFlags);
            SignalBus.Unsubscribe<MatchLitEvent>(OnMatchLit);
            SignalBus.Unsubscribe<EnemyGotHitEvent>(OnEnemyGotHit);
            SignalBus.Unsubscribe<MatchWentOutEvent>(OnMatchWentOut);
        }

        private void OnMatchLit()
        {
            const PlayerInputFlags inputFlags =
                PlayerInputFlags.NonGameplay | PlayerInputFlags.Look | PlayerInputFlags.Move | PlayerInputFlags.Fire;
            Controller.InputService.EnableInputs(inputFlags);
        }

        private void OnMatchWentOut()
        {
            const PlayerInputFlags inputFlags =
                PlayerInputFlags.NonGameplay | PlayerInputFlags.Look | PlayerInputFlags.Move |
                PlayerInputFlags.Matchstick;
            Controller.InputService.EnableInputs(inputFlags);
        }

        private void OnEnemyGotHit()
        {
            Progress += 1f;
        }
    }
}