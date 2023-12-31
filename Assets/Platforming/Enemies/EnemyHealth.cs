using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("This script can be attached to any enemy that can take damage, make sure you have a function called OnEnemyDeath in a different script that will handle the death of the enemy")]
public class EnemyHealth : DamageableEntity
{
    public int maxHp = 100;
    public int collisionDamage = 10;
    [SerializeField] private DamageFlash damageFlash;

    protected void Awake ()
    {
        currentHp = maxHp;
    }

    private void Start()
    {
        if(damageFlash == null)
        {
            damageFlash = GetComponent<DamageFlash>();
        }

    }


    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WaterBullet")) 
            takeDamage(other.gameObject.GetComponent<WaterBullet>().damage);
        else if (other.gameObject.CompareTag("PoisonBullet"))
            takeDamage(other.gameObject.GetComponent<PoisonBullet>().damage);
        else if (other.gameObject.CompareTag("FireBullet"))
            takeDamage(other.gameObject.GetComponent<FireBullet>().damage);
        else if (other.gameObject.CompareTag("Player"))
            PlayerHealth.Instance.takeDamage(collisionDamage, transform);

    }

    public override void takeDamage(int _damage)
    {
        base.takeDamage(_damage);
        AudioManager.Instance.PlaySound(AudioManager.Sounds.EnemyHit);
        damageFlash.CallDamageFlash();
    }


}

