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
        var newObject = Instantiate(WaterPrefab, this.transform.position, transform.rotation, this.transform);
        newObject.GetComponentInChildren<ProjectileScript>().setVelocityDecrease(Enemy3.GetComponent<Enemy3AI>().velocityDecrease);
        //Debug.Log(rangeCheck.GetComponent<Enemy3CheckRange>().facingRight);
        newObject.GetComponentInChildren<ProjectileScript>().setDirection(rangeCheck.GetComponent<Enemy3CheckRange>().facingRight);
        AudioManager.Instance.PlaySound(AudioManager.Sounds.SeafoamShot);
    } 
}
