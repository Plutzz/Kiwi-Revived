using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInventory))]
public class InventoryEditor : Editor {
    public override void OnInspectorGUI() {
        PlayerInventory inventoryScript = (PlayerInventory)target;

        EditorGUILayout.LabelField("Inventory", EditorStyles.boldLabel);
        foreach (KeyValuePair<ScriptableLoot, int> item in inventoryScript.inventory) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(item.Key.lootName);
            EditorGUILayout.LabelField(item.Value.ToString());
            EditorGUILayout.EndHorizontal();
        }

        base.OnInspectorGUI();
    }
}