namespace Tutorial.States
{
    public class MovementTutorialState : TutorialState
    {
        public MovementTutorialState(TutorialController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // disable all inputs except movement
        }

        public override void Exit()
        {
            base.Exit();
            // enable all inputs
        }


        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
        }
    }
}