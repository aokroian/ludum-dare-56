using BasicStateMachine;
using GameLoop;
using InputUtils;
using R3;
using Tutorial.States;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        public readonly ReactiveProperty<bool> IsDone = new();
        public PlayerInputsService InputService => _inputService;

        [Inject] private GameStateProvider _gameStateProvider;
        private GameStateData _gameState;

        private StateMachine<TutorialState> _fsm;
        private FireTutorialState _fireState;
        private MovementTutorialState _movementState;
        private LookTutorialState _lookState;
        private MatchstickTutorialState _matchstickState;
        private TutorialState[] _statesQueue;
        private int _currentStateIndex;
        private bool _isInit;

        [Inject] private PlayerInputsService _inputService;

        public void Init()
        {
            _gameStateProvider.LoadGameState();
            _gameState = _gameStateProvider.GameState;
            if (_isInit) UnInitStateMachine();
            InitStateMachine();
            IsDone.Value = false;
            _currentStateIndex = 0;
            _fsm.SetState(_statesQueue[_currentStateIndex]);
            _isInit = true;
        }

        private void OnDestroy()
        {
            _gameStateProvider.SaveGameState();
            UnInitStateMachine();
        }

        private void InitStateMachine()
        {
            _fsm = new StateMachine<TutorialState>();
            _fireState = new FireTutorialState(this);
            _movementState = new MovementTutorialState(this);
            _lookState = new LookTutorialState(this);
            _matchstickState = new MatchstickTutorialState(this);

            _statesQueue = new TutorialState[]
            {
                _movementState, _fireState, _lookState, _matchstickState
            };

            _fsm.Init(_statesQueue);
        }

        private void UnInitStateMachine()
        {
            _fsm.CurrentState?.Exit();
            _fsm = null;
        }

        private void Update()
        {
            if (!_isInit || IsDone.Value)
                return;
            if (_fsm.CurrentState.IsRunning && _fsm.CurrentState.IsDone)
            {
                _currentStateIndex++;
                if (_currentStateIndex < _statesQueue.Length)
                    _fsm.SetState(_statesQueue[_currentStateIndex]);
                else
                    IsDone.Value = true;
            }

            _fsm.Tick(Time.deltaTime);
        }
    }
}