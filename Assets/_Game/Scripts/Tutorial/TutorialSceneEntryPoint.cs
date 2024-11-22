using InputUtils;
using Matchstick;
using Shooting;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class TutorialSceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private TutorialController tutorialController;
        [Inject] private MatchService _matchService;
        [Inject] private ShootingService _shootingService;
        [Inject] private InputDeviceService _inputDeviceService;

        private void Start()
        {
            Debug.Log("Tutorial scene loaded");
            tutorialController.Init();
            _matchService.SetInfiniteMatches(true);
            _shootingService.SetInfiniteAmmo(true);
            _inputDeviceService.SetInputDeviceBasedOnPlatform();
        }
    }
}