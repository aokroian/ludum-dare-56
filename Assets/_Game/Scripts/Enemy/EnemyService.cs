using System;
using System.Collections.Generic;
using Level;
using MimicSpace;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Enemy
{
    public class EnemyService
    {
        public IReadOnlyList<Enemy> Enemies => _enemies;

        private Config _config;
        private SignalBus _signalBus;
        private LevelController _level;
        
        private List<Enemy> _enemies = new ();

        [Inject]
        private void Initialize(Config config, SignalBus signalBus, LevelController level)
        {
            _config = config;
            _signalBus = signalBus;
            _level = level;
        }
        
        public void ResetEnemies()
        {
            foreach (var enemy in Enemies)
            {
                Object.Destroy(enemy);
            }
            _enemies.Clear();
            
            for (int i = 0; i < _config.enemiesCount; i++)
            {
                var props = _level.ActiveProps;
                var prop = props[UnityEngine.Random.Range(0, props.Count)];
                var enemy = prop.gameObject.AddComponent<Enemy>();
                var enemyInstance = Object.Instantiate(_config.enemyPrefab, Vector3.down, Quaternion.identity);
                enemy.Init(prop, _signalBus, enemyInstance, MoveProp, OnEnemyDied);
                _enemies.Add(enemy);
            }
        }

        private void MoveProp(Prop prop)
        {
            _level.MovePropToAnotherSurface(prop);
            prop.SelectRandomVariation();
        }
        
        private void OnEnemyDied(Enemy enemy)
        {
            _enemies.Remove(enemy);
            if (_enemies.Count <= 0)
            {
                Debug.LogWarning("All enemies are dead");
            }
            // TODO: start next night
        }

        [Serializable]
        public class Config
        {
            public int enemiesCount;
            public Mimic enemyPrefab;
        }
    }
}