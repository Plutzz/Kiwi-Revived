using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Enemy3ProjectileSpawner : MonoBehaviour
{
    public GameObject WaterPrefab;
    public GameObject Enemy3;
    public GameObject rangeCheck;
    int num = 0;
    public void shoot()
    {
        num += 1;
        var newObject = Instantiate(WaterPrefab, this.transform.position, this.transform.rotation, this.transform);
        newObject.name = "Enemy3Projectile" + num;
        GameObject.Find("Enemy3Projectile" + num).GetComponent<ProjectileScript>().setVelocityDecrease(Enemy3.GetComponent<Enemy3AI>().velocityDecrease);
        Debug.Log(rangeCheck.GetComponent<Enemy3CheckRange>().facingRight);
        GameObject.Find("Enemy3Projectile" + num).GetComponent<ProjectileScript>().setDirection(rangeCheck.GetComponent<Enemy3CheckRange>().facingRight);
    } 
}
