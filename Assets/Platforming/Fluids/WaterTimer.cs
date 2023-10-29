using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaterTimer : PrefabTimer
{
    [SerializeField] private Ease ease;

    private bool destroyCalled = false;
    protected override void Update()
    {
        lifeTimeTimer -= Time.deltaTime;
        if (lifeTimeTimer <= 0 && !destroyCalled)
        {
            DestroyPrefab();
            destroyCalled = true;
        }
    }
    protected override void DestroyPrefab()
    {
       transform.DOScale(.5f, 1f).SetEase(ease).OnComplete(() => { Destroy(gameObject); });
    }
}
