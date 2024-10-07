using Enemy;
using GameLoop;
using Matchstick;
using Shooting;
using Zenject;

namespace Installers
{
    public class SceneInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ShootingService>().AsSingle();
            Container.Bind<MatchService>().AsSingle();
            Container.Bind<EnemyService>().AsSingle();
            Container.Bind<GameStateProvider>().AsSingle();
        }
    }
}