using System;
using InputUtils;
using Shooting.Events;
using UnityEngine;
using Zenject;

namespace Tutorial.States
{
    public class FireTutorialState : TutorialState
    {
        private PlayerInputFlags _previousInputFlags;
        public override string Name => "Fire";

        public FireTutorialState(TutorialController controller, SignalBus signalBus) : base(controller, signalBus)
        {
        }

        public override void Enter()
        {
            base.Enter();
            SignalBus.Subscribe<ShootingEvent>(OnFire);
            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Fire;
            Controller.InputService.EnableInputs(inputFlags);
        }

        public override void Exit()
        {
            base.Exit();
            SignalBus.Unsubscribe<ShootingEvent>(OnFire);
            Controller.InputService.EnableInputs(_previousInputFlags);
        }

        private void OnFire()
        {
            Progress += .5f;
        }
    }
}