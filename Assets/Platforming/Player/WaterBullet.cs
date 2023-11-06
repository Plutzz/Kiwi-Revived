using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    public float velocity = 20.0f;
    public int damage = 10;
    public static float distanceBetweenBullets = 0f;

    //lowest and highest direction water can spawn
    [Header ("Spread of bullets")]
    public int maxRange, minRange = 0;
    
    private Rigidbody2D waterParticle;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = PlayerMovement.Instance.gameObject;

        PlayerMovement playerMovement = PlayerMovement.Instance;

        Transform rotatePointTransform = RotationPoint.Instance.transform;

        waterParticle = GetComponent<Rigidbody2D>();

        float playerHorizontalMovementSpeed = playerMovement.getCurrentVelocityX();

        float rotatePointAngle = rotatePointTransform.rotation.eulerAngles.z;

        //if facing right
        if((rotatePointAngle > 270) || (rotatePointAngle < 90))
        {
            playerHorizontalMovementSpeed = Mathf.Cos(Mathf.Deg2Rad * rotatePointAngle) * playerHorizontalMovementSpeed;

        //if facing left
        } else if ((rotatePointAngle > 90) && (rotatePointAngle < 270))
        {
            playerHorizontalMovementSpeed = Mathf.Cos(Mathf.Deg2Rad * rotatePointAngle) * playerHorizontalMovementSpeed;
        }

        transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(maxRange, minRange));
        transform.Translate(0, distanceBetweenBullets, 0);
        distanceBetweenBullets *= -1;
        

        //speed of projectile
        waterParticle.velocity = transform.right * (velocity + Mathf.Abs(playerHorizontalMovementSpeed));
        //Debug.Log(Mathf.Abs(waterParticle.velocity.x));
    }

}
