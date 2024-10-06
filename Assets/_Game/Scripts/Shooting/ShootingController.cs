using InputUtils;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ShootingController : MonoBehaviour
{
    [Inject]
    private PlayerInputsService _playerInputService;

    private Camera _mainCamera;
    private float _timeForNextShoot;

    public ParticleSystem gunshot;
    public GameObject enemy;
    public int ammo = 2;

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
        if (!_playerInputService.CurrentState.fire) { return; }

        _playerInputService.CurrentState.fire = false;

        if (ammo <= 0)
        {
            Debug.Log($"No Ammo");
            return;
        }

        if (_timeForNextShoot > 0f)
        {
            Debug.Log($"Wait...");
            _timeForNextShoot =- Time.deltaTime;
            return;
        }

        Debug.Log($"shot");

        gunshot.Play();
        ammo--;
        _timeForNextShoot = 2.5f;

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
