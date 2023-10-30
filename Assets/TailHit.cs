using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailHit : MonoBehaviour
{
    public int damage = 10;
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().takeDamage(damage);
        }
    }
}
