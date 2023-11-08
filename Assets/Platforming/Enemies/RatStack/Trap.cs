using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound(AudioManager.Sounds.Trap);
            PlayerHealth.Instance.takeDamage(damage, transform);
            Destroy(gameObject);
        }
    }
}
