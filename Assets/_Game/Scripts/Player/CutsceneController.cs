using _GameTemplate.Scripts.SceneManagement;
using Cinemachine;
using DG.Tweening;
using Enemy.Events;
using GameLoop;
using GameLoop.Events;
using InputUtils;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace Player
{
    public class CutsceneController: MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup cutsceneCanvasGroup;
        [SerializeField] private TMP_Text cutsceneCanvasText;

        [SerializeField] private CinemachineVirtualCamera mainVirtualCamera;
        [SerializeField] private CinemachineVirtualCamera attackVirtualCamera;
        [SerializeField] private CinemachineVirtualCamera bedVirtualCamera;
        [SerializeField] private CinemachineBrain camBrain;

        [SerializeField] private Transform playerStartPos;
        [SerializeField] private CharacterController player;

        [SerializeField] private RectTransform gameOverScreen;
        
        
        private SignalBus _signalBus;
        private PlayerInputsService _playerInputsService;
        
        // Fast workaround
        private static CutsceneController _instance;

        [Inject]
        private void Initialize(SignalBus signalBus, PlayerInputsService playerInputsService)
        {
            gameOverScreen.gameObject.SetActive(false);
            
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            
            _playerInputsService = playerInputsService;
            _signalBus = signalBus;
            _signalBus.Subscribe<NightStartedEvent>(OnNightStarted);
            _signalBus.Subscribe<NightFinishedEvent>(OnNightFinished);
            _signalBus.Subscribe<GameFinishedEvent>(OnGameFinished);
            _signalBus.Subscribe<AttackPlayerEvent>(OnAttackPlayer);
            _signalBus.Subscribe<GameOverEvent>(OnGameOver);
            
            
            PrepareCutscene();
            
            // DontDestroyOnLoad(this);
        }

        private void PrepareCutscene()
        {
            _playerInputsService.DisableInput();
            canvas.gameObject.SetActive(true);
            cutsceneCanvasGroup.alpha = 1;
            cutsceneCanvasText.text = "";
        }

        private void OnNightFinished(NightFinishedEvent e)
        {
            PrepareCutscene();
            cutsceneCanvasText.text = "Night " + e.Night + 1;
        }

        private void OnNightStarted(NightStartedEvent e)
        {
            Debug.Log("Night started");
            // TODO: Start cutscene
            // TODO: Move parameters to config
            
            // var cam = Camera.main;
            // var camBrain = cam.GetComponent<CinemachineBrain>();
            camBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0);
            
            bedVirtualCamera.enabled = true;
            
            player.enabled = false;
            player.transform.position = playerStartPos.position;
            player.transform.rotation = playerStartPos.rotation;
            player.enabled = true;
            
            var seq = DOTween.Sequence();
            cutsceneCanvasText.text = "Night " + e.Night;
            seq.AppendInterval(1f);
            seq.Append(cutsceneCanvasGroup.DOFade(0, 0f));
            seq.onComplete += BedAnimation;
            // _playerInputsService.EnableInput();
        }

        private void BedAnimation()
        {
            // var cam = Camera.main;
            // var camBrain = cam.GetComponent<CinemachineBrain>();
            camBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2);
            bedVirtualCamera.enabled = false;

            DisposableBag disposable = default;
            
            Observable.EveryUpdate()
                .ObserveOn(UnityFrameProvider.Update)
                .Subscribe(_ =>
                {
                    if (camBrain.IsBlending)
                        return;
                    
                    _playerInputsService.EnableInput();
                    disposable.Dispose();
                }).AddTo(ref disposable);
        }
        
        private void OnGameFinished()
        {
            PrepareCutscene();
            cutsceneCanvasText.text = "Finally, I am safe";
            DisposableBag disposable = default;
            Observable.EveryUpdate()
                .ObserveOn(UnityFrameProvider.Update)
                .Subscribe(_ =>
                {
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        Debug.Log("Disposed");
                        disposable.Dispose();
                        CustomSceneManager.LoadScene("Menu");
                    }
                }).AddTo(ref disposable);
        }
        
        public void OnAttackPlayer(AttackPlayerEvent e)
        {
            camBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0.2f);
            _playerInputsService.DisableInput();
            attackVirtualCamera.transform.position = mainVirtualCamera.transform.position;
            attackVirtualCamera.enabled = true;
            attackVirtualCamera.LookAt = e.EnemyTransform;
        }
        
        private void OnGameOver()
        {
            attackVirtualCamera.enabled = false;
            
            canvas.gameObject.SetActive(true);
            gameOverScreen.gameObject.SetActive(true);
            
            DisposableBag disposable = default;
            Observable.EveryUpdate()
                .ObserveOn(UnityFrameProvider.Update)
                .Subscribe(_ =>
                {
                    if (Input.GetKey(KeyCode.R))
                    {
                        Debug.Log("Disposed");
                        disposable.Dispose();
                        gameOverScreen.gameObject.SetActive(false);
                        FindFirstObjectByType<GameSceneEntryPoint>().StartGame();
                    }
                }).AddTo(ref disposable);
        }
    }
}