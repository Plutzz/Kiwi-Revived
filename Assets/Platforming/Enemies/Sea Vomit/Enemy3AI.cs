using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3AI : MonoBehaviour
{
    public int maxSize = 3;
    float cooldownTimer = 0f;
    float attackCooldown = 0.5f;
    public float velocityDecrease = 1.0f;
    public GameObject rangeCheck;
    public GameObject projectileSpawner;
    private EnemyHealth enemyHealth;



    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }
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
            int health = enemyHealth.currentHp;
            
            // Calculate scale factor based on health
            float scaleFactor = 1 + (1 - (float)health / enemyHealth.maxHp) * 2;
            scaleFactor = Mathf.Min(scaleFactor, maxSize);
            this.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            float healthFactor = (float)health / enemyHealth.maxHp;
            // enemy dies when scale reaches a certain amount
            if (healthFactor <= 0.25)
            {
                velocityDecrease = 0.3f;
                attackCooldown = 2f;
            }
            else if (healthFactor <= 0.50)
            {
                velocityDecrease = 0.5f;
                attackCooldown = 1f;
            }
            else if (healthFactor <= 0.75)
            {
                velocityDecrease = 0.7f;
                attackCooldown = 0.7f;
            }


        }
    }
}
