using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject rotationPoint;
    private BulletSpawner BulletSpawnPoint;

    public float FireRate = 0.1f;      // Time between shots
    private float shootTimer;

    private Vector3 mousePos;

    private void Start()
    {
        BulletSpawnPoint = BulletSpawner.Instance;
        shootTimer = FireRate;
    }
    void Update()
    {
        followMouse();

        shootTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && shootTimer <= 0)
        {
            BulletSpawnPoint.Shoot();
            shootTimer = FireRate;
        }

    }

    private void followMouse()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        rotationPoint.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    
}
    