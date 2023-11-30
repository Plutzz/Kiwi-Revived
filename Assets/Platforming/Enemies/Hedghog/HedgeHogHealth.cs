using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgeHogHealth : EnemyHealth
{
    [SerializeField] private ParticleSystem deathParticles;

    protected override void OnDeath()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity).Play();
        Destroy(gameObject);
    }
}
