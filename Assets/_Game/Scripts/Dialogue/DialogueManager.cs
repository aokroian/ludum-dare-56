using Matchstick;
using Matchstick.Events;
using Shooting;
using Shooting.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueMessage messagePrefab;
        [SerializeField] private VerticalLayoutGroup messagesParent;

        private SignalBus _signalBus;
        private MatchService _matchService;
        private ShootingService _shootingService;

        [Inject]
        private void Initialize(
            SignalBus signalBus,
            MatchService matchService,
            ShootingService shootingService)
        {
            _signalBus = signalBus;
            _matchService = matchService;
            _shootingService = shootingService;

            _signalBus.Subscribe<MatchWentOutEvent>(OnMatchWeanOut);
            _signalBus.Subscribe<ShootingEvent>(OnShoot);
        }

        private void OnShoot()
        {
            var variant = Random.Range(0, 3);
            var ammoLeftPart = _shootingService.Ammo >= 1
                ? $"{_shootingService.Ammo} ammo left."
                : "No ammo left. This is the end.";
        }

        private void OnMatchWeanOut()
        {
            var variant = Random.Range(0, 3);
            var matchesLeftPart = _matchService.Matches >= 1
                ? $"{_matchService.Matches} matches left."
                : "No matches left. I'm doomed.";
            switch (variant)
            {
                case 0:
                    SpawnMessage("The match went out.\n" + matchesLeftPart);
                    break;
                case 1:
                    SpawnMessage("The match went out. It's dark now.\n" + matchesLeftPart);
                    break;
                case 2:
                    SpawnMessage("The match went out. It's getting cold.\n" + matchesLeftPart);
                    break;
            }
        }

        private void SpawnMessage(string text)
        {
            var spawnedMessage = Instantiate(messagePrefab, messagesParent.transform);
            spawnedMessage.tmp.text = text;
            messagesParent.enabled = false;
            messagesParent.enabled = true;
            Canvas.ForceUpdateCanvases();
        }
    }
}