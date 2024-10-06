using Unity.VisualScripting;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    private Camera _mainCamera;
    private float _timeForNextShoot;

    public GameObject enemy;
    public int ammo = 2;

    public float viewportPointX = .3f;
    public float viewportPointY = .7f;
    public float viewportPointZ = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shot();
        }
    }

    private void Shot()
    {
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
