using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInventory : MonoBehaviour
{

    public static PlayerInventory Instance;
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
    }
    public void AddItem(string itemName) {
        Debug.Log("Added " + itemName + " to inventory");
    }
}