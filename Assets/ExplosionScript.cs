using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ExplosionScript : MonoBehaviour
{
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        Destroy(gameObject, animTime + delay);
    }

    
}
