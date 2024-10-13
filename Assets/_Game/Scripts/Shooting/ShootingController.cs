using Enemy;
using Enemy.Events;
using InputUtils;
using Player;
using Shooting;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ShootingController : MonoBehaviour
{
    public ParticleSystem gunshot;
    [SerializeField] private ShootWeaponAnimation anim;
    [SerializeField] private PropDetector propDetector;

    [Inject] private PlayerInputsService _playerInputService;
    [Inject] private ShootingService _shootingService;
    [Inject] private EnemyService _enemyService;
    [Inject] private SignalBus _signalBus;

    private void Update()
    {
        TryToShoot();
    }

    private void TryToShoot()
    {
        if (!_playerInputService.CurrentState.fire)
            return;

        _playerInputService.CurrentState.fire = false;

        if (!_shootingService.TryShoot())
            return;

        gunshot.time = 0f;
        gunshot.Play();
        anim.PlayShootAnim();

        if (_enemyService.Enemies.Count == 0)
        {
            return;
        }

        var enemy = _enemyService.Enemies[0];

        if (propDetector.Detected?.gameObject != enemy.gameObject)
        {
            _signalBus.Fire<MissedEnemyEvent>();
            return;
        }

        enemy.Kill();
    }
}