using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float velocity = 30.0f;
    public int damage = 10;

    //lowest and highest direction flame can spawn
    [Header ("Vertical direction of flames")]
    public int maxRange, minRange = 0;

    private Rigidbody2D fireParticle;

    // Start is called before the first frame update
    void Start()
    {
        fireParticle = GetComponent<Rigidbody2D>();

        //just for fire to have flamethrower effect, changes rotation
        this.transform.rotation = Quaternion.Euler(0, 0, Random.Range(minRange, maxRange));

        //speed of projectile
        fireParticle.velocity = transform.right * velocity;
    }
}
