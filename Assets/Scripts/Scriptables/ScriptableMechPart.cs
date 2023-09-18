using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  Keeping all relevant information about a unit on a scriptable means we can gather and show
///  info on the meny screen, without instantiating the unit prefab.
/// </summary>

[CreateAssetMenu(fileName = "New Scriptable Mech Part")]
public class ScriptableMechPart : ScriptableObject
{
    [SerializeField] private Stats stats;
    public Stats BaseStats => stats;

    // Used in game
    // public MechPart prefab

    // Used in menus
    public string Descriptions;
    public Sprite MenuSprite;
    public bool Equipped;
}

/// <summary>
/// Keeping base stats as a struct on the scriptable keeps it flexible and easily editable.
/// We can pass this struct to the spawned prefab unity and alter them depending on conditions (Upgrades ect.)
/// </summary>
[Serializable]
public struct Stats
{
    public int Vision;
    public int Speed;
    public int Armor;
}
