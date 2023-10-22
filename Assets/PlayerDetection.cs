using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public Enemy1AI enemy1Movement;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            enemy1Movement.state = Enemy1AI.State.Chase;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            enemy1Movement.state = Enemy1AI.State.Idle;
        }
    }
}
