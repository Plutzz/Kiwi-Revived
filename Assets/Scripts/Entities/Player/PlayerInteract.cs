using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public bool canInteract = false;
    
    Door door = null;

    void Update ()
    {
        if((Input.GetKeyDown(KeyCode.F)) && (canInteract == true))
        {
            door.enterDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            canInteract = true;

            door = collision.GetComponent<Door>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            canInteract = false;

            door = null;
        }
    }
}
