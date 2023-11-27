using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : DamageableEntity
{
    public int maxHp = 100;
    [SerializeField] private Slider hpBar;
    private CinemachineImpulseSource impulseSource;
    public static PlayerHealth Instance;

    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private int invincibilityBlinks = 3;
    [SerializeField] private GameObject graphics;

    private bool canTakeDamage;

    [Header("Hit Effects")]
    [SerializeField] private float hitStopTimeScale = 0.05f;    // How slow the game slows down to when hit
    [SerializeField] private int hitStopRestoreSpeed = 10; // Speed at which time scale is restored
    [SerializeField] private float hitStopDelay = 0.1f;        // How much time before time begins to restore to normal
    [SerializeField] private ParticleSystem impactEffect;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject deathHandler;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private VolumeProfile globalVolume;
    private ChromaticAberration chromaticEffect;
    [SerializeField] private float maxChromaticIntensity;
    

    [Header("Knockback")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float knockbackCounter;
    [SerializeField] private float knockbackTotalTime;


    private bool knockedFromRight;

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
        currentHp = maxHp;
        playerMovement = PlayerMovement.Instance;


        globalVolume.TryGet(out chromaticEffect);
        
    }

    private void Update()
    {
        // chromatic abberation effect
        if (chromaticEffect.intensity.value > 0)
        {
            chromaticEffect.intensity.value -= Time.deltaTime * 2;
        }
        else
        {
            chromaticEffect.intensity.value = 0;
        }

        // Hit time stop
        if (restoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                playerMovement.enabled = true;
                Time.timeScale = 1f;
                restoreTime = false;
                anim.SetBool("Damaged", false);
            }
        }

        knockbackCounter -= Time.deltaTime;
    }

    public void takeDamage (int damage, Transform hitSource)
    {
        if (!canTakeDamage) return;

        knockedFromRight = hitSource.position.x > rb.position.x;
        Debug.Log(knockedFromRight);
        Debug.Log("hitSource x: " + hitSource.position.x);
        Debug.Log("player x: " + rb.position.x);

        canTakeDamage = false;
        StartCoroutine(Invincibility());

        currentHp -= damage;
        impactEffect.Play(true);
        CameraShakeManager.Instance.CameraShake(impulseSource);
        AudioManager.Instance.PlaySound(AudioManager.Sounds.PlayerDamaged);
        if (currentHp > 0)
        {
            ApplyKnockback();
            StopTime(hitStopTimeScale, hitStopRestoreSpeed, hitStopDelay);
        }

        if (currentHp <= 0)
        {
            OnDeath();
        }
        
        float fillvalue = (float)currentHp/(float)maxHp;
        hpBar.value = fillvalue;
    }

    public override void takeDamage(int damage)
    {
        takeDamage(damage, rb.transform);
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
            playerMovement.enabled = false;
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            restoreTime = true;
        }

        Time.timeScale = changeTimeScale;
        chromaticEffect.intensity.value = maxChromaticIntensity;
        anim.SetBool("Damaged", true);
    }

    private IEnumerator StartTimeAgain(float amt)
    {
        yield return new WaitForSecondsRealtime(amt);
        restoreTime = true;
    }

    protected override void OnDeath()
    {
        Instantiate(deathHandler, transform.position, transform.rotation);
        Destroy(graphics);
        rb.bodyType = RigidbodyType2D.Static;
        Timer.Instance.CallLoadStartScene();
        //Timer.Instance.LoadStartScene();
        Destroy(rb.gameObject.GetComponent<Collider2D>());
        Destroy(this.gameObject);
    }

    private void ApplyKnockback()
    {
        knockbackCounter = knockbackTotalTime;

        if(knockedFromRight) // hit from right
        {
            rb.velocity = new Vector2(-knockbackForce, knockbackForce);
        }
        else
        {
            rb.velocity = new Vector2(knockbackForce, knockbackForce); 
        }
    }

    public bool KnockbackEnabled()
    {
        return knockbackCounter > 0;
    }
}