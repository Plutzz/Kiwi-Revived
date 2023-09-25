using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    public float velocity = 20.0f;
    public int damage = 10;
    
    private Rigidbody2D waterParticle;

    // Start is called before the first frame update
    void Start()
    {
        waterParticle = GetComponent<Rigidbody2D>();

        //speed of projectile
        waterParticle.velocity = transform.right * velocity;
    }

}
