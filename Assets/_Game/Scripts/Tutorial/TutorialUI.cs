using TMPro;
using Tutorial.States;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private PulseEffect controlsTutorialTitle;
        [SerializeField] private GameObject controlsTutorialDoneIcon;
        [SerializeField] private Slider controlsTutorialProgress;
        [FormerlySerializedAs("controlsProgressTmp")] [SerializeField] private TextMeshProUGUI controlsHintTmp;

        [SerializeField] private PulseEffect gameplayTutorialTitle;
        [SerializeField] private Slider gameplayTutorialProgress;
        [SerializeField] private GameObject gameplayTutorialDoneIcon;
        [FormerlySerializedAs("gameplayProgressTmp")] [SerializeField] private TextMeshProUGUI gameplayHintTmp;

        [SerializeField] private GameObject movementGraphics;
        [SerializeField] private GameObject fireGraphics;
        [SerializeField] private GameObject lookGraphics;
        [SerializeField] private GameObject matchstickGraphics;
        [SerializeField] private GameObject finalGraphics;

        private TutorialState _prevState;
        private bool _prevIsDone;
        private TutorialController _controller;
        private bool _isInit;

        public void Init(TutorialController controller)
        {
            if (_isInit) UnInit();
            gameObject.SetActive(true);
            _controller = controller;
            ToggleGraphics();
            _isInit = true;
        }

        public void UnInit()
        {
            if (!_isInit) return;
            _controller = null;
            _prevState = null;
            _isInit = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_isInit) return;
            if (_prevState != _controller.CurrentState || _prevIsDone != _controller.IsDone)
            {
                ToggleGraphics();
            }

            UpdateProgress();
            _prevState = _controller.CurrentState;
            _prevIsDone = _controller.IsDone;
        }

        private void ToggleGraphics()
        {
            finalGraphics.SetActive(_controller.IsDone);
            movementGraphics.SetActive(_controller.CurrentState is MovementTutorialState && !_controller.IsDone);
            fireGraphics.SetActive(_controller.CurrentState is FireTutorialState && !_controller.IsDone);
            lookGraphics.SetActive(_controller.CurrentState is LookTutorialState && !_controller.IsDone);
            matchstickGraphics.SetActive(_controller.CurrentState is MatchstickTutorialState && !_controller.IsDone);

            var isGameplayState = _controller.CurrentState is MimicTutorialState;
            var isControlsTutorialDone = isGameplayState || _controller.IsDone;
            controlsTutorialDoneIcon.SetActive(isControlsTutorialDone);
            controlsTutorialTitle.Toggle(!isControlsTutorialDone);
            controlsHintTmp.gameObject.SetActive(!isControlsTutorialDone);

            gameplayTutorialDoneIcon.SetActive(_controller.IsDone);
            gameplayTutorialTitle.Toggle(isGameplayState && !_controller.IsDone);
            gameplayHintTmp.gameObject.SetActive(isGameplayState && !_controller.IsDone);
        }

        private void UpdateProgress()
        {
            if (_controller.IsDone || _controller.CurrentState == null)
            {
                controlsTutorialProgress.value = 1f;
                gameplayTutorialProgress.value = 1f;
                return;
            }

            if (_controller.CurrentState is MimicTutorialState)
            {
                gameplayHintTmp.text = $"{_controller.CurrentState?.Hint}";
            }
            else
            {
                controlsHintTmp.text =
                    // $"{_controller.CurrentState?.Hint} ({_controller.CurrentState?.Progress:P2})";
                    $"{_controller.CurrentState?.Hint}";
            }

            controlsTutorialProgress.value = _controller.ControlsProgress;
            gameplayTutorialProgress.value = _controller.GameplayProgress;
        }
    }
}