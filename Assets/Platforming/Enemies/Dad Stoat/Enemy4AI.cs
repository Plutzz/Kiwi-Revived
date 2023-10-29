using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4AI : MonoBehaviour
{
    public float heightWhenOnTail = 4f;
    public KainState currentState = KainState.Idle;
    public GameObject tailWhip;
    public GameObject tailEnd;
    public GameObject projectile;
    public GameObject Graphics;
    public float shootCooldown = 0.8f;
    public float meleeCooldown = 1f;
    public bool canShoot = true;
    public bool canMelee = true;
    public Vector3 shadowOffset;


    [SerializeField] private Transform player;
    [SerializeField] private Transform kain;
    [SerializeField] private float shadowSpeed;
    [SerializeField] private float shadowFollowRadius;
    [SerializeField] private float shadowRetreatRadius;
    [SerializeField] private Tweener tweener;
    private float distance = 999f;
    private bool onTail = false;
    private bool stanceChanged = false;

    public enum KainState {
        Idle,
        Chase,
        Melee,
        Range
    }

    private void Start() {
        //player = PlayerMovement.Instance.transform;
        ChangeState(KainState.Melee);
    }

    void Update()
    {

        distance = player.position.x - transform.position.x;

        if(distance > 0)
        {
            Graphics.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(distance < 0)
        {
            Graphics.transform.localScale = new Vector3(1, 1, 1);
        }


        switch ((int)currentState)
        {
            case (0):
                {
                    // Idle
                    break;
                }

            case (1):
                {
                    // Chase
                    break;
                }

            case (2):
                {
                    if(stanceChanged == false)
                    {
                        MeleeStance();
                        stanceChanged = true;
                    }
                    break;
                }

            case (3):
                {
                    if (stanceChanged == false)
                    {
                        ShootStance();
                        stanceChanged = true;
                    }
                    break;
                }
        }
    }

    private void ChangeState(KainState _state)
    {
        currentState = _state;
        stanceChanged = false;
    }

    void MeleeStance()
    {
        if(onTail)
        {
            transform.position -= new Vector3(0, heightWhenOnTail, 0);
            onTail = false;
        }
        else
        {
            ShadowPlayer();
        }
        // if (Cooldown is ready attack)
        StartCoroutine(Melee());
    }

    void ShootStance()
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
            Instantiate(tailWhip, transform.position, transform.rotation);
            canMelee = false;
            yield return new WaitForSeconds(meleeCooldown);
            canMelee = true;
        }
    }

    private void ShadowPlayer()
    {
        //transform.position = player.transform.position + shadowOffset;
        tweener = kain.DOMoveX(player.position.x, shadowSpeed).SetSpeedBased(true);
        tweener.OnUpdate(delegate () {

            if (currentState != KainState.Melee) tweener.Kill();

            float _distance = Vector3.Distance(player.position, transform.position);

            if (_distance > shadowFollowRadius)
            {
                tweener.ChangeEndValue(player.transform.position, shadowSpeed, true);
            }
            else if(_distance < shadowRetreatRadius)
            {
                tweener.ChangeEndValue(player.position + new Vector3(shadowFollowRadius * 2 * transform.localScale.x, 0, 0), shadowSpeed, true);
            }
            else
            {
                Debug.Log("Stop distance");
                tweener.ChangeEndValue(player.position, 0.00000001f, true);
            }
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, shadowFollowRadius);
        Gizmos.DrawWireSphere(this.transform.position, shadowRetreatRadius);
    }


}
