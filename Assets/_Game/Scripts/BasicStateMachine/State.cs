namespace BasicStateMachine
{
    public abstract class State
    {
        public bool IsRunning { get; private set; }
        
        public virtual bool IsDone { get; protected set; }

        public virtual void Enter()
        {
            if (IsRunning)
                Exit();
            IsRunning = true;
            IsDone = false;
        }

        public virtual void Exit()
        {
            IsRunning = false;
        }

        public virtual void Tick(float deltaTime)
        {
        }
    }
}