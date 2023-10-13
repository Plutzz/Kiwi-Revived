using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class BulletSpawner : Singleton<BulletSpawner>
{
    public BulletType bulletType;

    public GameObject FirePrefab, WaterPrefab, PoisonPrefab;

    [Header("Fire Rate")]
    public float fireFireRate = 0.1f;
    public float waterFireRate = 0.1f;
    public float poisonFireRate = 2.0f;

    [Header("Water Settings")]
    public float WaterOffset = 0.5f;

    public PlayerController playerController;

    BulletType[] bulletTypes = (BulletType[])System.Enum.GetValues (typeof(BulletType));

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
                Vector3 _pos = transform.position;
                _pos.y += Random.Range(-.25f, .25f);
                Instantiate(WaterPrefab, _pos, transform.rotation);
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

    public void changeBulletType(int type)
    {
        bulletType = bulletTypes[type];
    }

    public enum BulletType
    {
        Fire,
        Water,
        Poison
    }
}
