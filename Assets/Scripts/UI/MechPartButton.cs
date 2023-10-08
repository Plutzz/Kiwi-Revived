using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MechPartButton : MonoBehaviour
{
    [SerializeField] private ScriptableMechPart part;

    public void EquipPart()
    {
        MechSystem.Instance.UnequipBodies();
        part.Equipped = true;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("PlatformingMovement");
    }

}
