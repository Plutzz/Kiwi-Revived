using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Loot", menuName = "Loot")]
public class ScriptableLoot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;

    public ScriptableLoot(string lootName, int dropChance) {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
