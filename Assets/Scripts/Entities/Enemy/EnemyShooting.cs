using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    public GameObject trapPrefab;
    public GameObject spearPrefab;
    public Rigidbody2D rb;
    public Transform trapSpawnPoint;
    public Transform spearSpawnPoint;
    public Vector3 spearSpawnPointPosition;
    public Vector3 _directionToPlayer;
    public float _visionRange;
    public CircleCollider2D attackRangeCollider;
    public RaycastHit2D[] hits;


    void Update()
    {

    }

    public void ShootSpear(float enemyVelocity, Vector3 directionToPlayer, float visionRange) {
        _directionToPlayer = directionToPlayer;
        _visionRange = attackRangeCollider.radius * transform.localScale.x;
        
        Vector3 start = transform.position + new Vector3(0, 1f, 0);
        Vector3 end = start + directionToPlayer.normalized * _visionRange;

        hits = Physics2D.LinecastAll(start, end);

        bool playerInSight = false;
        RaycastHit2D playerHit = new RaycastHit2D();
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.CompareTag("Player")) {
                playerInSight = true;
                playerHit = hit;
                break;
            }
        }

        if (playerInSight) {
                Vector3 directionToHit = playerHit.point - ((Vector2) transform.position + new Vector2(0, 1f));

                GameObject spear = Instantiate(spearPrefab, spearSpawnPointPosition, Quaternion.LookRotation(Vector3.forward, directionToPlayer));
                Rigidbody2D spearPrefab_rb = spear.GetComponent<Rigidbody2D>();
                spearPrefab_rb.AddForce(directionToPlayer.normalized * 50, ForceMode2D.Impulse);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Vector3 start = transform.position + new Vector3(0, 1f, 0);
        Vector3 end = start + _directionToPlayer.normalized * _visionRange;
        Gizmos.DrawLine(start, end);
        
        Gizmos.color = Color.red;
        if (hits != null)
            foreach (RaycastHit2D hit in hits) {
                Gizmos.DrawRay(transform.position + new Vector3(0, 1f, 0), hit.centroid);
            }
        
    }

    // Initial velocity from the enemy and addition to the velocity of the trap when it is thrown
    // This is to make the trap move faster than the enemy

    // Two options:
    // First Option: When you spawn the trap take the x velocity of the enemy and see whether it is pos or neg.
    // Pass that value to the trap function and make that trap's velocity the same as the enemy's

    // Second Option: On the trap, grab enemy velocity from the start function and add it to the velocity of the trap
    public void ShootTraps(float enemyVelocity)
    {
        GameObject trap = Instantiate(trapPrefab, this.transform.position + new Vector3(0, 5, 0), transform.rotation);
        Rigidbody2D trapPrefab_rb = trap.GetComponent<Rigidbody2D>();
        trapPrefab_rb.AddForce(transform.up * 100, ForceMode2D.Impulse);
        // Random Direction (Left or Right) of the trap
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, 0);
        trapPrefab_rb.AddForce(direction * 250, ForceMode2D.Impulse);
        trapPrefab_rb.AddTorque(30, ForceMode2D.Impulse);

    }
}
