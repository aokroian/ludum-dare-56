using GameLoop.Events;
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

        private const float FirstMessageLifetime = 8f;
        private const float NextMessageLifetime = 5f;

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
            _signalBus.Subscribe<GameSceneLoadedEvent>(OnGameSceneLoaded);
        }

        private void OnGameSceneLoaded()
        {
            SpawnMessage("I'm being hunted by a mimic. I have to find and shoot it.", FirstMessageLifetime);
        }

        private void OnShoot()
        {
            var variant = Random.Range(0, 3);
            var bulletsLeftPart = _shootingService.Ammo >= 1
                ? $"{_shootingService.Ammo} bullets left."
                : "No bullets left.";
            switch (variant)
            {
                case 0:
                    SpawnMessage("The shot echoed.\n" + bulletsLeftPart, NextMessageLifetime);
                    break;
                case 1:
                    SpawnMessage("The shot echoed. It's getting closer.\n" + bulletsLeftPart, NextMessageLifetime);
                    break;
                case 2:
                    SpawnMessage("The shot echoed. It's getting colder.\n" + bulletsLeftPart, NextMessageLifetime);
                    break;
            }
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
                    SpawnMessage("The match went out.\n" + matchesLeftPart, NextMessageLifetime);
                    break;
                case 1:
                    SpawnMessage("The match went out. It's dark now.\n" + matchesLeftPart, NextMessageLifetime);
                    break;
                case 2:
                    SpawnMessage("The match went out. It's getting cold.\n" + matchesLeftPart, NextMessageLifetime);
                    break;
            }
        }

        private void SpawnMessage(string text, float lifetime)
        {
            var spawnedMessage = Instantiate(messagePrefab, messagesParent.transform);
            spawnedMessage.tmp.text = text;
            spawnedMessage.Activate(lifetime);
            messagesParent.enabled = false;
            messagesParent.enabled = true;
            Canvas.ForceUpdateCanvases();
        }
    }
}