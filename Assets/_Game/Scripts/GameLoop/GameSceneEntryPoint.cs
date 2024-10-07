using _GameTemplate.Scripts.SceneManagement;
using Enemy;
using Enemy.Events;
using GameLoop.Events;
using Level;
using Player;
using R3;
using UnityEngine;
using Zenject;

namespace GameLoop
{
    public class GameSceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private CutsceneController _cutsceneController;
        
        
        [Inject]
        private SignalBus _signalBus;
        [Inject]
        private GameStateProvider _gameStateProvider;
        [Inject]
        private LevelController _levelController;
        [Inject]
        private EnemyService _enemyService;

        private GameStateData _gameState;
        
        private void Start()
        {
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
            _gameStateProvider.LoadGameState();
            _gameState = _gameStateProvider.GameState;
            
            _signalBus.Subscribe<AllEnemiesDeadEvent>(OnAllEnemiesDied);
        }
        
        private void StartGame()
        {
            _levelController.ResetProps(10);
            _enemyService.ResetEnemies();
            
            _signalBus.Fire(new NightStartedEvent(_gameState.night));
        }
        
        private void OnAllEnemiesDied()
        {
            // TODO: dialogue
            
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