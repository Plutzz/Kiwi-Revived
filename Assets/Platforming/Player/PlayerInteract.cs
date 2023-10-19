using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    //checks if you are over something interactable
    private bool canInteractWithDoor = false;
    
    //what door you can interact with
    Door door = null;

    void Update ()
    {
        //if you press f while you can interact with a door
        if((Input.GetKeyDown(KeyCode.F)) && (canInteractWithDoor == true))
        {
            //activates enterDoor method associated with door and loads new scene corresponding with door
            door.enterDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if you entered a trigger with tag "Door", makes player able to interact with door, and gets door script attached to door
        if(collision.gameObject.tag == "Door")
        {
            canInteractWithDoor = true;

            door = collision.GetComponent<Door>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if you exit a trigger with tag "Door", no longer able to interact with door, and sets door able to interact with to null
        if(collision.gameObject.tag == "Door")
        {
            canInteractWithDoor = false;

            door = null;
        }
    }
}
