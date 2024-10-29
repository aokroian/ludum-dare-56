using System;
using BasicStateMachine;
using InputUtils;
using R3;
using Tutorial.States;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        public bool IsDone { get; private set; }
        public TutorialState CurrentState => _fsm.CurrentState;
        public PlayerInputsService InputService => _inputService;

        [SerializeField] private TutorialUI keyboardMouseUI;
        [SerializeField] private TutorialUI gamepadUI;
        [SerializeField] private TutorialUI touchUI;

        private StateMachine<TutorialState> _fsm;
        private FireTutorialState _fireState;
        private MovementTutorialState _movementState;
        private LookTutorialState _lookState;
        private MatchstickTutorialState _matchstickState;
        private TutorialState[] _statesQueue;
        private int _currentStateIndex;
        private TutorialUI _currentUI;
        private IDisposable _deviceSubscription;
        private bool _isInit;

        [Inject] private PlayerInputsService _inputService;
        [Inject] private InputDeviceService _inputDeviceService;
        [Inject] private SignalBus _signalBus;

        public void Init()
        {
            if (_isInit) UnInitStateMachine();
            InitStateMachine();
            IsDone = false;
            _currentStateIndex = 0;
            _fsm.SetState(_statesQueue[_currentStateIndex]);
            _deviceSubscription = _inputDeviceService.CurrentDevice.Subscribe(OnInputDeviceChanged);
            OnInputDeviceChanged(_inputDeviceService.CurrentDevice.Value);
            _isInit = true;
        }

        private void OnDestroy()
        {
            _deviceSubscription?.Dispose();
            UnInitStateMachine();
        }

        private void InitStateMachine()
        {
            _fsm = new StateMachine<TutorialState>();
            _fireState = new FireTutorialState(this, _signalBus);
            _movementState = new MovementTutorialState(this, _signalBus);
            _lookState = new LookTutorialState(this, _signalBus);
            _matchstickState = new MatchstickTutorialState(this, _signalBus);

            _statesQueue = new TutorialState[]
            {
                _movementState, _lookState, _fireState, _matchstickState
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
            if (!_isInit || IsDone)
                return;
            if (_fsm.CurrentState.IsRunning && _fsm.CurrentState.IsDone)
            {
                _currentStateIndex++;
                if (_currentStateIndex < _statesQueue.Length)
                    _fsm.SetState(_statesQueue[_currentStateIndex]);
                else
                    IsDone = true;
            }

            _fsm.Tick(Time.deltaTime);
        }

        private void OnInputDeviceChanged(InputDevice device)
        {
            var ui = device switch
            {
                Keyboard => keyboardMouseUI,
                Gamepad => gamepadUI,
                Touchscreen => touchUI,
                _ => null
            };

            if (_currentUI && ui != _currentUI)
            {
                _currentUI.UnInit();
            }

            if (ui) ui.Init(this);

            _currentUI = ui;
        }
    }
}