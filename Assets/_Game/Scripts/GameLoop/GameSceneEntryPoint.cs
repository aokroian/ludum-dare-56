using System;
using Enemy;
using Level;
using Player.Events;
using UnityEngine;
using Zenject;

namespace GameLoop
{
    public class GameSceneEntryPoint : MonoBehaviour
    {
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
        }
        
        private void StartGame()
        {
            _levelController.ResetProps(10);
            _enemyService.ResetEnemies();
            
            _signalBus.Fire(new NightStartedEvent(_gameState.night));
        }
    }
}