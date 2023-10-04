using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyAIState _state = EnemyAIState.Patrol;

    public EnemyAIState state
    {
        get { return _state; }
        set { _state = value; }
    }
    public enum EnemyAIState
    {
        Patrol,
        Chase,
        Attack,
        Retreat
    }

    // GameObjects
    public EnemyShooting _trap;
    public EnemyMovement _enemyMovement;

    public bool isCoroutineRunning = false;
    public float waitTime = 3f;
    private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D attackCollider; // Attack collider is used to check if the player is within attack range
    [SerializeField] private CircleCollider2D escapeCollider; // Escape collider is used to check if the player is within minimum range (melee)
    [SerializeField] private LayerMask playerLayer;           // Layer mask for the player
    [SerializeField] private Transform playerTransform;       // Transform of the player
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _trap = GetComponent<EnemyShooting>();
    }

    void Update()
    {
        switch (_state)
        {
            case EnemyAIState.Patrol:
                if (!isCoroutineRunning) {
                    StartCoroutine(ShootTrap());
                }
                _enemyMovement.CheckCollision();
                _enemyMovement.Move();
                _enemyMovement.CheckVision();
                
                break;
            // Move towards the last known position of the player when a ray cast (vision) hits the player
            case EnemyAIState.Chase:
                _enemyMovement.CheckVision();
                _enemyMovement.CheckCollision();
                _enemyMovement.Chase();
                break;
            // Shoot at the player, once it is within attack range, in sight, not behind a wall, and not in minimum range (melee)
            case EnemyAIState.Attack:
                DetectPlayer();
                break;
            // Run away from the player, once it is within minimum range (melee)
            case EnemyAIState.Retreat:
                break;

        }
    }

    // Trips over its own traps. Need to fix this
    IEnumerator ShootTrap()
    {
        isCoroutineRunning = true;
        // _trap.ShootTraps(rb.velocity.x);
        yield return new WaitForSeconds(waitTime);
        isCoroutineRunning = false;
    }

    IEnumerator ShootSpear()
    {
        isCoroutineRunning = true;
        _trap.ShootSpear(rb.velocity.x, _enemyMovement.playerTransform.position, _enemyMovement.visionRange);
        yield return new WaitForSeconds(waitTime);
        isCoroutineRunning = false;
    }
    
    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _state = EnemyAIState.Attack;
        }
    }

    void OnTriggerExit2D (Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
           _enemyMovement.CheckVision(); // Go to last known position
        }
    }
    void DetectPlayer()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, _enemyMovement.visionRange, playerLayer);
        Debug.DrawRay(transform.position, directionToPlayer * _enemyMovement.visionRange, Color.green);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player detected");
            }
        }
    }


    

    // Death once health system is implemented
    void Death()
    {
        Destroy(gameObject);
    }


}
