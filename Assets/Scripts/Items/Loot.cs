using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public float magnetSpeed = 5f;
    public float magnetDistance = 2f;
    private Transform player;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        if (player != null) {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= magnetDistance) {
                rb.isKinematic = true;
                bc.isTrigger = true;
                transform.position = Vector2.MoveTowards(transform.position, player.position, magnetSpeed * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player")) {
            PlayerInventory.Instance.AddItem(gameObject.name);
            Destroy(gameObject);
        }
    }
}
