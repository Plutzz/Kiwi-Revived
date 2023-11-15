using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KainTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("KainProjectile"))
        {
            Enemy4AI kain = other.gameObject.GetComponent<Enemy4AI>();
            kain.EndCharge();
        }
    }
}
