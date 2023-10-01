using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Enemy3ProjectileSpawner : MonoBehaviour
{
    public GameObject WaterPrefab;
    public GameObject Enemy3;
    int num = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shoot()
    {
        num += 1;
        var newObject = Instantiate(WaterPrefab, this.transform.position, this.transform.rotation, this.transform);
        newObject.name = "Enemy3Projectile" + num;
        GameObject.Find("Enemy3Projectile" + num).GetComponent<ProjectileScript>().setVelocityDecrease(Enemy3.GetComponent<Enemy3AI>().velocityDecrease);
    } 
}
