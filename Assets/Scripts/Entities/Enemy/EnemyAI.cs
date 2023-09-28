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

    Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    public float speed = 10f;
    public float waitTime = 3f;
    public bool isCoroutineRunning = false;
    public bool isMoving = false;

    public Rigidbody2D rb;
    public Collider2D playerDetector;
    public EnemyAIState state = EnemyAIState.Patrol;
    public GameObject trapPrefab;

    void Start()
    {

    }
    void Update()
    {
        switch (state)
        {
            case EnemyAIState.Patrol:
                if (!isCoroutineRunning && !isMoving) {
                    StartCoroutine(Idle());
                }
                
                if (!isMoving) {
                    StartCoroutine(MoveCoroutine(directions[Random.Range(0, directions.Length)]));
                }

                break;
            // Move towards the last known position of the player
            case EnemyAIState.Chase:
                StopCoroutine(Idle());
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
    
    // Moves the enemy in the specified direction
    IEnumerator MoveCoroutine(Vector2 direction)
    {
        // Normalize the direction vector to get a unit vector
        direction.Normalize();

        // Set the isMoving flag to true
        isMoving = true;

        // Move the enemy towards the direction vector
        Vector2 targetPosition = rb.position + direction;
        while (Vector2.Distance(rb.position, targetPosition) > 0.01f) {
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
            yield return null;
        }

        // Set the isMoving flag to false
        isMoving = false;
    }

    // Shoots traps every waitTime seconds
    IEnumerator Idle()
    {
        isCoroutineRunning = true;
        Debug.Log("Idle");
        // Shoot traps
        ShootTraps();

        yield return new WaitForSeconds(waitTime);
        isCoroutineRunning = false;
    }


    void ShootTraps()
    {
        GameObject trap = Instantiate(trapPrefab, this.transform.position + new Vector3(0, 5, 0), transform.rotation);
        trap.GetComponent<Rigidbody2D>().AddForce(transform.up * 100, ForceMode2D.Impulse);
        // Random Direction (Left or Right) of the trap
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, 0);
        // Make the trap move left or right
        trap.GetComponent<Rigidbody2D>().AddForce(direction * 250, ForceMode2D.Impulse);
        // Make the trap spin
        trap.GetComponent<Rigidbody2D>().AddTorque(30, ForceMode2D.Impulse);

    }

    

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         Debug.Log("Player Detected");
    //     }
    // }

    
}
