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
    public float timeBetweenShots = 0.5f;
    private float nextShotTime;

    public bool isCoroutineRunning = false;
    public float waitTime = 3f;
    private Rigidbody2D rb;
    
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
                if (!isCoroutineRunning) {
                    StartCoroutine(ShootTrap());
                }
                ShootSpear();
                _enemyMovement.CheckVision();
                _enemyMovement.CheckCollision();
                break;
            // Run away from the player, once it is within minimum range (melee)
            case EnemyAIState.Retreat:
                _enemyMovement.CheckVision();
                _enemyMovement.CheckCollision();
                break;

        }
    }

    // Trips over its own traps. Need to fix this
    IEnumerator ShootTrap()
    {
        isCoroutineRunning = true;
        _trap.ShootTraps(rb.velocity.x);
        yield return new WaitForSeconds(waitTime);
        isCoroutineRunning = false;
    }

    void ShootSpear()
    {
        if (nextShotTime >= timeBetweenShots)
        {
            _trap.ShootSpear(rb.velocity.x);
            nextShotTime = 0;
        }
        nextShotTime += Time.deltaTime;
    }
    
    

    // Death once health system is implemented
    void Death()
    {
        Destroy(gameObject);
    }


}
