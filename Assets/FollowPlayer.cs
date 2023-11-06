using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool follow = false;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if(follow)
        {
            transform.position = player.transform.position;
        }
    }
}
