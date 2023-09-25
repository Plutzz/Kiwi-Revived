using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Singleton<BulletSpawner>
{
    public BulletType bulletType;

    public GameObject FirePrefab, WaterPrefab, PoisonPrefab;

    public PlayerController playerController;

    public void Shoot()
    {
        switch ((int)bulletType)
        {
            //Fire
            case(0):
            {
                playerController.FireRate = 0.1f;
                Instantiate(FirePrefab, transform.position, transform.rotation);
                break;
            }

            //Water
            case(1):
            {
                playerController.FireRate = 0.1f;
                Instantiate(WaterPrefab, transform.position, transform.rotation);
                break;
            }

            //Poison
            case(2):
            {
                playerController.FireRate = 2f;
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
