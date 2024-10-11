using System;
using _GameTemplate.Scripts.SceneManagement;
using InputUtils;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Dialogue
{
    public class EscManager : MonoBehaviour
    {
        [SerializeField] private Button escButtonTouchscreen;
        [SerializeField] private GameObject escGamepad;
        [SerializeField] private GameObject escKeyboard;
        [Inject] private InputDeviceService _inputDeviceService;
        [Inject] private PlayerInputsService _playerInputService;

        private float _delayAfterSceneLoad = 2f;

        private void Start()
        {
            _inputDeviceService.CurrentDevice.Subscribe(OnInputDeviceChanged).AddTo(this);
            escButtonTouchscreen.onClick.AddListener(ExitToMenu);
        }

        private void Update()
        {
            _delayAfterSceneLoad -= Time.deltaTime;
            if (_playerInputService.CurrentState.escape)
            {
                _playerInputService.CurrentState.escape = false;
                ExitToMenu();
            }
        }

        private void ExitToMenu()
        {
            if (_delayAfterSceneLoad > 0)
            {
                _playerInputService.CurrentState.escape = false;
                return;
            }

            CustomSceneManager.LoadScene("Menu");
        }

        private void OnInputDeviceChanged(InputDevice device)
        {
            escButtonTouchscreen.gameObject.SetActive(device is Touchscreen);
            escGamepad.gameObject.SetActive(device is Gamepad);
            escKeyboard.SetActive(device is not Touchscreen);
        }
    }
}