using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void DamageHealth(float health)
    {
        slider.value -= health;
    }

    public void SetMaxHealth(float health)
    {
        slider.value = health;
    }
}
