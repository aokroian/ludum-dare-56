using System;
using System.Collections.Generic;
using System.Linq;
using Enemy.Events;
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
                var deathEffect = enemy.GetComponentInChildren<EnemyDeathEffect>();
                if (deathEffect)
                    Object.Destroy(deathEffect.gameObject);
                Object.Destroy(enemy);
            }
            _enemies.Clear();
            
            for (int i = 0; i < _config.enemiesCount; i++)
            {
                var props = _level.ActiveProps;
                var prop = props[UnityEngine.Random.Range(0, props.Count)];
                var enemy = prop.gameObject.AddComponent<Enemy>();
                // var enemyInstance = Object.Instantiate(_config.enemyPrefab, Vector3.down, Quaternion.identity);
                enemy.Init(prop, _signalBus, MoveProp, OnEnemyDied);
                _enemies.Add(enemy);
            }
        }

        private void MoveProp(Prop oldProp)
        {
            var oldEnemy = oldProp.GetComponent<Enemy>();
            _enemies.Remove(oldEnemy);
            Object.Destroy(oldEnemy);
            var prop = _level.MovePropToAnotherSurface(oldProp);
            prop.SelectRandomVariation();
            var enemy = prop.gameObject.AddComponent<Enemy>();
            enemy.Init(prop, _signalBus, MoveProp, OnEnemyDied);
            _enemies.Add(enemy);
            
            _signalBus.Fire(new EnemyRepositionEvent(enemy.transform.position));
        }

        private void OnEnemyDied(Enemy enemy)
        {
            _signalBus.Fire(new EnemyDiedEvent(enemy.transform.position));
            var deathEffect = Object.Instantiate(_config.deathEffect, enemy.transform.position, Quaternion.identity, enemy.transform);
            deathEffect.PlayDeathEffect(enemy.Prop, () =>
            {
                if (_enemies.Count(it => it.Alive) <= 0)
                {
                    Debug.LogWarning("All enemies are dead");
                    _signalBus.Fire<AllEnemiesDeadEvent>();
                } 
                    
            });
            // TODO: start next night
        }

        [Serializable]
        public class Config
        {
            public int enemiesCount;
            public Mimic enemyPrefab;
            public EnemyDeathEffect deathEffect;
        }
    }
}