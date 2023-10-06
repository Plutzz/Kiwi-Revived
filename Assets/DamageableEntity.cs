using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEntity : MonoBehaviour
{
    protected int currentHp;
    public virtual void takeDamage(int damage)
    {
        currentHp -= damage;
    }
    
    public int GetCurrentHp()
    {
        return currentHp;
    }

}
