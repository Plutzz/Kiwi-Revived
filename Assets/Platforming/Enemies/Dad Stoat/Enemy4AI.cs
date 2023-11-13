using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4AI : MonoBehaviour
{

    public KainState currentState = KainState.Idle;
    public GameObject tailEnd;
    public GameObject projectile;
    public GameObject Graphics;
    public float shootCooldown = 2f;
    public float meleeCooldown = 1f;
    public bool canShoot = true;
    public bool canMelee = true;
    public Vector3 shadowOffset;
    public Animator animator;
    public bool neutral = true;
    public bool charging = false;


    [SerializeField] private Transform player;
    [SerializeField] private Transform kain;
    [SerializeField] private float shadowSpeed;
    [SerializeField] private float shadowFollowRadius;
    [SerializeField] private float shadowRetreatRadius;
    [SerializeField] private Tweener tweener;
    [SerializeField] private float distanceToStartShooting = 10f;
    private int test;
    private float distance = 999f;
    //private bool stanceChanged = false;

    public enum KainState {
        Idle,
        Melee,
        Range
    }

    private void Start() {
        player = PlayerMovement.Instance.transform;
    }

    void Update()
    {
        
        distance = player.position.x - transform.position.x;

        if(!charging)
        {
            if(distance > 0)
            {
                Graphics.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(distance < 0)
            {
                Graphics.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if(neutral)
        {
            neutral = false;

            int newState = 1; //Random.Range(0, 1);

            switch (newState)
            {

                case (0):
                    {
                        {
                            ChangeState(KainState.Melee);
                            animator.SetBool("isMelee", true);
                            MeleeStance();
                        }
                        break;
                    }

                case (1):
                    {
                        {
                            ChangeState(KainState.Range);
                            animator.SetBool("isRanged", true);
                            ShootStance();
                        }
                        break;
                    }
            }
        }
    }

    private void ChangeState(KainState _state)
    {
        currentState = _state;
        animator.SetBool("isMelee", false);
        animator.SetBool("isRanged", false);
        animator.SetBool("isIdle", false);
    }

    void MeleeStance()
    {
        int meleePattern = Random.Range(1, 2);
        // switch (meleePattern)
        // {
        //     case (1):
        //     {
        //         StartCoroutine(Charge);
        //         break;
        //     }

        //     case (2):
        //     {
        //         StartCoroutine(Sweep);
        //         break;
        //     }
        // }

    }

    void ShootStance()
    {
        int shootingPattern = 1; //Random.Range(1, 3);
        switch (shootingPattern)
        {
            case (1):
            {
                FireBolt.bulletState = FireBolt.AttackPattern.Volley;
                StartCoroutine(Volley());
                break;
            }

            // case (2):
            // {
            //     FireBolt.bulletState = FireBolt.AttackPattern.Volley;
            //     StartCoroutin(Homing())
            //     break;
            // }

            // case (3):
            // {
            //     FireBolt.bulletState = FireBolt.AttackPattern.Rain;
            //     StartCoroutine(Rain())
            //     break;
            // }
        }
    }

    private IEnumerator Volley()
    {
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        neutral = true;
    }

    private IEnumerator Homing()
    {
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        neutral = true;
    }

    private IEnumerator Rain()
    {
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, transform.rotation);
        yield return new WaitForSeconds(shootCooldown);
        Instantiate(projectile, tailEnd.transform.position, transform.rotation);
        neutral = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, distanceToStartShooting);
        //Gizmos.DrawWireSphere(this.transform.position, distanceToIdle);
    }

}
