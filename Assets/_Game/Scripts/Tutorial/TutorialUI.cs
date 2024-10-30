using TMPro;
using Tutorial.States;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI progressText;

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

            progressText.gameObject.SetActive(!_controller.IsDone && _controller.CurrentState != null);
        }

        private void UpdateProgress()
        {
            if (_controller.IsDone)
                return;
            progressText.text = $"{_controller.CurrentState?.Name} ({_controller.CurrentState?.Progress:P2})";
        }
    }
}