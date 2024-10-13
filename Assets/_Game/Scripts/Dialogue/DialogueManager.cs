using Enemy.Events;
using GameLoop.Events;
using Matchstick;
using Matchstick.Events;
using Shooting;
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

        private const float LongMessageDuration = 8f;
        private const float DefaultMessageDuration = 5f;

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
            _signalBus.Subscribe<MissedEnemyEvent>(OnMissedEnemy);
            _signalBus.Subscribe<EnemyGotHitEvent>(OnEnemyGotHit);
            _signalBus.Subscribe<NightStartedEvent>(OnNightStarted);
        }

        private void OnEnemyGotHit(EnemyGotHitEvent data)
        {
            SpawnMessage(Strings.GetHitEnemyMessage(), LongMessageDuration);
        }

        private void OnMissedEnemy()
        {
            var bulletsLeftPart = _shootingService.Ammo > 0
                ? $"<color=#EED17A>{_shootingService.Ammo}</color> bullets left."
                : "No bullets left. This is the end.";
            SpawnMessage(Strings.GetMissedEnemyMessage() + "\n" + bulletsLeftPart, DefaultMessageDuration);
        }

        private void OnNightStarted(NightStartedEvent data)
        {
            ClearSpawnedMessages();
            SpawnMessage(data.Night == 1 ? Strings.GetFirstNightMessage() : Strings.GetNextNightMessage(),
                LongMessageDuration);
        }

        private void ClearSpawnedMessages()
        {
            foreach (Transform child in messagesParent.transform)
                Destroy(child.gameObject);
        }

        private void OnMatchWeanOut()
        {
            var matchesLeftPart = _matchService.Matches > 0
                ? $"<color=#EED17A>{_matchService.Matches}</color> matches left."
                : "No matches left. I'm doomed.";
            SpawnMessage($"{Strings.GetMatchWentOutMessage()}\n{matchesLeftPart}", DefaultMessageDuration);
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