using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public Enemy1AI enemy1Movement;

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            if(enemy1Movement.state != Enemy1AI.State.Attack)
            enemy1Movement.state = Enemy1AI.State.Chase;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            if(enemy1Movement.grounded == true)
            {
                enemy1Movement.state = Enemy1AI.State.Idle;
            }
        }
    }
}
