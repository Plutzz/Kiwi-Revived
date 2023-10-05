using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject trapPrefab;
    public GameObject spearPrefab;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ShootSpear(float enemyVelocity) {
        GameObject spear = Instantiate(spearPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, player.position - transform.position));
        Rigidbody2D spearPrefab_rb = spear.GetComponent<Rigidbody2D>();
        Vector2 modifiedVelocity = spearPrefab_rb.velocity;
        modifiedVelocity.x += enemyVelocity + 10f;
        spearPrefab_rb.velocity = modifiedVelocity;
        spearPrefab_rb.AddForce((player.position - transform.position).normalized * 100, ForceMode2D.Impulse);
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
