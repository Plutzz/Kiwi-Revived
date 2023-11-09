using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2AI : MonoBehaviour
{
    [Header("Enemy AI States")]
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

    [Header("Enemy AI Components")]
    public EnemyShooting _trap;
    public EnemyMovement _enemyMovement;
    public float timeBetweenShots = 0.5f;
    public float waitTime = 3f;
    private float nextShotTime;
    private bool isCoroutineRunning = false;
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
                _enemyMovement.CheckCollision();
                _enemyMovement.CheckVision();
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
}
