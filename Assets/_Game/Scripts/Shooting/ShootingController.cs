using Enemy;
using InputUtils;
using Player;
using Shooting;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private ShootWeaponAnimation anim;
    [SerializeField] private PropDetector propDetector;

    [Inject]
    private PlayerInputsService _playerInputService;

    [Inject]
    private ShootingService _shootingService;

    [Inject]
    private EnemyService _enemyService;

    private Camera _mainCamera;

    public ParticleSystem gunshot;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        Shot();
    }

    private void Shot()
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

        if (!propDetector.Detected)
        {
            return;
        }

        Debug.Log($"{enemy.name} killed");
        enemy.Kill();
    }
}