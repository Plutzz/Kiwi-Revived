using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{

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
}
