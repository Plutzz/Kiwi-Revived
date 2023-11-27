using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KainHealth : EnemyHealth
{
    protected override void OnDeath()
    {
        Destroy(transform.parent.gameObject);
    }
}
