using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : Singleton<PlayerController>
{
    private BulletSpawner BulletSpawnPoint;
    public Animator animator;
    private WeaponDisplay weaponDisplay;
    public float FireRate = 0.1f;      // Time between shots
    private float shootTimer;


    private void Start()
    {
        weaponDisplay = FindObjectOfType<WeaponDisplay>();
        BulletSpawnPoint = BulletSpawner.Instance;
        shootTimer = FireRate;
    }
    
    void Update()
    {

        // If the mouse button is held down, shoot 1 water particle every FireRate seconds
        shootTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && shootTimer <= 0)
        {
            BulletSpawnPoint.Shoot();

            shootTimer = FireRate;
        }

        if (Input.GetMouseButton(0)){
            animator.SetBool("isTrigger", true);
        }
        else{
            animator.SetBool("isTrigger", false);
        }
    

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BulletSpawnPoint.changeBulletType(0);
            weaponDisplay.ChangeWeaponImage(0);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            BulletSpawnPoint.changeBulletType(1);
            weaponDisplay.ChangeWeaponImage(1);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            BulletSpawnPoint.changeBulletType(2);
            weaponDisplay.ChangeWeaponImage(2);
        }

    }

    
}
    