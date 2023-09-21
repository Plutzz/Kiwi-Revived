using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Singleton<BulletSpawner>
{
    public GameObject WaterPrefab;
    public void Shoot()
    {
        Instantiate(WaterPrefab, transform.position, transform.rotation);
    }
}
