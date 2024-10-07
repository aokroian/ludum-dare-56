using DG.Tweening;
using InputUtils;
using Player.Events;
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
        
        [SerializeField] private CharacterController characterController;
        
        private SignalBus _signalBus;
        private PlayerInputsService _playerInputsService;

        [Inject]
        private void Initialize(SignalBus signalBus, PlayerInputsService playerInputsService)
        {
            _playerInputsService = playerInputsService;
            _signalBus = signalBus;
            _signalBus.Subscribe<NightStartedEvent>(OnNightStarted);
            
            
            PrepareCutscene();
        }

        private void PrepareCutscene()
        {
            _playerInputsService.DisableInput();
            canvas.gameObject.SetActive(true);
            cutsceneCanvasGroup.alpha = 1;
            cutsceneCanvasText.text = "";
        }

        private void OnNightStarted(NightStartedEvent e)
        {
            // TODO: Start cutscene
            // TODO: Move parameters to config
            
            var seq = DOTween.Sequence();
            cutsceneCanvasText.text = "Night " + e.Night;
            seq.AppendInterval(1f);
            seq.Append(cutsceneCanvasGroup.DOFade(0, 1f));
            seq.onComplete += () =>
            {
                _playerInputsService.EnableInput();
            };
            // _playerInputsService.EnableInput();
        }
    }
}