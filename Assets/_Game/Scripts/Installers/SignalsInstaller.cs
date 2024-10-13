using Enemy.Events;
using GameLoop.Events;
using Matchstick.Events;
using Player.Events;
using Shooting.Events;
using Zenject;

namespace Installers
{
    public class SignalsInstaller : Installer<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            // game loop events
            Container.DeclareSignal<GameStartPressedEvent>().OptionalSubscriber();
            Container.DeclareSignal<GameSceneLoadedEvent>().OptionalSubscriber();
            Container.DeclareSignal<MenuSceneLoadedEvent>().OptionalSubscriber();
            Container.DeclareSignal<NightStartedEvent>().OptionalSubscriber();
            Container.DeclareSignal<NightFinishedEvent>().OptionalSubscriber();
            Container.DeclareSignal<GameFinishedEvent>().OptionalSubscriber();
            Container.DeclareSignal<GameOverEvent>().OptionalSubscriber();

            Container.DeclareSignal<MatchLitEvent>().OptionalSubscriber();
            Container.DeclareSignal<MatchWentOutEvent>().OptionalSubscriber();

            // enemies events
            Container.DeclareSignal<EnemyRepositionEvent>().OptionalSubscriber();
            Container.DeclareSignal<EnemyGotHitEvent>().OptionalSubscriber();
            Container.DeclareSignal<MissedEnemyEvent>().OptionalSubscriber();
            Container.DeclareSignal<AllEnemiesDeadEvent>().OptionalSubscriber();
            Container.DeclareSignal<AttackPlayerEvent>().OptionalSubscriber();

            Container.DeclareSignal<ShootingEvent>().OptionalSubscriber();
            Container.DeclareSignal<ShootingNoAmmoEvent>().OptionalSubscriber();

            // player signals
            Container.DeclareSignal<PlayerStepEvent>().OptionalSubscriber();
        }
    }
}