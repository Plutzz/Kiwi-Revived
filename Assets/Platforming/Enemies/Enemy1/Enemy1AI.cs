using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1AI : MonoBehaviour
{
    // public CircleCollider2D visionRange;
    // public CircleCollider2D attackRange;
    private Rigidbody2D rb;
    private Transform player;
    public float speed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 100;
    public float jumpCooldown = 1f;
    public float distanceToPounce = 5f;
    public State currentState = State.Idle;
    public bool grounded = true;
    public bool moveRight = false;

    private float distance;
    private bool canJump = true;
    private Vector3 currentVelocity;
    
    public enum State {
        Idle,
        Chase,
        Attack
    }

    public State state
    {
        get { return currentState; }
        set { currentState = value; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerMovement.Instance.transform;
    }

    void Update()
    {
        distance = player.position.x - transform.position.x;

        switch((int)currentState)
        {
            case(0):
            {
                Patrol();
                break;
            }

            case(1):
            {
                FollowPlayer();
                break;
            }

            case(2):
            {
                FollowPlayer();
                Pounce();
                break;
            }
        }
    }

    void Patrol ()
    {
        if(moveRight)
        {

            currentVelocity = rb.velocity;
            currentVelocity.x = speed;
            rb.velocity = currentVelocity;
        } else {
            currentVelocity = rb.velocity;
            currentVelocity.x = -speed;
            rb.velocity = currentVelocity;
        }
    }

    void FollowPlayer ()
    {
        if(distance > 0 && canJump)
        {
            currentVelocity = rb.velocity;
            currentVelocity.x = runSpeed;
            rb.velocity = currentVelocity;
            moveRight = true;
        } else if(distance < 0 && canJump)
        {
            currentVelocity = rb.velocity;
            currentVelocity.x = -runSpeed;
            rb.velocity = currentVelocity;
            moveRight = false;
        } else {
            currentVelocity = rb.velocity;
            currentVelocity.x = 0;
            rb.velocity = currentVelocity;
        }

        if(Mathf.Abs(distance) <= distanceToPounce)
        {
            currentState = State.Attack;
        }
    }

    void Pounce ()
    {
        if(grounded && canJump)
        {
            StartCoroutine(Jump());
            grounded = false;
        }

        if(Mathf.Abs(distance) > distanceToPounce)
        {
            currentState = State.Chase;
        }
    }

    private IEnumerator Jump()
    {
        rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

}
