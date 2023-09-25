using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechPartButton : MonoBehaviour
{
    [SerializeField] private ScriptableMechPart part;

    public void EquipPart()
    {
        MechSystem.Instance.UnequipBodies();
        part.Equipped = true;
        
    }

}
