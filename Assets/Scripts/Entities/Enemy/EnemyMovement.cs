using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    // Variables
    public float speed = 10f;
    public float moveTime = 5f;
    public float visionRange = 40f;
    public float visionAngle = 45f;
    public float minimumDistance = 10f;
    public float maximumDistance = 20f;
    public bool isJumping = false;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    public Transform playerTransform;
    [SerializeField] private LayerMask groundLayer;                 // Layer mask for the ground
    [SerializeField] private EdgeCollider2D leftGroundCheck;
    [SerializeField] private EdgeCollider2D rightGroundCheck;

    [SerializeField] private CircleCollider2D attackCollider; // Attack collider is used to check if the player is within attack range
    [SerializeField] private CircleCollider2D escapeCollider; // Escape collider is used to check if the player is within minimum range (melee)

    // Private Variables
    Vector2[] directions = { Vector2.left, Vector2.right };
    public bool isLeftGrounded = false;
    public bool isRightGrounded = false;
    private float timer = 0f;
    [SerializeField] private Vector2 currentDirection;
    [SerializeField] private Vector2 lastKnownPosition;

    // States
    private EnemyAI enemy;
    

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentDirection = directions[Random.Range(0, directions.Length)];
        enemy = GetComponent<EnemyAI>();
    }

    public void Chase() {
        rb.velocity = Vector2.zero; // Stop moving from patrol        // // If the player is above the enemy, jump towards the player
        if (!isJumping) {
            Jump(100f, playerTransform.position);
            rb.position = Vector2.MoveTowards(rb.position, playerTransform.position, speed * Time.deltaTime);
        }

        if (isLeftGrounded && isRightGrounded) {
            isJumping = false;
        }
    }

    /*
        Jumps in the specified direction with the specified force towards the player
        @param direction: The direction to jump in
        @param force: The force to jump with
        @param playerPosition: The position of the player

        It will check if the distance between the enemy and the player is greater than the minimum distance and if it is less than the maximum distance
        because we don't want the enemy to jump over the player if it is too close (we can jump away from the player if it is too close)
        nor do we want the enemy to jump towards the player if it is too far away
    */ 
    void Jump(float force, Vector2 playerPosition) {
        Debug.Log("Jump");
        
        bool isBlocked = Physics2D.OverlapCircle(transform.position + Vector3.up * 1.5f, 0.1f, groundLayer);

        Vector2 direction = Vector2.zero;

        // Determine the direction to jump in based on the player's position
        if (playerPosition.y < rb.position.y && playerPosition.x < rb.position.x) {
            direction = new Vector2(-1, -2);
        } else if (playerPosition.y < rb.position.y && playerPosition.x > rb.position.x) {
            direction = new Vector2(1,-2);
        } else if (playerPosition.y > rb.position.y && playerPosition.x < rb.position.x) {
            direction = new Vector2(-1,2);
        } else if (playerPosition.y > rb.position.y && playerPosition.x > rb.position.x) {
            direction = new Vector2(1,2);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 10f, groundLayer);

        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);
        Debug.DrawRay(transform.position, direction * 10f, Color.magenta);
    if (distanceToPlayer > minimumDistance &&
        distanceToPlayer < maximumDistance &&
        !isBlocked && hit.collider == null &&
        hit.point.y < playerPosition.y) {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        isJumping = true;
    }

    if (isLeftGrounded && isRightGrounded) {
        isJumping = false;
    }
    }
    
    public void Move()
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

    public void CheckVision()
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
        
        // Suggestion: There should be a time delay before the enemy goes back to patrol.
        // This is to prevent the enemy from going back to patrol immediately after seeing the player.
        // When seeing the player it will be in the Chase state and the vision cone will be drawn towards the player.
        if (distanceToPlayer > visionRange || angleToPlayer > visionAngle)
            enemy.state = EnemyAI.EnemyAIState.Patrol;
        else if (distanceToPlayer <= visionRange && angleToPlayer <= visionAngle)
            enemy.state = EnemyAI.EnemyAIState.Chase;
        
        Debug.DrawRay(transform.position, directionToPlayer.normalized * visionRange, Color.green);
    }

    public void CheckCollision()
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.collider == attackCollider)
            {
                Debug.Log("Attack");
                enemy.state = EnemyAI.EnemyAIState.Attack;
            }
            else if (collision.collider == escapeCollider)
            {
                Debug.Log("Retreat");
                enemy.state = EnemyAI.EnemyAIState.Retreat;
            }
        }
    }

    // Debug Tool: To show the vision range and vision angle of the enemy.
    // Bug: The vision angle is not working properly, but its whatever
    void OnDrawGizmosSelected()
    {
        // JUMP



        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Vector3 visionAngleA = Quaternion.AngleAxis(visionAngle, transform.forward) * currentDirection * visionRange;
        Vector3 visionAngleB = Quaternion.AngleAxis(-visionAngle, transform.forward) * currentDirection * visionRange;
        Gizmos.DrawLine(transform.position, transform.position + visionAngleA);
        Gizmos.DrawLine(transform.position, transform.position + visionAngleB);
    }
}
