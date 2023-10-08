using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("This script can be attached to any enemy that can take damage, make sure you have a function called OnEnemyDeath in a different script that will handle the death of the enemy")]
public class EnemyHealth : DamageableEntity
{
    public int maxHp = 100;
    void Awake ()
    {
        currentHp = maxHp;
    }

    
    public override void takeDamage (int damage)
    {
        base.takeDamage(damage);
        Debug.Log("Current HP: " + currentHp);
        if(currentHp <= 0)
            gameObject.SendMessage("OnEnemyDeath", SendMessageOptions.DontRequireReceiver);
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

