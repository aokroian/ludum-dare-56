using _GameTemplate.Scripts.SceneManagement;
using Enemy;
using Enemy.Events;
using GameLoop.Events;
using InputUtils;
using Level;
using Matchstick;
using Player;
using R3;
using Shooting;
using UnityEngine;
using Zenject;

namespace GameLoop
{
    public class GameSceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private CutsceneController _cutsceneController;

        [Inject] private SignalBus _signalBus;
        [Inject] private GameStateProvider _gameStateProvider;
        [Inject] private LevelController _levelController;
        [Inject] private EnemyService _enemyService;
        [Inject] private MatchService _matchService;
        [Inject] private ShootingService _shootingService;
        [Inject] private InputDeviceService _inputDeviceService;

        private GameStateData _gameState;
        
        private void Start()
        {
            _signalBus.Fire<GameSceneLoadedEvent>();
            Debug.Log("Game scene loaded");
            Configure();
            StartGame();
        }

        private void OnDestroy()
        {
            _gameStateProvider.SaveGameState();
        }

        private void Configure()
        {
            _matchService.SetInfiniteMatches(false);
            _shootingService.SetInfiniteAmmo(false);
            
            _gameStateProvider.LoadGameState();
            _gameState = _gameStateProvider.GameState;
            
            _signalBus.Subscribe<AllEnemiesDeadEvent>(OnAllEnemiesDied);
        }
        
        public void StartGame()
        {
            _levelController.ResetProps(_gameState.night + 2);
            _enemyService.ResetEnemies();
            _signalBus.Fire(new NightStartedEvent(_gameState.night));
            _inputDeviceService.SetInputDeviceBasedOnPlatform();
        }
        
        private void OnAllEnemiesDied()
        {
            // TODO: dialogue
            if (_gameState.night >= 7)
            {
                _signalBus.Fire<GameFinishedEvent>();
                _gameState.night = 1;
                return;
            }
            
            Observable.Timer(System.TimeSpan.FromSeconds(1)).ObserveOnCurrentSynchronizationContext().Subscribe(_ =>
            {
                Debug.Log("After timer");
                _signalBus.Fire(new NightFinishedEvent(_gameState.night));
                
                _gameState.night++;
                _gameStateProvider.SaveGameState();
                StartGame();
                Debug.LogWarning("Level restarted");
                // CustomSceneManager.LoadScene("Game", false, false);
            });
        }
    }
}