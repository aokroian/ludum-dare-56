using Enemy.Events;
using GameLoop.Events;
using Matchstick;
using Matchstick.Events;
using Shooting;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;
using Zenject;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private VariablesGroupAsset localizationVariables;
        
        [Space]
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

        private async void OnEnemyGotHit(EnemyGotHitEvent data)
        {
            var message = await Strings.Localize(Strings.GetHitEnemyMessage());
            SpawnMessage(message, LongMessageDuration);
        }

        private async void OnMissedEnemy()
        {
            ((IntVariable)localizationVariables["bullets"]).Value = _shootingService.Ammo;
            // var bulletsLeftPart = _shootingService.Ammo > 0
            //     ? $"<color=#EED17A>{_shootingService.Ammo}</color> bullets left."
            //     : "No bullets left. This is the end.";
            var firstRow = await Strings.Localize(Strings.GetMissedEnemyMessage());
            var secondRow = await Strings.Localize(_shootingService.Ammo > 0 ? Strings.BulletsLeftNotZero : Strings.BulletsLeftZero);
            SpawnMessage(firstRow + "\n" + secondRow, DefaultMessageDuration);
        }

        private async void OnNightStarted(NightStartedEvent data)
        {
            ClearSpawnedMessages();
            var task = data.Night == 1
                ? Strings.Localize(Strings.GetFirstNightMessage())
                : Strings.Localize(Strings.GetNextNightMessage());
            var message = await task;
            SpawnMessage(message, LongMessageDuration);
        }

        private void ClearSpawnedMessages()
        {
            foreach (Transform child in messagesParent.transform)
                Destroy(child.gameObject);
        }

        private async void OnMatchWeanOut()
        {
            ((IntVariable)localizationVariables["matches"]).Value = _matchService.Matches;
            // var matchesLeftPart = _matchService.Matches > 0
            //     ? $"<color=#EED17A>{_matchService.Matches}</color> matches left."
            //     : "No matches left. I'm doomed.";
            var firstRow = await Strings.Localize(Strings.GetMatchWentOutMessage());
            var secondRow = await Strings.Localize(_matchService.Matches > 0 ? Strings.MatchesLeftNotZero : Strings.MatchesLeftZero);
            SpawnMessage($"{firstRow}\n{secondRow}", DefaultMessageDuration);
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