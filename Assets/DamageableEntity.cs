using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEntity : MonoBehaviour
{
    int currentHp;
    public void takeDamage(int damage)
    {
        currentHp -= damage;
    }
}
