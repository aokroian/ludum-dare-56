using Matchstick;
using Shooting;
using Sound;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(menuName = "Custom/GameConfigInstaller")]
    public class ConfigInstaller : ScriptableObjectInstaller
    {
        public ShootingService.Config shootingService;
        public MatchService.Config matchService;
        public SoundsConfig soundsConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(shootingService).IfNotBound();
            Container.BindInstance(matchService).IfNotBound();
            Container.BindInstance(soundsConfig).IfNotBound();
        }
    }
}