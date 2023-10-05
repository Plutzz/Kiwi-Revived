using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float velocity = 20.0f;
    public int damage = 10;
    public float velocityDecrease = 1.0f;
    public bool isFacingRight;
 

    //lowest and highest direction flame can spawn
    [Header("Spread of bullets")]
    public int maxRange, minRange = 0;

    private Rigidbody2D waterParticle;

    // Start is called before the first frame update
    void Start()
    {
        int direction = 1;
        if (isFacingRight)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        waterParticle = GetComponent<Rigidbody2D>();

        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(maxRange, minRange));
        Debug.Log(velocityDecrease);
        //speed of projectile
        waterParticle.velocity = transform.right * velocityDecrease * velocity * direction;
    }

    public void setVelocityDecrease(float velocityDec)
    {
        velocityDecrease = velocityDec;
    }

    public void setDirection(bool facingRight)
    {
        isFacingRight = facingRight;
    }

}
