using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaVomitHealth : EnemyHealth
{
    public GameObject explosion;
    public GameObject parent;
    private bool instantiatedExplosion = false;
    protected override void OnDeath()
    {
        Instantiate(explosion, parent.transform.localPosition, Quaternion.identity);
        instantiatedExplosion = true;
        Destroy(parent);
    }
}
