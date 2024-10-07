using Matchstick;
using Matchstick.Events;
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
        }
    }
}