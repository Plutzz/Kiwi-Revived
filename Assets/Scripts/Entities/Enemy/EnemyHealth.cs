using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : DamageableEntity
{
    public int maxHp = 100;
    void Awake ()
    {
    }

    // Maybe when it is at a certain health, it will change its behavior
    // public override void takeDamage (int damage)
    // {
    //     base.takeDamage(damage);
    //     if(currentHp <= 0)
    //         Destroy(gameObject);
    // }    

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WaterBullet"))
        {
            takeDamage(other.gameObject.GetComponent<WaterBullet>().damage);
        }

        // Add future bullet types here
    }
}
