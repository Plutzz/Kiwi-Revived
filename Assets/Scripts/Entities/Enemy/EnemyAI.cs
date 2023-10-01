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
    public float moveTime = 2f;
    public bool isCoroutineRunning = false;

    // GameObjects
    Vector2[] directions = { Vector2.left, Vector2.right };
    public Collider2D playerDetector;
    public EnemyAIState state = EnemyAIState.Patrol;
    public GameObject trapPrefab;
    public Transform groundCheck;
    private Rigidbody2D rb;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask groundLayer;                 // Layer mask for the ground

    // Private Variables
    [SerializeField] private bool IsGrounded = false;
    [SerializeField] private EdgeCollider2D groundCheckLeft;
    [SerializeField] private EdgeCollider2D groundCheckRight;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private float timer = 0f;
    [SerializeField] private Vector2 currentDirection = Vector2.zero;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
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
                if (timer >= moveTime) {
                    currentDirection = directions[Random.Range(0, directions.Length)];
                    timer = 0f;
                }
                Move(currentDirection);
                timer += Time.deltaTime;

                break;
            // Move towards the last known position of the player
            case EnemyAIState.Chase:
                StopCoroutine(Idle());
                if (!isMoving) {
                    Vector2 direction = playerTransform.position - transform.position;
                    RotateTowards(direction);
                    StartCoroutine(MoveCoroutine(direction));
                }
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

    void Move(Vector2 direction)
    {
        if (IsGrounded) {
            direction.Normalize();
            rb.position += direction * speed * Time.deltaTime;
        } 
    }

    void CheckCollision()
    {
        // RaycastHit2D hit = Physics2D.Raycast(bottomCenter, Vector2.down, groundLayer);
        // IsGrounded = hit.collider != null;
    }

    // Optional: Shoot in the direction of the player
    void ShootTraps()
    {
        GameObject trap = Instantiate(trapPrefab, this.transform.position + new Vector3(0, 5, 0), transform.rotation);
        trap.GetComponent<Rigidbody2D>().AddForce(transform.up * 100, ForceMode2D.Impulse);
        // Random Direction (Left or Right) of the trap
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, 0);
        trap.GetComponent<Rigidbody2D>().AddForce(direction * 250, ForceMode2D.Impulse);
        trap.GetComponent<Rigidbody2D>().AddTorque(30, ForceMode2D.Impulse);

    }

    // Moves the enemy in the specified direction
    IEnumerator MoveCoroutine(Vector2 direction)
    {
        // Normalize the direction vector to get a unit vector
        direction.Normalize();

        isMoving = true;

        // Move the enemy towards the direction vector
        Vector2 targetPosition = rb.position + direction;
        while (Vector2.Distance(rb.position, targetPosition) > 0.01f) {
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }

    // Shoots traps every waitTime seconds
    IEnumerator Idle()
    {
        isCoroutineRunning = true;
        Debug.Log("Idle");
        // Shoot traps
        // ShootTraps();

        yield return new WaitForSeconds(waitTime);
        isCoroutineRunning = false;
    }



    

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         Debug.Log("Player Detected");
    //     }
    // }

    
}
