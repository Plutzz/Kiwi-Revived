using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KainHealth : EnemyHealth
{
    [SerializeField] private HealthBar healthBar;
    void Awake()
    {
        base.Awake();
        healthBar.SetMaxHealth(maxHp);
    }

    public override void takeDamage(int _damage)
    {
        base.takeDamage(_damage);
        healthBar.DamageHealth((float) _damage / maxHp);
    }

    protected override void OnDeath()
    {
        Destroy(transform.parent.gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WaterBullet"))
            takeDamage(collision.gameObject.GetComponent<WaterBullet>().damage);
        else if (collision.gameObject.CompareTag("PoisonBullet"))
            takeDamage(collision.gameObject.GetComponent<PoisonBullet>().damage);
        else if (collision.gameObject.CompareTag("FireBullet"))
            takeDamage(collision.gameObject.GetComponent<FireBullet>().damage);
        else if (collision.gameObject.CompareTag("Player"))
            PlayerHealth.Instance.takeDamage(collisionDamage, transform);
    }
}
