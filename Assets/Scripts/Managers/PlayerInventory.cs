using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInventory : MonoBehaviour
{
    public Dictionary<ScriptableLoot, int> inventory = new Dictionary<ScriptableLoot, int>();

    public static PlayerInventory Instance;
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
    }
    public void AddItem(string itemName) {
        ScriptableLoot item = Resources.Load<ScriptableLoot>("Items/" + itemName);
        if (inventory.ContainsKey(item))
            inventory[item]++;
        else
            inventory.Add(item, 1);
    }
}


