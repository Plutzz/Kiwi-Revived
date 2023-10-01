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

    // Start is called before the first frame update
    void Start()
    {
        
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
            this.transform.localScale = new Vector3(scalex, scalex, 1);
            scalex += 0.1f;
            // enemy dies when scale reaches a certain amount
            if (this.transform.localScale.x >= 3)
            {
                gameObject.SetActive(false);

            }
            else if (this.transform.localScale.x >= 2)
            {
                velocityDecrease = 0.5f;
            }
           
        }
    }
}
