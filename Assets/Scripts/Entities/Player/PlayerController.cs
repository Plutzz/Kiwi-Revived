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



        // If the mouse button is held down, shoot 1 water particle every FireRate seconds
        shootTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && shootTimer <= 0)
        {
            BulletSpawnPoint.Shoot();

            shootTimer = FireRate;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BulletSpawnPoint.changeBulletType(0);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            BulletSpawnPoint.changeBulletType(1);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            BulletSpawnPoint.changeBulletType(2);
        }

    }

    // Rotates the rotation point to follow the mouse
    private void followMouse()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        rotationPoint.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    
}
    