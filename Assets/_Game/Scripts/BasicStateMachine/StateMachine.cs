using System;
using System.Linq;
using UnityEngine;

namespace BasicStateMachine
{
    public class StateMachine<T> where T : State
    {
        private T[] _states;

        public T CurrentState { get; private set; }
        public string OwnerName { get; private set; }
        public event Action<T> OnStateEnter;
        public event Action<T> OnStateExit;

        public void Init(T[] states, string ownerName = null)
        {
            _states = states.ToArray();
            OwnerName = ownerName;
        }

        public void SetState(T newState)
        {
            if (!_states.Contains(newState))
            {
                Debug.LogWarning("State is not in the list of available states. Cannot set state.");
                return;
            }

            CurrentState?.Exit();

            if (CurrentState != null)
                OnStateExit?.Invoke(CurrentState);

            CurrentState = newState;
            CurrentState.Enter();

            if (CurrentState != null)
                OnStateEnter?.Invoke(CurrentState);
        }

        public void Tick(float deltaTime) => CurrentState?.Tick(deltaTime);
    }
}