using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    public GameObject trapPrefab;
    public GameObject spearPrefab;
    public Rigidbody2D rb;
    public Transform spawnPoint;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootSpear(float enemyVelocity, Vector3 directionToPlayer, float visionRange) {
        Debug.DrawRay(transform.position, directionToPlayer.normalized * visionRange, Color.green);
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
