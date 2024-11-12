using System;
using BasicStateMachine;
using DG.Tweening;
using Enemy;
using InputUtils;
using Level;
using Matchstick;
using R3;
using R3.Triggers;
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

        public float ControlsProgress =>
            (_lookState.Progress + _movementState.Progress + _fireState.Progress + _matchstickState.Progress) / 4f;
        public float GameplayProgress => _mimicState.Progress;

        [SerializeField] private float matchDuration = 12f;
        [SerializeField] private Transform playerShootMimicPos;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private CharacterController player;
        [SerializeField] private TutorialUI keyboardMouseUI;
        [SerializeField] private TutorialUI gamepadUI;
        [SerializeField] private TutorialUI touchUI;

        private StateMachine<TutorialState> _fsm;
        private FireTutorialState _fireState;
        private MovementTutorialState _movementState;
        private LookTutorialState _lookState;
        private MatchstickTutorialState _matchstickState;
        private MimicTutorialState _mimicState;
        private TutorialState[] _statesQueue;
        private int _currentStateIndex;
        private TutorialUI _currentUI;
        private IDisposable _deviceSubscription;
        private bool _isLookAtActiveProp;
        private bool _isInit;

        [Inject] private PlayerInputsService _inputService;
        [Inject] private InputDeviceService _inputDeviceService;
        [Inject] private LevelController _levelController;
        [Inject] private EnemyService _enemyService;
        [Inject] private MatchService _matchService;
        [Inject] private SignalBus _signalBus;

        public void Init()
        {
            if (_isInit) UnInit();
            InitStateMachine();
            IsDone = false;
            _currentStateIndex = 0;
            _matchService.SetOverwrittenDuration(true, matchDuration);
            _fsm.SetState(_statesQueue[_currentStateIndex]);
            _deviceSubscription = _inputDeviceService.CurrentDevice.Subscribe(OnInputDeviceChanged);
            OnInputDeviceChanged(_inputDeviceService.CurrentDevice.Value);
            _isInit = true;
        }

        private void UnInit()
        {
            if (!_isInit) return;
            _deviceSubscription?.Dispose();
            _matchService.SetOverwrittenDuration(false);
            UnInitStateMachine();
            _isInit = false;
        }

        private void InitStateMachine()
        {
            _fsm = new StateMachine<TutorialState>();
            _fireState = new FireTutorialState(this, _signalBus);
            _movementState = new MovementTutorialState(this, _signalBus);
            _lookState = new LookTutorialState(this, _signalBus);
            _matchstickState = new MatchstickTutorialState(this, _signalBus);
            _mimicState = new MimicTutorialState(this, _signalBus, _levelController, _enemyService);

            _statesQueue = new TutorialState[]
            {
                _lookState, _movementState, _fireState, _matchstickState, _mimicState
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
            if (_isLookAtActiveProp)
            {
                var activeProp = _enemyService.Enemies[0].Prop;
                SmoothLookAt(activeProp.transform.position + Vector3.up * .3f);
            }

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
                Keyboard or Mouse => keyboardMouseUI,
                Gamepad => gamepadUI,
                Touchscreen => touchUI,
                _ => null
            };

            if (_currentUI && ui != _currentUI)
            {
                _currentUI.UnInit();
            }

            if (ui && (!_currentUI || (ui != _currentUI && !_currentUI.IsInit)))
                ui.Init(this);

            _currentUI = ui;
        }

        public void TweenPlayerPosToShootMimicPos(float duration, Action onComplete = null)
        {
            player.transform.DOMove(playerShootMimicPos.position, duration).OnComplete(() => { onComplete?.Invoke(); });
        }

        private void SmoothLookAt(Vector3 targetPosition)
        {
            var targetRotation = Quaternion.LookRotation(targetPosition - cameraRoot.transform.position);
            cameraRoot.transform.rotation =
                Quaternion.Slerp(cameraRoot.transform.rotation, targetRotation, Time.deltaTime * 2f);
        }

        public void SetLookAtActiveProp(bool isOn) => _isLookAtActiveProp = isOn;
        // {
        // var activeProp = _levelController.ActiveProps[0];
        // player.transform.LookAt(activeProp.transform.position);
        // }
    }
}