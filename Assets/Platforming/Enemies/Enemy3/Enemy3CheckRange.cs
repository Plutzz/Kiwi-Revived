using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3CheckRange : MonoBehaviour
{
    bool playerInRange = false;
    public bool facingRight = false;
    public GameObject parent;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = true;
            if (collision.transform.position.x < this.transform.position.x && facingRight)
            {
                parent.transform.localScale = new Vector3(parent.transform.localScale.x * -1, 1, 1);
                facingRight = false;
            }
            if (collision.transform.position.x > this.transform.position.x && !facingRight)
            {
                parent.transform.localScale = new Vector3(parent.transform.localScale.x * -1, 1, 1);
                facingRight = true;
            }
        }
   
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = false;
            
        }
    }

    public bool isPlayerInRange()
    {
        if (playerInRange)
        {
            return true;   
        }
        return false;
    }

    public bool isFacingRight()
    {
        if (facingRight)
        {
            return true;
        }
        return false;
    }
}
