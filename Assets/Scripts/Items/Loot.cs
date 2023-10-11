using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public float magnetSpeed = 5f;
    public float magnetDistance = 2f;
    private Transform player;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        if (player != null) {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= magnetDistance) {
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
