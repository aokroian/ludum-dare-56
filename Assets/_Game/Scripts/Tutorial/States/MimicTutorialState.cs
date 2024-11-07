using Zenject;

namespace Tutorial.States
{
    public class MimicTutorialState : TutorialState
    {
        public override string Name => "Shoot the Mimic";

        public MimicTutorialState(TutorialController controller, SignalBus signalBus) : base(controller, signalBus)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
    }
}