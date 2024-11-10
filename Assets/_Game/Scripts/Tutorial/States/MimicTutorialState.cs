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

            _message = "Now let's learn about mimics. They can disguise themselves as props";
            await Observable.Timer(System.TimeSpan.FromSeconds(2f)).ObserveOnMainThread().WaitAsync();

            Controller.TweenPlayerPosToShootMimicPos(2f, () =>
            {
                _message =
                    "Take a look at this prop. Try to remember where it is. The matchstick will go out soon.";
                Controller.SetLookAtActiveProp(true);
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
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Matchstick;
            Controller.InputService.EnableInputs(inputFlags);
            _message = "The matchstick went out. Let's light it up again.";
        }

        private void OnMatchLit()
        {
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Fire;
            Controller.InputService.EnableInputs(inputFlags);
            _message = "Now look at the prop. It was moved. The mimic is trying to trick you. Shoot the mimic.";
        }

        private void OnEnemyGotHit()
        {
            Progress += 1f;
        }
    }
}