using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    void Update()
    {
        if(Input.anyKeyDown)
        {
            startMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
