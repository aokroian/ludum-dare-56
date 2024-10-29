using InputUtils;
using StarterAssets;
using UnityEngine;
using Zenject;

namespace Tutorial.States
{
    public class LookTutorialState : TutorialState
    {
        private PlayerInputFlags _previousInputFlags;
        private Transform _cameraT;
        private Quaternion _prevRotation;
        private const float ProgressSpeed = .5f;

        public LookTutorialState(TutorialController controller, SignalBus signalBus) : base(controller, signalBus)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _cameraT = Camera.main.transform;
            _prevRotation = _cameraT.rotation;
            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags = PlayerInputFlags.NonGameplay | PlayerInputFlags.Look;
            Controller.InputService.EnableInputs(inputFlags);
        }

        public override void Exit()
        {
            base.Exit();
            Controller.InputService.EnableInputs(_previousInputFlags);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            
            if (Quaternion.Angle(_prevRotation, _cameraT.rotation) > .1f)
                Progress += ProgressSpeed * deltaTime;
            
            _prevRotation = _cameraT.rotation;
        }
    }
}