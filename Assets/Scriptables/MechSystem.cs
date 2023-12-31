using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MechSystem : SingletonPersistent<MechSystem>
{
    //General Stats
    public int Vision;
    public int Speed;
    public int Armor;
    public int Velocity;
    public int Damage;


    //Keeps Track of scriptableMechPart objects
    private ScriptableMechPart[] mechHeads;
    private ScriptableMechPart[] mechBodies;
    private ScriptableMechPart equippedHead;
    private ScriptableMechPart equippedBody;

    private void Start()
    {
        LoadResources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Loads all head and body parts into arrays
    private void LoadResources()
    {
        mechHeads = Resources.LoadAll<ScriptableMechPart>("MechParts/Head");
        mechBodies = Resources.LoadAll<ScriptableMechPart>("MechParts/Chasis");
    }

    // Unequips all head parts
    public void UnequipHeads()
    {
        foreach(var part in mechHeads)
        {
            part.Equipped = false;
        }
    }

    // Unequips all body parts
    public void UnequipBodies()
    {
        foreach (var part in mechBodies)
        {
            part.Equipped = false;
        }
    }

    // This is called whenever a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " +  scene.name);
        if (!scene.name.Equals("PlatformingMovement"))
        {
            return;
        }
        AddStats();
        UpdatePlayerVariables();
    }

    // Finds which parts are currently equipped and sets them to equipped varibles
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
    }

    public void AddStats()
    {
        ClearStats();
        getEquippedParts();

        if (equippedBody == null && equippedHead == null)
        {
            return;
        }


        Vision += equippedBody.BaseStats.Vision;
        Speed += equippedBody.BaseStats.Speed;
        Armor += equippedBody.BaseStats.Armor;

        Vision += equippedBody.BaseStats.Vision;
        Speed += equippedBody.BaseStats.Speed;
        Armor += equippedBody.BaseStats.Armor;

        Debug.Log(Vision);
        Debug.Log(Speed);
        Debug.Log(Armor);


    }

    private void ClearStats()
    {
        Vision = 0;
        Armor = 0;
        Speed = 0;
        Velocity = 0;
        Damage = 0;
    }

    private void UpdatePlayerVariables()
    {
        PlayerHealth.Instance.maxHp += Armor;
        PlayerMovement.Instance.SetMaxSpeed(PlayerMovement.Instance.GetMaxSpeed() + Speed);
    }

    



}
