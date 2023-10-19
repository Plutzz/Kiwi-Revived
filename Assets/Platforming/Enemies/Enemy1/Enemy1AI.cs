using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1AI : MonoBehaviour
{
    public CircleCollider2D visionRange;
    public CircleCollider2D attackRange;
    private Rigidbody2D rb;
    private Transform player;
    public float speed = 5f;
    public float runSpeed = 10f;

    
    public enum State {
        Idle,
        Chase,
        Attack
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        
    }


}
