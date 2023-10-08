using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : DamageableEntity
{
    public int maxHp = 100;
    public Image hpSliderFill;
    public Slider hpBar;

    void Awake ()
    {
        GetComponent<Slider>();
        currentHp = maxHp;
    }

    public override void takeDamage (int damage)
    {
        if(currentHp > 0)
        {
            currentHp -= damage;
        }
        
        float fillvalue = (float)currentHp/(float)maxHp;

        Debug.Log(fillvalue);

        hpBar.value = fillvalue;
    }

}
