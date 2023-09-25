using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class MechSystem : SingletonPersistent<MechSystem>
{
    public int Vision;
    public int Speed;
    public int Armor;
    public int Velocity;
    public int Damage;

    private ScriptableMechPart[] mechHeads;
    private ScriptableMechPart[] mechBodies;
    private ScriptableMechPart equippedHead;
    private ScriptableMechPart equippedBody;

    private void Start()
    {
        LoadResources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void LoadResources()
    {
        mechHeads = Resources.LoadAll<ScriptableMechPart>("MechParts/Head");
        mechBodies = Resources.LoadAll<ScriptableMechPart>("MechParts/Chasis");
    }

    public void UnequipHeads()
    {
        foreach(var part in mechHeads)
        {
            part.Equipped = false;
        }
    }

    public void UnequipBodies()
    {
        foreach (var part in mechBodies)
        {
            part.Equipped = false;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("PlatformingMovement"))
        {
            return;
        }

        getEquippedParts();
    }

    private void getEquippedParts()
    {
        foreach(var part in mechHeads)
        {
            if (part.Equipped == true)
            {
                equippedHead = part;
                break;
            }
        }

        foreach (var part in mechBodies)
        {
            if (part.Equipped == true)
            {
                equippedBody = part;
                break;
            }
        }

        Debug.Log("Equipped Parts: " +  equippedHead + equippedBody);
    }

    



}
