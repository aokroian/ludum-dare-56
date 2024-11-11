using System;
using Enemy;
using Enemy.Events;
using InputUtils;
using Level;
using Matchstick.Events;
using R3;
using Zenject;

namespace Tutorial.States
{
    public class MimicTutorialState : TutorialState
    {
        public override string Hint => _message;
        private readonly LevelController _levelController;
        private readonly EnemyService _enemyService;
        private PlayerInputFlags _previousInputFlags;

        private string _message;

        public MimicTutorialState(
            TutorialController controller,
            SignalBus signalBus,
            LevelController levelController,
            EnemyService enemyService) : base(controller, signalBus)
        {
            _levelController = levelController;
            _enemyService = enemyService;
        }

        public override async void Enter()
        {
            base.Enter();

            SignalBus.Subscribe<MatchLitEvent>(OnMatchLit);
            SignalBus.Subscribe<MatchWentOutEvent>(OnMatchWentOut);
            SignalBus.Subscribe<EnemyGotHitEvent>(OnEnemyGotHit);

            _levelController.ResetProps(0);
            _levelController.ResetProps(1);
            _enemyService.ResetEnemies();

            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay;
            Controller.InputService.EnableInputs(inputFlags);

            _message = Strings.MimicTutorialIntroMessage1;
            await Observable.Timer(TimeSpan.FromSeconds(4f)).ObserveOnMainThread().WaitAsync();
            Progress = 0.2f;
            _message = Strings.MimicTutorialIntroMessage2;
            await Observable.Timer(TimeSpan.FromSeconds(4f)).ObserveOnMainThread().WaitAsync();
            Progress = 0.4f;

            Controller.TweenPlayerPosToShootMimicPos(1f, () =>
            {
                _message = Strings.MimicTutorialIntroMessage3;
                Controller.SetLookAtActiveProp(true);
                Progress = 0.6f;
            });
        }

        public override void Exit()
        {
            base.Exit();
            SignalBus.Unsubscribe<MatchWentOutEvent>(OnMatchWentOut);
            SignalBus.Unsubscribe<MatchLitEvent>(OnMatchLit);
            SignalBus.Unsubscribe<EnemyGotHitEvent>(OnEnemyGotHit);
            _levelController.ResetProps(0);
            Controller.InputService.EnableInputs(_previousInputFlags);
            Controller.SetLookAtActiveProp(false);
        }

        private void OnMatchWentOut()
        {
            Controller.SetLookAtActiveProp(false);
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Matchstick;
            Controller.InputService.EnableInputs(inputFlags);
            Progress = 0.75f;
            _message = Strings.MimicTutorialMatchWentOutMessage;
        }

        private void OnMatchLit()
        {
            Controller.SetLookAtActiveProp(true);
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Fire;
            Controller.InputService.EnableInputs(inputFlags);
            Progress = 0.9f;
            _message = Strings.MimicTutorialShootMimicMessage;
        }

        private void OnEnemyGotHit()
        {
            Progress = 1f;
        }
    }
}