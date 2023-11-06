using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailHit : MonoBehaviour
{
    public int damage = 10;
    [SerializeField] private int playerScriptsChildNumber = 5;
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.GetChild(playerScriptsChildNumber).GetComponent<PlayerHealth>().takeDamage(damage);
        }
    }
}
