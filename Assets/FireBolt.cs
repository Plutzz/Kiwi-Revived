using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : MonoBehaviour
{
    public int damage = 5;
    public float velocity = 10.0f;


    void Start ()
    {

    }

    void Update ()
    {
        transform.position += transform.right * velocity * Time.deltaTime; 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().takeDamage(damage);
            Debug.Log("damagetaken");
        }
    }
}
