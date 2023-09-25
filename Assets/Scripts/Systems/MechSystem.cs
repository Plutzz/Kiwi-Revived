using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


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

        getEquippedParts();
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

    private void AddStats()
    {
        //Adds up all stats from currently equipped parts
    }

    private void ClearStats()
    {
        //Sets all stats to 0
    }

    



}
