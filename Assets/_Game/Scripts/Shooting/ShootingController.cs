using InputUtils;
using Shooting;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private ShootWeaponAnimation anim;
    
    [Inject]
    private PlayerInputsService _playerInputService;
    
    [Inject]
    private ShootingService _shootingService;

    private Camera _mainCamera;

    public ParticleSystem gunshot;
    public GameObject enemy;

    public float viewportPointX = .3f;
    public float viewportPointY = .7f;
    public float viewportPointZ = 0f;

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
        
        // TODO: Recoil
        
        if (!IsObjectInView(enemy)) { return; }

        Debug.Log($"{enemy.name} killed");
        Destroy(enemy);
    }

    private bool IsObjectInView(GameObject desiredObject)
    {
        if (desiredObject is null || desiredObject.IsDestroyed()) { return false; }

        var viewportPoint = _mainCamera.WorldToViewportPoint(desiredObject.transform.position);

        return viewportPoint.z > viewportPointZ
               && viewportPoint.x > viewportPointX
               && viewportPoint.x < viewportPointY
               && viewportPoint.y > viewportPointX
               && viewportPoint.y < viewportPointY;
    }
}
