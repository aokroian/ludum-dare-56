using System;
using System.Threading;
using DG.Tweening;
using GameLoop.Events;
using InputUtils;
using Player.Events;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Player
{
    public class CutsceneController: MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup cutsceneCanvasGroup;
        [SerializeField] private TMP_Text cutsceneCanvasText;
        
        private SignalBus _signalBus;
        private PlayerInputsService _playerInputsService;
        
        // Fast workaround
        private static CutsceneController _instance;

        [Inject]
        private void Initialize(SignalBus signalBus, PlayerInputsService playerInputsService)
        {
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
            
            
            PrepareCutscene();
            
            DontDestroyOnLoad(this);
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
            
            var seq = DOTween.Sequence();
            cutsceneCanvasText.text = "Night " + e.Night;
            seq.AppendInterval(1f);
            seq.Append(cutsceneCanvasGroup.DOFade(0, 0f));
            seq.onComplete += () =>
            {
                _playerInputsService.EnableInput();
            };
            // _playerInputsService.EnableInput();
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
                    if (Input.GetMouseButton(0))
                    {
                        Debug.Log("Disposed");
                        disposable.Dispose();
                    }
                }).AddTo(ref disposable);
        }
    }
}