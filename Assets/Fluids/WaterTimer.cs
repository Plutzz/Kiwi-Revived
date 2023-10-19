using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaterTimer : PrefabTimer
{
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
        transform.DOScale(3f, 1f).SetEase(Ease.OutSine).OnComplete(() => { Destroy(gameObject); });
    }
}
