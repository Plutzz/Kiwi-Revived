using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapper : MonoBehaviour
{

    public Collider2D playerDetector;
    // Update is called once per frame
    void Update()
    {
        if (playerDetector.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            Debug.Log("Player Detected");
        }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         Debug.Log("Player Detected");
    //     }
    // }
}
