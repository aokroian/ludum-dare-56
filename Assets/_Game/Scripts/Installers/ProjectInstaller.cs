﻿using GameLoop;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalsInstaller.Install(Container);
            Container.Bind<GameStateProvider>().AsSingle();
        }
    }
}