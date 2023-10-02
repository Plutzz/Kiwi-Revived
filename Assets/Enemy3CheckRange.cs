using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3CheckRange : MonoBehaviour
{
    bool playerInRange = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInRange = true;
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
}
