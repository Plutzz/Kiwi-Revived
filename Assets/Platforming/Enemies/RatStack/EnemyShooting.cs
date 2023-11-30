using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float force = 50f;
    public GameObject trapPrefab;
    public GameObject spearPrefab;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ShootSpear(float enemyVelocity) {
        GameObject spear = Instantiate(spearPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.LookRotation(Vector3.forward, player.position - transform.position));
        Rigidbody2D spearPrefab_rb = spear.GetComponent<Rigidbody2D>();
        Vector2 modifiedVelocity = spearPrefab_rb.velocity;
        modifiedVelocity.x += enemyVelocity * 1.1f;
        spearPrefab_rb.velocity = modifiedVelocity;
        spearPrefab_rb.AddForce((player.position - transform.position).normalized * force, ForceMode2D.Impulse);

        AudioManager.Instance.PlaySound(AudioManager.Sounds.SpearShot);
    }


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
