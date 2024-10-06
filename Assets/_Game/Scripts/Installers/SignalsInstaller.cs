using Enemy.Events;
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

            Container.DeclareSignal<MatchLitEvent>().OptionalSubscriber();
            Container.DeclareSignal<MatchWentOutEvent>().OptionalSubscriber();

            Container.DeclareSignal<EnemyRepositionEvent>().OptionalSubscriber();

            Container.DeclareSignal<ShootingEvent>().OptionalSubscriber();
            Container.DeclareSignal<ShootingNoAmmoEvent>().OptionalSubscriber();

            // player signals
            Container.DeclareSignal<PlayerStepEvent>().OptionalSubscriber();
        }
    }
}