using Cinemachine;
using DG.Tweening;
using Enemy.Events;
using GameLoop;
using GameLoop.Events;
using InputUtils;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Player
{
    public class CutsceneController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject gamepadRestartText;
        [SerializeField] private GameObject keyboardRestartText;
        [SerializeField] private Button touchRestartButton;

        [SerializeField] private CanvasGroup cutsceneCanvasGroup;
        [SerializeField] private TMP_Text nightCountText;
        [SerializeField] private GameObject nightsTextsParent;
        [SerializeField] private GameObject gameCompletedText;

        [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
        [SerializeField] private CinemachineVirtualCamera attackVirtualCamera;
        [SerializeField] private CinemachineVirtualCamera bedVirtualCamera;
        [SerializeField] private CinemachineBrain camBrain;

        [SerializeField] private Transform playerStartPos;
        [SerializeField] private CharacterController player;
        [SerializeField] private RectTransform gameOverScreen;

        private GameStateProvider _gameStateProvider;
        private SignalBus _signalBus;
        private PlayerInputsService _playerInputsService;
        private InputDeviceService _inputDeviceService;

        [Inject]
        private void Initialize(
            SignalBus signalBus,
            PlayerInputsService playerInputsService,
            GameStateProvider gameStateProvider,
            InputDeviceService inputDeviceService
        )
        {
            gameOverScreen.gameObject.SetActive(false);
            _playerInputsService = playerInputsService;
            _inputDeviceService = inputDeviceService;
            _inputDeviceService.CurrentDevice
                .Subscribe(
                    device =>
                    {
                        gamepadRestartText.SetActive(device is Gamepad);
                        keyboardRestartText.SetActive(device is Keyboard or Mouse);
                        touchRestartButton.transform.parent.gameObject.SetActive(device is Touchscreen);
                    })
                .AddTo(this);
            touchRestartButton.onClick.AddListener(() => { _playerInputsService.CurrentState.restart = true; });
            
            _gameStateProvider = gameStateProvider;
            _signalBus = signalBus;
            _signalBus.Subscribe<NightStartedEvent>(OnNightStarted);
            _signalBus.Subscribe<NightFinishedEvent>(OnNightFinished);
            _signalBus.Subscribe<GameFinishedEvent>(OnGameFinished);
            _signalBus.Subscribe<AttackPlayerEvent>(OnAttackPlayer);
            _signalBus.Subscribe<GameOverEvent>(OnGameOver);
            nightsTextsParent.SetActive(true);
            gameCompletedText.SetActive(false);
        }

        private void PrepareCutscene()
        {
            _playerInputsService.EnableInputs(PlayerInputFlags.NonGameplay);
            canvas.gameObject.SetActive(true);
            cutsceneCanvasGroup.alpha = 1;
            nightsTextsParent.SetActive(true);
            gameCompletedText.SetActive(false);
        }

        private void OnNightFinished(NightFinishedEvent e)
        {
            PrepareCutscene();
            nightCountText.text = (e.Night + 1).ToString();
        }

        private void OnNightStarted(NightStartedEvent e)
        {
            PrepareCutscene();
            camBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0);
            bedVirtualCamera.enabled = true;

            player.enabled = false;
            player.transform.position = playerStartPos.position;
            player.transform.rotation = playerStartPos.rotation;
            player.enabled = true;

            var seq = DOTween.Sequence();
            nightCountText.text = e.Night.ToString();
            seq.AppendInterval(1f);
            seq.Append(cutsceneCanvasGroup.DOFade(0, .5f));
            seq.onComplete += BedAnimation;
        }

        private void BedAnimation()
        {
            camBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2);
            bedVirtualCamera.enabled = false;

            DisposableBag disposable = default;

            Observable.EveryUpdate()
                .ObserveOn(UnityFrameProvider.Update)
                .Subscribe(
                    _ =>
                    {
                        if (camBrain.IsBlending)
                            return;

                        _playerInputsService.EnableInputs(PlayerInputFlags.All);
                        disposable.Dispose();
                    })
                .AddTo(ref disposable);
        }

        private void OnGameFinished()
        {
            PrepareCutscene();
            nightsTextsParent.SetActive(false);
            gameCompletedText.SetActive(true);
            _gameStateProvider.ClearGameState();
        }

        private async void OnAttackPlayer(AttackPlayerEvent e)
        {
            await Observable.Timer(System.TimeSpan.FromSeconds(0.2f))
                .ObserveOnMainThread()
                .WaitAsync();
            camBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0.5f);
            _playerInputsService.EnableInputs(PlayerInputFlags.NonGameplay);
            attackVirtualCamera.transform.position = mainVirtualCamera.transform.position;
            attackVirtualCamera.enabled = true;
            attackVirtualCamera.LookAt = e.EnemyTransform;
        }

        private void OnGameOver()
        {
            attackVirtualCamera.enabled = false;
            _playerInputsService.CurrentState.restart = false;

            canvas.gameObject.SetActive(true);
            gameOverScreen.gameObject.SetActive(true);

            DisposableBag disposable = default;
            Observable.EveryUpdate()
                .ObserveOn(UnityFrameProvider.Update)
                .Subscribe(
                    _ =>
                    {
                        if (_playerInputsService.CurrentState.restart)
                        {
                            _playerInputsService.CurrentState.restart = false;
                            disposable.Dispose();
                            gameOverScreen.gameObject.SetActive(false);
                            FindFirstObjectByType<GameSceneEntryPoint>().StartGame();
                        }
                    })
                .AddTo(ref disposable);
        }
    }
}