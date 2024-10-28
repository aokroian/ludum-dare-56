using BasicStateMachine;

namespace Tutorial.States
{
    public abstract class TutorialState : State
    {
        protected float Progress;
        protected TutorialController Controller;
        public sealed override bool IsDone => Progress >= 1;

        public TutorialState(TutorialController controller)
        {
            Controller = controller;
        }

        public override void Enter()
        {
            base.Enter();
            Progress = 0;
        }
    }
}