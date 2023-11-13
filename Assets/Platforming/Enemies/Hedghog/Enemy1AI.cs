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
    public int verticalMultiplier = 5;
    public State currentState = State.Idle;
    public bool grounded = true;
    public bool moveRight = false;
    public Transform sprite;

    private float distance = 999f;
    private bool canJump = true;
    private Vector3 currentVelocity;
    //private GameObject PlayerScripts;
    
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
        if (PlayerMovement.Instance != null)
            player = PlayerMovement.Instance.transform;
    }

    void Update()
    {
        if(PlayerMovement.Instance != null) 
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
    void SpriteFlip() {
        if (moveRight) {
            sprite.localScale = new Vector3(-Mathf.Abs(sprite.localScale.x), sprite.localScale.y, sprite.localScale.z);
        } else {
            sprite.localScale = new Vector3(Mathf.Abs(sprite.localScale.x), sprite.localScale.y, sprite.localScale.z);
        }
    }
    void Patrol ()
    {
        SpriteFlip();
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
        SpriteFlip();
        if(distance > 0 && grounded && canJump)
        {
            currentVelocity = rb.velocity;
            currentVelocity.x = runSpeed;
            rb.velocity = currentVelocity;
            moveRight = true;
        } else if(distance < 0 && grounded && canJump)
        {
            currentVelocity = rb.velocity;
            currentVelocity.x = -runSpeed;
            rb.velocity = currentVelocity;
            moveRight = false;
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
        }

        if(Mathf.Abs(distance) > distanceToPounce)
        {
            currentState = State.Chase;
        }
    }

    private IEnumerator Jump()
    {
        grounded = false;
        canJump = false;
        rb.AddForce(new Vector2(rb.velocity.x * verticalMultiplier, jumpForce));
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

}
