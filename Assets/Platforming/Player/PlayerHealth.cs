using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerHealth : DamageableEntity
{
    public int maxHp = 100;
    public Image hpSliderFill;
    public Slider hpBar;
    private CinemachineImpulseSource impulseSource;
    public static PlayerHealth Instance;

    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private int invincibilityBlinks = 3;
    [SerializeField] private GameObject graphics;

    private bool canTakeDamage;

    [SerializeField] private float hitStopTimeScale = 0.05f;    // How slow the game slows down to when hit
    [SerializeField] private int hitStopRestoreSpeed = 10; // Speed at which time scale is restored
    [SerializeField] private float hitStopDelay = 0.1f;        // How much time before time begins to restore to normal

    [SerializeField] private ParticleSystem impactEffect;
    [SerializeField] private Animator anim;

    private float speed;
    private bool restoreTime;
    void Awake ()
    {
        Instance = this;
        canTakeDamage = true;
    }

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        GetComponent<Slider>();
        currentHp = maxHp;
    }

    private void Update()
    {
        if(restoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
                anim.SetBool("Damaged", false);
            }
        }
    }

    public override void takeDamage (int damage)
    {
        if (!canTakeDamage) return;

        canTakeDamage = false;
        StartCoroutine(Invincibility());

        if(currentHp > 0)
        {
            currentHp -= damage;
            impactEffect.Play(true);
            CameraShakeManager.Instance.CameraShake(impulseSource);
            AudioManager.Instance.PlaySound(AudioManager.Sounds.PlayerDamaged);
            StopTime(hitStopTimeScale, hitStopRestoreSpeed, hitStopDelay);
        }
        
        float fillvalue = (float)currentHp/(float)maxHp;

        // Debug.Log(fillvalue);

        hpBar.value = fillvalue;
    }

    private IEnumerator Invincibility()
    {
        for (int i = 0; i < invincibilityBlinks; i++)
        {
            yield return new WaitForSeconds(invincibilityTime / (invincibilityBlinks * 2));
            graphics.SetActive(false);
            yield return new WaitForSeconds(invincibilityTime / (invincibilityBlinks * 2));
            graphics.SetActive(true);
        }

        canTakeDamage = true;
    }

    private void StopTime(float changeTimeScale, int restoreSpeed, float delay)
    {
        speed = restoreSpeed;

        if(delay > 0)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            restoreTime = true;
            
        }

        Time.timeScale = changeTimeScale;
        anim.SetBool("Damaged", true);
    }

    private IEnumerator StartTimeAgain(float amt)
    {
        yield return new WaitForSecondsRealtime(amt);
        restoreTime = true;
    }

}