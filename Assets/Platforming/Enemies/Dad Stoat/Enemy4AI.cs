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
    [Header("Ranged Attributes")]
    public int howManyRangedShots = 5;
    public float shootCooldown = 2f;
    [Header("Melee Attributes")]
    public float meleeCooldown = 1f;
    public float meleeRange = 10f;
    public float chargeSpeed = 20f;
    public bool charging = false;
    public bool right = true;
    [Header("General Attributes")]
    public float secondsBeforeNextAttack = 2f;
    public Vector3 shadowOffset;
    public Animator animator;
    [SerializeField] private Transform player;
    [SerializeField] private Transform kain;
    [SerializeField] private float shadowSpeed;
    [SerializeField] private float shadowFollowRadius;
    [SerializeField] private float shadowRetreatRadius;
    [SerializeField] private Tweener tweener;
    [SerializeField] private float distanceToStartShooting = 10f;
    private bool neutral = true;
    private float distance = 999f;
    private int newState;
    //private bool stanceChanged = false;

    public enum KainState {
        Idle,
        Charge,
        Melee,
        Range
    }

    private void Start() {
        player = PlayerMovement.Instance.transform;
        newState = Random.Range(0, 2);
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

            switch (newState)
            {
                //melee
                case (0):
                {
                    {
                        if(Mathf.Abs(distance) > meleeRange && !charging)
                        {
                            Debug.Log("Charging");
                            if(distance > 0)
                            {
                                right = true;
                            } else {
                                right = false;
                            }

                            charging = true;
                        }
                        
                        if(charging)
                        {
                            ChangeState(KainState.Charge);
                            animator.SetBool("isMelee", true);
                            //animator.SetBool("isCharging", true);
                            Charge();
                        } else {
                            Debug.Log("Sweeping");
                            ChangeState(KainState.Melee);
                            animator.SetBool("isMelee", true);
                            MeleeStance();
                        }
                    }
                    break;
                }

                //range
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

    void MeleeStance()
    {
        int meleePattern = 1;

        // if(distance > meleeRange)
        // {
        //     meleePattern = 2;
        // }

        switch (meleePattern)
        {
            case (1):
            {
                StartCoroutine(Sweep());
                break;
            }
        }

    }

    void ShootStance()
    {
        int shootingPattern = Random.Range(1, 4);
        switch (shootingPattern)
        {
            case (1):
            {
                Debug.Log("Volley");
                StartCoroutine(Volley());
                break;
            }

            case (2):
            {
                Debug.Log("Homing");
                StartCoroutine(Homing());
                break;
            }

            case (3):
            {
                Debug.Log("Rain");
                StartCoroutine(Rain());
                break;
            }
        }
    }

    private void Charge()
    {
        if(charging)
        {
            if(right)
            {
                kain.position += transform.right * chargeSpeed * Time.deltaTime;
            } else {
                kain.position -= transform.right * chargeSpeed * Time.deltaTime;
            }
        }

        neutral = true;
    }

    private IEnumerator Sweep()
    {
        //animation for sweep attack
        
        yield return new WaitForSeconds(secondsBeforeNextAttack);

        newState = Random.Range(0, 2);
        neutral = true;
    }

    private IEnumerator Volley()
    {
        FireBolt.bulletState = FireBolt.AttackPattern.Volley;

        for(int i = 0; i < howManyRangedShots; i++)
        {
            yield return new WaitForSeconds(shootCooldown);
            Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        }
        
        yield return new WaitForSeconds(secondsBeforeNextAttack);
        
        newState = Random.Range(0, 2);
        neutral = true;
    }

    private IEnumerator Homing()
    {
        FireBolt.bulletState = FireBolt.AttackPattern.Homing;

        for(int i = 0; i < howManyRangedShots; i++)
        {
            yield return new WaitForSeconds(shootCooldown);
            Instantiate(projectile, tailEnd.transform.position, tailEnd.transform.rotation);
        }

        yield return new WaitForSeconds(secondsBeforeNextAttack);

        newState = Random.Range(0, 2);
        neutral = true;
    }

    private IEnumerator Rain()
    {
        FireBolt.bulletState = FireBolt.AttackPattern.Rain;

        for(int i = 0; i < howManyRangedShots; i++)
        {
            yield return new WaitForSeconds(shootCooldown);
            Instantiate(projectile, tailEnd.transform.position, transform.rotation);
        }

        yield return new WaitForSeconds(1f);

        FireBolt.bulletState = FireBolt.AttackPattern.RainFall;

        for(int i = 0; i < howManyRangedShots; i++)
        {
            yield return new WaitForSeconds(shootCooldown);
            Instantiate(projectile, new Vector3(player.position.x + Random.Range(-10f, 10f), player.position.y + 20, 0), transform.rotation);
        }

        yield return new WaitForSeconds(secondsBeforeNextAttack);

        newState = Random.Range(0, 2);
        neutral = true;
    }

    private void ChangeState(KainState _state)
    {
        currentState = _state;
        //animator.SetBool("isCharging", false);
        animator.SetBool("isMelee", false);
        animator.SetBool("isRanged", false);
        animator.SetBool("isIdle", false);
    }

    public void EndCharge()
    {
        charging = false;
        newState = Random.Range(0, 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, distanceToStartShooting);
        //Gizmos.DrawWireSphere(this.transform.position, distanceToIdle);
    }
}
