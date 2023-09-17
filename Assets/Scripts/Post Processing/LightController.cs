using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [SerializeField] private Light2D playerLight;

    void Update()
    {
        playerLight.pointLightOuterRadius = Timer.instance.TimeLeft;
    }
}
