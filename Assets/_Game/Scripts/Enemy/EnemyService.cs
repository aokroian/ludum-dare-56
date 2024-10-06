using System;
using System.Collections.Generic;
using Level;
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
                enemy.Init(prop, _signalBus, MoveProp);
                _enemies.Add(enemy);
            }
        }

        private void MoveProp(Prop prop)
        {
            _level.MovePropToAnotherSurface(prop);
            prop.SelectRandomVariation();
        }

        [Serializable]
        public class Config
        {
            public int enemiesCount;
        }
    }
}