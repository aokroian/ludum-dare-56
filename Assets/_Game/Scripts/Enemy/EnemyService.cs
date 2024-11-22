using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enemy.Events;
using GameLoop.Events;
using Level;
using Matchstick;
using MimicSpace;
using Shooting;
using Shooting.Events;
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
        private MatchService _matchService;
        private ShootingService _shootingService;

        private List<Enemy> _enemies = new();

        [Inject]
        private void Initialize(
            Config config,
            SignalBus signalBus,
            MatchService matchService,
            ShootingService shootingService,
            LevelController level
        )
        {
            _config = config;
            _signalBus = signalBus;
            _shootingService = shootingService;
            _matchService = matchService;
            _level = level;

            _signalBus.Subscribe<ShootingEvent>(OnShot);
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
            if (_shootingService.Ammo <= 0 || _matchService.Matches <= 0)
            {
                AttackPlayer();
                return;
            }

            var oldEnemy = oldProp.GetComponent<Enemy>();
            _enemies.Remove(oldEnemy);
            Object.Destroy(oldEnemy);
            var prop = _level.MovePropToAnotherSurface(oldProp);
            var enemy = prop.gameObject.AddComponent<Enemy>();
            enemy.Init(prop, _signalBus, MoveProp, OnEnemyDied);
            _enemies.Add(enemy);

            _signalBus.Fire(new EnemyRepositionEvent(enemy.transform.position));
        }

        private void AttackPlayer()
        {
            if (_enemies.Count == 0)
                return;
            var mimic = Object.Instantiate(_config.enemyPrefab, _enemies[0].transform.position, Quaternion.identity);
            _level.ToggleAllColliders(true);
            _signalBus.Fire(new AttackPlayerEvent(mimic.transform));
            var seq = DOTween.Sequence();
            seq.AppendInterval(2f);
            seq.Append(_enemies[0].Prop.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InSine));
            var mainCam = Camera.main;
            var dirFromCamToMimic = mimic.transform.position - mainCam.transform.position;
            var targetPos = mainCam.transform.position + dirFromCamToMimic.normalized;
            targetPos.y = 1f;
            var distance = Vector3.Distance(mimic.transform.position, targetPos);
            var duration = distance / 1.7f;
            seq.Join(mimic.transform.DOMove(targetPos, duration).SetEase(Ease.OutQuint));
            seq.OnComplete(
                () =>
                {
                    Object.Destroy(mimic.gameObject);
                    _signalBus.Fire(new GameOverEvent());
                });
            seq.Play();
        }

        private void OnEnemyDied(Enemy enemy)
        {
            _signalBus.Fire(new EnemyGotHitEvent(enemy.transform.position));
            var deathEffect = Object.Instantiate(
                _config.deathEffect,
                enemy.transform.position,
                Quaternion.identity,
                enemy.transform);
            deathEffect.PlayDeathEffect(
                enemy.Prop,
                () =>
                {
                    if (_enemies.Count(it => it.Alive) <= 0)
                    {
                        Debug.LogWarning("All enemies are dead");
                        _signalBus.Fire<AllEnemiesDeadEvent>();
                    }
                });
        }

        private void OnShot()
        {
            if (_shootingService.Ammo <= 0 && !_matchService.IsLit())
                AttackPlayer();
        }

        [Serializable]
        public class Config
        {
            public int enemiesCount;
            public EnemyMimic enemyPrefab;
            public EnemyDeathEffect deathEffect;
        }
    }
}