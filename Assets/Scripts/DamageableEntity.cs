using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEntity : MonoBehaviour
{
    public int currentHp;
    public virtual void takeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
            OnDeath();
    }
    
    public int GetCurrentHp()
    {
        return currentHp;
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

}
