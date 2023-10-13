using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is a general script used to destroy sound effect game objects after a set amount of time 
// Attatch this to a prefab if you need to destroy it after some time

public class PrefabTimer : MonoBehaviour
{

    public float lifetime = 2f;                 // How long before the game object is destroyed
    protected float lifeTimeTimer = 0f;           // Tracks how long the game object has been alive for


    // Start is called before the first frame update
    protected virtual void Start()
    {

        lifeTimeTimer = lifetime;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0)
        {
            DestroyPrefab();
        }
    }

    protected virtual void DestroyPrefab()
    {
        Destroy(this.gameObject);
    }
}