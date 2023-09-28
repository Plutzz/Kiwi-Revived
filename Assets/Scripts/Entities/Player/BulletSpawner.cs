using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Singleton<BulletSpawner>
{
    public BulletType bulletType;

    public GameObject FirePrefab, WaterPrefab, PoisonPrefab;

    [Header("Fire Rate")]
    public float fireFireRate = 0.1f;
    public float waterFireRate = 0.1f;
    public float poisonFireRate = 2.0f;

    public PlayerController playerController;

    public void Shoot()
    {
        switch ((int)bulletType)
        {
            //Fire
            case(0):
            {
                playerController.FireRate = fireFireRate;

                Instantiate(FirePrefab, transform.position, transform.rotation);
                break;
            }

            //Water
            case(1):
            {
                playerController.FireRate = waterFireRate;

                Instantiate(WaterPrefab, transform.position, transform.rotation);
                break;
            }

            //Poison
            case(2):
            {
                playerController.FireRate = poisonFireRate;

                Instantiate(PoisonPrefab, transform.position, transform.rotation);
                break;
            }
        }
    }

    public enum BulletType
    {
        Fire,
        Water,
        Poison
    }
}
