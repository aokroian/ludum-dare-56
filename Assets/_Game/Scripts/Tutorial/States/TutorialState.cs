using BasicStateMachine;
using Zenject;

namespace Tutorial.States
{
    public abstract class TutorialState : State
    {
        public float Progress { get; protected set; }
        protected SignalBus SignalBus { get; private set; }
        protected TutorialController Controller { get; private set; }
        public sealed override bool IsDone => Progress >= 1;

        public TutorialState(TutorialController controller, SignalBus signalBus)
        {
            Controller = controller;
            SignalBus = signalBus;
        }

        public virtual string Name => GetType().Name;

        public override void Enter()
        {
            base.Enter();
            Progress = 0;
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            if (Progress >= 1f)
            {
                IsDone = true;
            }
        }
    }
}