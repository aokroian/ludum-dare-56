using InputUtils;
using StarterAssets;
using UnityEngine;
using Zenject;

namespace Tutorial.States
{
    public class MovementTutorialState : TutorialState
    {
        public override string Hint => Strings.MoveControlsTutorialHint;

        private PlayerInputFlags _previousInputFlags;
        private Transform _playerT;
        private Vector3 _prevPosition;
        private const float ProgressSpeed = .8f;

        public MovementTutorialState(TutorialController controller, SignalBus signalBus) : base(controller, signalBus)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _playerT = Object.FindFirstObjectByType<FirstPersonController>().transform;
            _prevPosition = _playerT.position;
            _previousInputFlags = Controller.InputService.InputFlags;
            const PlayerInputFlags inputFlags =
                PlayerInputFlags.NonGameplay | PlayerInputFlags.Move | PlayerInputFlags.Look;
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

            if (Vector3.Distance(_prevPosition, _playerT.position) > .001f)
                Progress += ProgressSpeed * deltaTime;

            _prevPosition = _playerT.position;
        }
    }
}