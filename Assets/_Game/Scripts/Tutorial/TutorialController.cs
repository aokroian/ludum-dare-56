using BasicStateMachine;
using GameLoop;
using InputUtils;
using Tutorial.States;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        public PlayerInputsService InputService => _inputService;
        
        [Inject] private GameStateProvider _gameStateProvider;
        private GameStateData _gameState;

        private StateMachine<TutorialState> _fsm;
        private FireTutorialState _fireState;
        private MovementTutorialState _movementState;
        private LookTutorialState _lookState;
        private MatchstickTutorialState _matchstickState;
        private NoneTutorialState _noneTutorialState;
        private TutorialState[] _statesQueue;
        private int _currentStateIndex;

        [Inject] private PlayerInputsService _inputService;

        private void Start()
        {
            _gameStateProvider.LoadGameState();
            _gameState = _gameStateProvider.GameState;
            InitStateMachine();
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
            _noneTutorialState = new NoneTutorialState(this);

            _statesQueue = new TutorialState[]
            {
                _noneTutorialState, _fireState, _movementState, _lookState, _matchstickState
            };

            _fsm.Init(_statesQueue);
            _fsm.SetState(_noneTutorialState);
            _currentStateIndex = 0;
        }

        private void UnInitStateMachine()
        {
            _fsm.CurrentState?.Exit();
            _fsm = null;
        }

        private void Update()
        {
            if (_fsm.CurrentState == _noneTutorialState)
                return;
            if (_fsm.CurrentState.IsRunning && _fsm.CurrentState.IsDone)
            {
                _currentStateIndex++;
                _fsm.SetState(_currentStateIndex < _statesQueue.Length
                    ? _statesQueue[_currentStateIndex]
                    : _noneTutorialState);
            }

            _fsm.Tick(Time.deltaTime);
        }

        public void StartTutorial()
        {
        }
    }
}