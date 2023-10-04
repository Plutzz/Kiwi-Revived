using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Put script on doors
public class Door : MonoBehaviour
{
    //enter name of scene you want to enter in editor
    public string room;

    public void enterDoor ()
    {
        //loads scene with name equal to room
        SceneManager.LoadScene(room);
    }
}
