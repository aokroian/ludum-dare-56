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

        private void Start()
        {
            Debug.Log("Tutorial scene loaded");
            tutorialController.Init();
            _matchService.SetInfiniteMatches(true);
            _shootingService.SetInfiniteAmmo(true);
        }
    }
}