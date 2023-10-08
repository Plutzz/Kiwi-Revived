using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    // Variables
    [Header("MOVEMENT")]
    public float speed = 10f;
    private float runSpeed = 20;
    public float moveTime = 5f;
    public float visionRange = 40f;
    public float visionAngle = 45f;
    public float patrolDelay = 3f;

    [Header("GAMEOBJECTS")]
    public LayerMask groundLayer;                 // Layer mask for the ground
    private Transform playerTransform;
    private Rigidbody2D rb;
    [SerializeField] private EdgeCollider2D leftGroundCheck;
    [SerializeField] private EdgeCollider2D rightGroundCheck;
    [SerializeField] private CircleCollider2D attackCollider; // Attack collider is used to check if the player is within attack range
    [SerializeField] private CircleCollider2D escapeCollider; // Escape collider is used to check if the player is within minimum range (melee)

    // Private Variables
    Vector2[] directions = { Vector2.left, Vector2.right };
    private float timer = 0f;
    private Vector2 currentDirection;
    private Vector2 lastKnownPosition;
    private bool isLeftGrounded = false;
    private bool isRightGrounded = false;
    // States
    private EnemyAI enemy;


    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        currentDirection = directions[Random.Range(0, directions.Length)];
        enemy = GetComponent<EnemyAI>();
    }

    public void Chase()
    {
        rb.velocity = Vector2.zero; // Stop moving from patrol 
        if (isLeftGrounded && isRightGrounded)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, rb.position.y), runSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);
        }
    }

    public void Move()
    {
        if (timer >= moveTime || !isLeftGrounded || !isRightGrounded)
        {
            currentDirection = directions[Random.Range(0, directions.Length)];
            timer = 0f;
        }

        if (isLeftGrounded && isRightGrounded)
        {
            currentDirection.Normalize();
            rb.position += currentDirection * speed * Time.deltaTime;
        }
        else if (!isLeftGrounded)
        {
            currentDirection = Vector2.right;
            rb.position += currentDirection * speed * Time.deltaTime;
        }
        else if (!isRightGrounded)
        {
            currentDirection = Vector2.left;
            rb.position += currentDirection * speed * Time.deltaTime;
        }


        // Check if the direction that the enemy is facing is not moving into a wall
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, 1f, groundLayer);
        Debug.DrawRay(transform.position, currentDirection * 3f, Color.magenta);

        if (hit.collider != null) {
            currentDirection *= -1;
            timer = 0f;
        }


        timer += Time.deltaTime;
    }

    public void CheckVision()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        float angleToPlayer = Vector2.Angle(directionToPlayer, transform.right);
        float minimumDistance = escapeCollider.radius;
        float maximumDistance = attackCollider.radius;

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
        {
            if (enemy.state == EnemyAI.EnemyAIState.Attack || enemy.state == EnemyAI.EnemyAIState.Chase)
            {
                Vector2 newPosition = transform.position;
                // Move towards the last known position of the player if and only if the enemy is grounded
                if (isLeftGrounded && isRightGrounded)
                    newPosition = Vector2.MoveTowards(transform.position, new Vector2(lastKnownPosition.x, transform.position.y), speed * Time.deltaTime);
                
                rb.MovePosition(newPosition);

                if (newPosition == lastKnownPosition)
                    enemy.state = EnemyAI.EnemyAIState.Patrol;
                 else
                    StartCoroutine(GoBackToPatrol());
            } else
                enemy.state = EnemyAI.EnemyAIState.Patrol;
        } else if (distanceToPlayer <= visionRange && angleToPlayer <= visionAngle && enemy.state != EnemyAI.EnemyAIState.Attack)
        {
            enemy.state = EnemyAI.EnemyAIState.Chase;
            lastKnownPosition = playerTransform.position;
        } 

        if (distanceToPlayer < minimumDistance) {
            // Add panic shooting if it is on the edge and cant go anywhere?
            enemy.state = EnemyAI.EnemyAIState.Retreat;
            if (isLeftGrounded && isRightGrounded)
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, -speed * Time.deltaTime);
        } else if (distanceToPlayer <= maximumDistance && distanceToPlayer > minimumDistance) {
            enemy.state = EnemyAI.EnemyAIState.Attack;
        }

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
        return Physics2D.OverlapArea(startPoint, endPoint, groundLayer);
    }
    IEnumerator GoBackToPatrol()
    {
        yield return new WaitForSeconds(patrolDelay);
        enemy.state = EnemyAI.EnemyAIState.Patrol;
    }


    // Debug Tool: To show the vision range and vision angle of the enemy.
    // Bug: The vision angle is not working properly, but its whatever
    void OnDrawGizmosSelected()
    {

        float minimumDistance = escapeCollider.radius;
        float maximumDistance = attackCollider.radius;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimumDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maximumDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        
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
    // Scratched this idea because it was too buggy, and we are ging to use NavMesh later on
    // IEnumerator Jump(float force, Vector2 playerPosition)
    // {
    //     bool isBlocked = Physics2D.OverlapCircle(transform.position + Vector3.up * 1.5f, 0.1f, groundLayer);

    //     Vector2 direction = Vector2.zero;

    //     // Determine the direction to jump in based on the player's position
    //     if (playerPosition.y < rb.position.y && playerPosition.x < rb.position.x)
    //         direction = new Vector2(-1, -1);
    //     else if (playerPosition.y < rb.position.y && playerPosition.x > rb.position.x)
    //         direction = new Vector2(1, -1);
    //     else if (playerPosition.y > rb.position.y && playerPosition.x < rb.position.x)
    //         direction = new Vector2(-1, 1);
    //     else if (playerPosition.y > rb.position.y && playerPosition.x > rb.position.x)
    //         direction = new Vector2(1, 1);

    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 10f, groundLayer);
    //     Debug.DrawRay(transform.position, direction * 10f, Color.magenta);

    //     float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);
    //     Debug.Log("Distance to player: " + distanceToPlayer + ", Minimum distance: " + minimumDistance + ", Maximum distance: " + maximumDistance);
    //     // Check if there's nothing blocking the jump and if the enemy will land below the player after jumping
    //     // && distanceToPlayer > minimumDistance && distanceToPlayer <= maximumDistance
    //     if (!isBlocked && hit.collider == null &&
    //         hit.point.y < playerPosition.y )
    //     {
    //         float time = 0f;
    //         float maxTime = 0.5f;
    //         float maxForce = force;
    //         float minForce = force / 2f;

    //         while (isJumping)
    //         {
    //             yield return null;
    //         }

    //         isJumping = true;
    //         while (time < maxTime)
    //         {
    //             float t = time / maxTime;
    //             float currentForce = Mathf.Lerp(maxForce, minForce, t);
    //             rb.AddForce(direction * currentForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
    //             time += Time.fixedDeltaTime;
    //             yield return new WaitForFixedUpdate();
    //         }

    //     }
    // }
}
