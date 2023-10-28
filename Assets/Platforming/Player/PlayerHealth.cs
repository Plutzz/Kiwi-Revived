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

    void Awake ()
    {
        Instance = this;
    }

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        GetComponent<Slider>();
        currentHp = maxHp;
    }

    public override void takeDamage (int damage)
    {
        if(currentHp > 0)
        {
            currentHp -= damage;
            ParticleSystem ps = GameObject.Find("HitParticles").GetComponent<ParticleSystem>();
            ps.Play(true);
            CameraShakeManager.Instance.CameraShake(impulseSource);
            AudioManager.Instance.PlaySound(AudioManager.Sounds.PlayerDamaged);
        }
        
        float fillvalue = (float)currentHp/(float)maxHp;

        // Debug.Log(fillvalue);

        hpBar.value = fillvalue;
    }

}