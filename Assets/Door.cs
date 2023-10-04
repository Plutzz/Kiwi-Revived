using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string room;

    public void enterDoor ()
    {
        SceneManager.LoadScene(room);
    }
}
