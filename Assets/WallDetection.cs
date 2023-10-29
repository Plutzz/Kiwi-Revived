using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    public Enemy1AI enemy1Movement;

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.CompareTag("Ground"))
        {
            enemy1Movement.moveRight = !enemy1Movement.moveRight;
            //enemy1Movement.currentState = Enemy1AI.State.Idle;
        }

        
        //enemy1Movement.grounded = false;
    }
}
