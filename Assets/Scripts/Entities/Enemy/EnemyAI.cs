using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public enum EnemyAIState
    {
        Patrol, // Patrols around the map
        Chase, // Chase player to get to attack range includes jumping
        Attack, // Shoots at player
        Retreat // Runs away from player when they get too close
    } 

    // Variables
    public float speed = 10f;
    public float waitTime = 3f;
    public float moveTime = 5f;
    public bool isCoroutineRunning = false;

    // GameObjects
    public Trap _trap;
    public EnemyAIState state = EnemyAIState.Patrol;
    public Transform groundCheck;
    private Rigidbody2D rb;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask groundLayer;                 // Layer mask for the ground
    [SerializeField] private EdgeCollider2D leftGroundCheck;
    [SerializeField] private EdgeCollider2D rightGroundCheck;

    // Private Variables
    Vector2[] directions = { Vector2.left, Vector2.right };
    [SerializeField] private bool isLeftGrounded = false;
    [SerializeField] private bool isRightGrounded = false;
    [SerializeField] private float timer = 0f;
    [SerializeField] private Vector2 currentDirection;
    [SerializeField] private Vector2 lastKnownPosition;
    [SerializeField] private float visionRange = 40f;
    [SerializeField] private float visionAngle = 45f;



    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentDirection = directions[Random.Range(0, directions.Length)];

    }

    void Update()
    {
        switch (state)
        {
            case EnemyAIState.Patrol:
                if (!isCoroutineRunning) {
                    StartCoroutine(Idle());
                }
                CheckCollision();
                Move();
                CheckVision();
                
                break;
            // Move towards the last known position of the player when a ray cast (vision) hits the player
            case EnemyAIState.Chase:
                StopCoroutine(Idle());
                Debug.Log("Chase");
                CheckVision();
                CheckCollision();
                break;
            // Shoot at the player, once it is within attack range, in sight, not behind a wall, and not in minimum range (melee)
            case EnemyAIState.Attack:
                StopCoroutine(Idle());
                break;
            // Run away from the player, once it is within minimum range (melee)
            case EnemyAIState.Retreat:
                StopCoroutine(Idle());
                break;

        }
    }


    // Rotates the enemy towards the specified direction
    void RotateTowards(Vector2 direction)
    {
        // Get the angle between the enemy's forward vector and the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the enemy towards the direction vector
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Move()
    {
        if (timer >= moveTime || !isLeftGrounded || !isRightGrounded) {
            currentDirection = directions[Random.Range(0, directions.Length)];
            timer = 0f;
        }

        if (isLeftGrounded && isRightGrounded) {
            currentDirection.Normalize();
            rb.position += currentDirection * speed * Time.deltaTime;
        } else if (!isLeftGrounded) {
            currentDirection = Vector2.right; // Right = (1, 0)
            currentDirection.Normalize();
            rb.position += currentDirection * speed * Time.deltaTime;
        } else if (!isRightGrounded) {
            currentDirection = Vector2.left; // Left = (-1, 0)
            currentDirection.Normalize();
            rb.position += currentDirection * speed * Time.deltaTime;
        }

        timer += Time.deltaTime;
    }

    void CheckVision()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector2.Angle(directionToPlayer, transform.right);

        // Flip the angle if the enemy is facing left
        if (currentDirection.x < 0)
        {
            visionAngle = 180 - visionAngle;
            angleToPlayer = 180 - angleToPlayer;
        }

        if (distanceToPlayer > visionRange || angleToPlayer > visionAngle)
            state = EnemyAIState.Patrol;
        else if (distanceToPlayer <= visionRange && angleToPlayer <= visionAngle)
            state = EnemyAIState.Chase;
        
        Debug.DrawRay(transform.position, directionToPlayer.normalized * visionRange, Color.green);
    }

    void CheckCollision()
    {
        isLeftGrounded = IsColliderOverlappingFloor(leftGroundCheck);
        isRightGrounded = IsColliderOverlappingFloor(rightGroundCheck);

        Debug.DrawLine(leftGroundCheck.transform.position + (Vector3)leftGroundCheck.points[0], leftGroundCheck.transform.position + (Vector3)leftGroundCheck.points[1], Color.red);
        Debug.DrawLine(rightGroundCheck.transform.position + (Vector3)rightGroundCheck.points[0], rightGroundCheck.transform.position + (Vector3)rightGroundCheck.points[1], Color.red);
    }
    bool IsColliderOverlappingFloor(EdgeCollider2D collider)
    {
        Vector2 startPoint = collider.points[0] + (Vector2)transform.position;
        Vector2 endPoint = collider.points[1] + (Vector2)transform.position;
        bool isOverlapping = Physics2D.OverlapArea(startPoint, endPoint, groundLayer);
        // Debug.Log("Collider: " + collider.name + ", Start: " + startPoint + ", End: " + endPoint + ", Overlapping: " + isOverlapping);
        return isOverlapping;
    }


    // Moves the enemy in the specified direction
    IEnumerator MoveCoroutine(Vector2 direction)
    {
        // Normalize the direction vector to get a unit vector
        direction.Normalize();


        // Move the enemy towards the direction vector
        Vector2 targetPosition = rb.position + direction;
        while (Vector2.Distance(rb.position, targetPosition) > 0.01f) {
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
            yield return null;
        }

    }

    IEnumerator Idle()
    {
        isCoroutineRunning = true;
        //_trap.ShootTraps(rb.velocity.x);
        yield return new WaitForSeconds(waitTime);
        isCoroutineRunning = false;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Vector3 visionAngleA = Quaternion.AngleAxis(visionAngle, transform.forward) * currentDirection * visionRange;
        Vector3 visionAngleB = Quaternion.AngleAxis(-visionAngle, transform.forward) * currentDirection * visionRange;
        Gizmos.DrawLine(transform.position, transform.position + visionAngleA);
        Gizmos.DrawLine(transform.position, transform.position + visionAngleB);
    }
}
