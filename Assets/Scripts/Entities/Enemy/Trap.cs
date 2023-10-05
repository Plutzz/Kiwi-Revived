using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public AudioSource boom;


    // Idea: When the trap hits the player, it will explode and deal damage to the player
    // The trap will also be destroyed
    // When the trap hits the ground, it will freeze the x and y position of the trap so that it doesn't move/interfere with anything

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("Ground"));
        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(boom);
            // DAMAGE THE PLAYER
            Destroy(this.gameObject);
        }
    }
}
