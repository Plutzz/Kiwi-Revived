using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("This script can be attached to any enemy that can take damage")]
public class EnemyHealth : DamageableEntity
{
    public int maxHp = 100;
    void Awake ()
    {
        currentHp = maxHp;
    }

    // Maybe when it is at a certain health, it will change its behavior
    public override void takeDamage (int damage)
    {
        base.takeDamage(damage);
        Debug.Log("Current HP: " + currentHp);
        if(currentHp <= 0)
            Destroy(gameObject);
    }    

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WaterBullet")) 
            takeDamage(other.gameObject.GetComponent<WaterBullet>().damage);
        else if (other.gameObject.CompareTag("PoisonBullet"))
            takeDamage(other.gameObject.GetComponent<PoisonBullet>().damage);
        else if (other.gameObject.CompareTag("FireBullet"))
            takeDamage(other.gameObject.GetComponent<FireBullet>().damage);
    }
}

