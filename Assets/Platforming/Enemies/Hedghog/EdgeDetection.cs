using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetection : MonoBehaviour
{
    public Enemy1AI enemy1Movement;

    private void OnTriggerExit2D(Collider2D other) {

        enemy1Movement.moveRight = !enemy1Movement.moveRight;
        enemy1Movement.currentState = Enemy1AI.State.Idle;

        
        enemy1Movement.grounded = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        enemy1Movement.grounded = true;
    }
}
