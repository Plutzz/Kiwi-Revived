using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public AudioSource boom;
    public int damage = 10;
    private Rigidbody2D rb;

    // Idea: When the trap hits the player, it will explode and deal damage to the player
    // The trap will also be destroyed
    // When the trap hits the ground, it will freeze the x and y position of the trap so that it doesn't move/interfere with anything

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.12f, LayerMask.GetMask("Ground"));
        
        Debug.DrawRay(transform.position, Vector2.down * 0.12f, Color.red);

        if (hit == true) {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(boom);
            PlayerHealth.Instance.takeDamage(damage);
            Destroy(gameObject);

        }
    }
}
