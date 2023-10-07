using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public int damage = 10;
    private PolygonCollider2D tipCollider;
    private Collider2D spearCollider;
    private Rigidbody2D spear_rb;
    void Start()
    {
        spear_rb = GetComponent<Rigidbody2D>();
        tipCollider = GetComponentInChildren<PolygonCollider2D>();
        spearCollider = GetComponent<Collider2D>();
    }
    void Update()
    {
        if (tipCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            spear_rb.constraints = RigidbodyConstraints2D.FreezeAll;
            spearCollider.enabled = false; 
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //collider.gameObject.GetComponent<PlayerHealth>().takeDamage(damage);
            spear_rb.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.parent = collider.transform;
            spear_rb.isKinematic = true;
            spearCollider.enabled = false;
        }
    }
}
