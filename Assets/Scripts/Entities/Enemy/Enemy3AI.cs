using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3AI : MonoBehaviour
{
    float scalex = 1f;
    float cooldownTimer = 0f;
    float attackCooldown = 0.5f;
    public float velocityDecrease = 1.0f;
    public GameObject rangeCheck;
    public GameObject projectileSpawner;
    public GameObject parent;
    public GameObject explosion;

    // Update is called once per frame
    void Update()
    {
        
        cooldownTimer += Time.deltaTime;
        if (rangeCheck.GetComponent<Enemy3CheckRange>().isPlayerInRange())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                projectileSpawner.GetComponent<Enemy3ProjectileSpawner>().shoot();
                
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WaterBullet")
        {
            this.transform.localScale = new Vector3(scalex, scalex, 1);
            scalex += 0.1f;
            // enemy dies when scale reaches a certain amount
            if (this.transform.localScale.x >= 3)
            {
                Instantiate(explosion, this.transform.position, this.transform.rotation);
                Destroy(parent);

            }
            else if (this.transform.localScale.x >= 2.5)
            {
                velocityDecrease = 0.3f;
                attackCooldown = 2f;
            }
            else if (this.transform.localScale.x >= 2)
            {
                velocityDecrease = 0.5f;
                attackCooldown = 1f;
            }
            else if (this.transform.localScale.x >= 1.5)
            {
                velocityDecrease = 0.7f;
                attackCooldown = 0.7f;
            }

        }
    }
}
