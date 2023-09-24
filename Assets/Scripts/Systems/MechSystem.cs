using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
    }

    private void LoadResources()
    {
        mechHeads = Resources.LoadAll<ScriptableMechPart>("MechParts/Head");
        mechBodies = Resources.LoadAll<ScriptableMechPart>("MechParts/Chasis");
    }


}
