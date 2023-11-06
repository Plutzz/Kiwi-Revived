using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : MonoBehaviour
{
    public new CircleCollider2D collider;
    public new ParticleSystem particleSystem;
    public SpriteRenderer spriteRenderer;
    public int damage = 5;
    public float velocity = 10.0f;
    [SerializeField] private int playerScriptsChildNumber = 5;


    void Start ()
    {
        Destroy(this.gameObject, 5f);
    }

    void Update ()
    {
        transform.position += transform.right * velocity * Time.deltaTime; 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("nohit");
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.GetChild(playerScriptsChildNumber).GetComponent<PlayerHealth>().takeDamage(damage);
            Extinguish();
            Debug.Log("hit");
        }
        Debug.Log("nohit2");
    }

    public void Extinguish()
    {
        collider.isTrigger = false;
        particleSystem.Stop();
        spriteRenderer.color = Color.blue;
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }
}
