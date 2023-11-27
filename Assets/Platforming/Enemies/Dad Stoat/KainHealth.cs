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
}
