using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : MonoBehaviour
{
    public static AttackPattern bulletState;
    public new CircleCollider2D collider;
    public new ParticleSystem particleSystem;
    public SpriteRenderer spriteRenderer;
    public int damage = 5;
    public float velocity = 10.0f;
    [SerializeField] private int playerScriptsChildNumber = 5;
    private bool extinguished = false;
    [SerializeField] private ParticleSystem.MinMaxGradient extinguishedGradient;
    [SerializeField] private Color extinguishedColor;


    void Start ()
    {
        Destroy(this.gameObject, 5f);
        transform.Rotate(new Vector3(0, 0, Random.Range(-10, 20)), Space.Self);
    }

    void Update ()
    {
        transform.position += transform.right * velocity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!extinguished && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.GetChild(playerScriptsChildNumber).GetComponent<PlayerHealth>().takeDamage(damage);
        } else if(other.gameObject.CompareTag("WaterBullet"))
        {
            Extinguish();
        }
    }

    public void Extinguish()
    {
        collider.isTrigger = false;
        ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.color = extinguishedGradient;
        spriteRenderer.color = extinguishedColor;
        gameObject.layer = LayerMask.NameToLayer("Ground");
        extinguished = true;
    }

    public enum AttackPattern
    {
        Volley,
        Homing,
        Rain
    }
}
