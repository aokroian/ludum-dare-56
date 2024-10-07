using GameLoop.Events;
using UnityEngine;
using Zenject;

namespace InputUtils
{
    public class CursorManager : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        private void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<GameSceneLoadedEvent>(OnGameSceneLoaded);
            _signalBus.Subscribe<MenuSceneLoadedEvent>(OnMenuSceneLoaded);
        }

        private void OnGameSceneLoaded()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnMenuSceneLoaded()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}