using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4AI : MonoBehaviour
{
    public float heightWhenOnTail = 4f;
    public Enemy3AIState currentState = Enemy3AIState.Idle;
    public GameObject tail;
    public GameObject tailEnd;
    public GameObject projectile;
    public float shootCooldown = 0.8f;
    public float meleeCooldown = 1f;
    public bool canShoot = true;
    public bool canMelee = true;


    private Transform player;
    private float distance = 999f;
    private bool onTail = false;

    public enum Enemy3AIState {
        Idle,
        Chase,
        Melee,
        Range
    }

    private void Start() {
        player = PlayerMovement.Instance.transform;
    }

    void Update()
    {

        distance = player.position.x - transform.position.x;

        switch((int)currentState)
        {
            case(0):
            {
                // Patrol();
                break;
            }

            case(1):
            {
                // FollowPlayer();
                break;
            }

            case(2):
            {
                MeleeStance();
                break;
            }

            case(3):
            {
                ShootStance();
                break;
            }
        }
    }

    void MeleeStance ()
    {
        if(onTail)
        {
            transform.position -= new Vector3(0, heightWhenOnTail, 0);
            onTail = false;
        }

        StartCoroutine(Melee());
    }

    void ShootStance ()
    {
        

        if(!onTail)
        {
            transform.position += new Vector3(0, heightWhenOnTail, 0);
            onTail = true;
        }

        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        if(canShoot)
        {
            canShoot = false;
            yield return new WaitForSeconds(shootCooldown);
            Instantiate(projectile, tailEnd.transform.position, transform.rotation);
            canShoot = true;
        }
    }

    private IEnumerator Melee()
    {
        if(canMelee)
        {
            Instantiate(tail, transform.position, transform.rotation);
            canMelee = false;
            yield return new WaitForSeconds(meleeCooldown);
            canMelee = true;
        }
    }

}
