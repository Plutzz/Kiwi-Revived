using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy3AI : MonoBehaviour
{
    public float maxSize = 2f;
    float cooldownTimer = 0f;
    float attackCooldown = 0.5f;
    public float velocityDecrease = 1.0f;
    public GameObject rangeCheck;
    public GameObject projectileSpawner;
    private EnemyHealth enemyHealth;
    [SerializeField] private GameObject[] stages;



    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        for(int i = 1; i < stages.Length; i++)
        {
            stages[i].SetActive(false);
        }
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (rangeCheck.GetComponent<Enemy3CheckRange>().isPlayerInRange()) {
            if (cooldownTimer >= attackCooldown) {
                cooldownTimer = 0;
                projectileSpawner.GetComponent<Enemy3ProjectileSpawner>().shoot();
            }
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WaterBullet"))
        {
            int health = enemyHealth.currentHp;

            // Calculate scale factor based on health

            int numStages = stages.Length;
            int currentStage = Mathf.Clamp(Mathf.FloorToInt((1 - (float)health / enemyHealth.maxHp) * numStages), 0, numStages - 1);

            for (int i = 0; i < numStages; i++)
            {
                bool isActive = i == currentStage;
                stages[i].SetActive(isActive);
                stages[i].GetComponent<BoxCollider2D>().enabled = isActive;

                float scaleFactor = 1f + ((1 - (float)health / enemyHealth.maxHp) * 0.8f);
                scaleFactor = Mathf.Min(scaleFactor, maxSize);

                if (isActive)
                    transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            }

            velocityDecrease = 1 - 0.2f * currentStage;
            attackCooldown = 1 - 0.1f * currentStage;

        }
    }
}
