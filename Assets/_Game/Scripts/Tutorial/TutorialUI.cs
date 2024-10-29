using TMPro;
using Tutorial.States;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private Image progressImage;
        [SerializeField] private TextMeshProUGUI progressText;

        [SerializeField] private GameObject movementGraphics;
        [SerializeField] private GameObject fireGraphics;
        [SerializeField] private GameObject lookGraphics;
        [SerializeField] private GameObject matchstickGraphics;

        private TutorialState _prevState;
        private TutorialController _controller;
        private bool _isInit;

        public void Init(TutorialController controller)
        {
            if (_isInit) UnInit();
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
        }

        private void Update()
        {
            if (!_isInit) return;
            if (_prevState != _controller.CurrentState)
            {
                ToggleGraphics();
            }

            UpdateProgress();
            _prevState = _controller.CurrentState;
        }

        private void ToggleGraphics()
        {
            movementGraphics.SetActive(_controller.CurrentState is MovementTutorialState);
            fireGraphics.SetActive(_controller.CurrentState is FireTutorialState);
            lookGraphics.SetActive(_controller.CurrentState is LookTutorialState);
            matchstickGraphics.SetActive(_controller.CurrentState is MatchstickTutorialState);

            progressText.gameObject.SetActive(!_controller.IsDone && _controller.CurrentState != null);
            progressImage.gameObject.SetActive(!_controller.IsDone && _controller.CurrentState != null);
        }

        private void UpdateProgress()
        {
            if (_controller.IsDone) return;
            progressImage.fillAmount = _controller.CurrentState?.Progress ?? 0;
            // format 
            var formattedProgress = Mathf.RoundToInt(progressImage.fillAmount * 100);
            progressText.text = $"{_controller.CurrentState?.Name} ({formattedProgress}%)";
        }
    }
}