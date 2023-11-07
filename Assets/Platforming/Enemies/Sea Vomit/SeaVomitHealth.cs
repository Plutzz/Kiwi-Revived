using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaVomitHealth : EnemyHealth
{
    public GameObject explosion;
    public GameObject parent;
    public float blinkDuration = 1f;
    public float blinkInterval = 0.1f;
    public float explosionRadius = 5f;
    public int explosionDamage = 10;
    [SerializeField] private CircleCollider2D deathCollider;
    [SerializeField] private BoxCollider2D rangeCollider;

    protected override void OnDeath() {
        rangeCollider.enabled = false;
        StartCoroutine(DelayExplosion());
    }

    IEnumerator DelayExplosion() {
        float endTime = Time.time + blinkDuration;

        while (Time.time < endTime) {
            SpriteRenderer[] sprites = parent.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites) {
                sprite.enabled = !sprite.enabled;
            } 
            float interval = blinkInterval * (1 - (Time.time - endTime) / blinkDuration);
            yield return new WaitForSeconds(interval);
        }

        parent.GetComponentInChildren<SpriteRenderer>().enabled = true;  
        Collider2D collider = Physics2D.OverlapCircle(transform.position, explosionRadius, LayerMask.GetMask("Player"));
        if (collider != null)
            PlayerHealth.Instance.takeDamage(explosionDamage);
        Instantiate(explosion, parent.transform.localPosition, Quaternion.identity);
        Destroy(parent);
    }

}
