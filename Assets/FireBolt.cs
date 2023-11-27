using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : MonoBehaviour
{
    public static AttackPattern bulletState;
    public int damage = 5;
    public float homingTime = 1.5f;
    public float velocity = 10.0f;
    public float lifeTime = 5f;
    public new CircleCollider2D collider;
    public new ParticleSystem particleSystem;
    public SpriteRenderer spriteRenderer;
    Transform player;
    [SerializeField] private int playerScriptsChildNumber = 5;
    private bool extinguished = false;
    [SerializeField] private ParticleSystem.MinMaxGradient extinguishedGradient;
    [SerializeField] private Color extinguishedColor;
    private AttackPattern spawnedState;


    void Start ()
    {
        spawnedState = bulletState;
        player = PlayerMovement.Instance.transform;
        switch (spawnedState)
        {
            case(AttackPattern.Volley):
            {
                Destroy(this.gameObject, 5f);
                transform.Rotate(new Vector3(0, 0, Random.Range(-10f, 20f)), Space.Self);
                break;
            }
            case(AttackPattern.Homing):
            {
                Destroy(this.gameObject, 5f);
                break;
            }
            case(AttackPattern.Rain):
            {
                Destroy(this.gameObject, 1f);
                break;
            }
            case(AttackPattern.RainFall):
            {
                Destroy(this.gameObject, 5f);
                break;
            }
        }

    }

    void Update ()
    {
        if (player == null) return;
        switch (spawnedState)
        {
            case(AttackPattern.Volley):
            {
                transform.position += transform.right * velocity * Time.deltaTime;
                break;
            }

            case(AttackPattern.Homing):
            {

                Vector3 direction = player.transform.position - transform.position;

                float distance = velocity * Time.deltaTime;

                if(homingTime > 0)
                {
                    homingTime -= Time.deltaTime;
                    transform.Translate(direction.normalized * distance, Space.World);

                    float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, rot + 90);
                } else {
                    transform.position += transform.up * velocity * Time.deltaTime;
                }

                break;
            }

            case(AttackPattern.Rain):
            {
                transform.position += transform.up * velocity * 5 * Time.deltaTime;
                break;
            }

            case(AttackPattern.RainFall):
            {
                transform.position -= transform.up * velocity * Time.deltaTime;
                break;
            }
        }
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
        Rain,
        RainFall
    }
}
