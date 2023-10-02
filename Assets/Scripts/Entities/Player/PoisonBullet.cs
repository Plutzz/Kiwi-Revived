using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : MonoBehaviour
{
    public float velocity = 5.0f;
    public int damage = 10;

    private Rigidbody2D poisonParticle;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = PlayerMovement.Instance.gameObject;

        PlayerMovement playerMovement = PlayerMovement.Instance;

        Transform rotatePointTransform = RotationPoint.Instance.transform;

        poisonParticle = GetComponent<Rigidbody2D>();

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

        //speed of projectile
        poisonParticle.velocity = transform.right * (velocity + playerHorizontalMovementSpeed);
        Debug.Log(poisonParticle.velocity);
    }

}
