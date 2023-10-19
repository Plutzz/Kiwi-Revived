using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Weapon Part")]
public class ScriptableWeaponPart : ScriptableObject
{
    [SerializeField] private WeaponStats stats;
    public WeaponStats BaseStats => stats;




    /// <summary>
    /// Keeping base stats as a struct on the scriptable keeps it flexible and easily editable.
    /// We can pass this struct to the spawned prefab unity and alter them depending on conditions (Upgrades ect.)
    /// </summary>
    [Serializable]
    public struct WeaponStats
    {
        public int Velocity;
        public int Damage;
    }
}
