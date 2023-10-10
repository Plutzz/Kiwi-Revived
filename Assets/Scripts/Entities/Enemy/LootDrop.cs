using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public GameObject lootPrefab;
    public List<ScriptableLoot> lootList = new List<ScriptableLoot>();

    void Start()
    {

    }

    public List<ScriptableLoot> GetDroppedItems() {
        int randomNum = Random.Range(0, 101);
        int randAmount = Random.Range(1, 4);
        List<ScriptableLoot> droppedItems = new List<ScriptableLoot>();

        foreach(ScriptableLoot loot in lootList) {
            if (randomNum <= loot.dropChance) {
                for (int i = 0; i < randAmount; i++)
                    droppedItems.Add(loot);
            }
        }

        if (droppedItems.Count > 0) {
            // Drop One Item
            // ScriptableLoot droppedItem = droppedItems[Random.Range(0, droppedItems.Count)];

            // Drop All Items
            return droppedItems;
        }

        Debug.Log("No Loot Dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawn) {
        List<ScriptableLoot> droppedItems = GetDroppedItems();
        foreach (ScriptableLoot loot in droppedItems) {
            Debug.Log("Loot Dropped: " + loot.lootName);
        }

        if (droppedItems != null) {
            foreach (ScriptableLoot loot in droppedItems) {
                GameObject lootObj = Instantiate(lootPrefab, spawn, Quaternion.identity);
                lootObj.GetComponent<SpriteRenderer>().sprite = loot.lootSprite;
                lootObj.name = loot.lootName;

                float randomX = Random.Range(-1f, 1f);
                float randomY = Random.Range(-1f, 1f);
                float force = Random.Range(0.5f, 1f);
                Vector2 dropDirection = new Vector2(randomX, randomY);
                lootObj.GetComponent<Rigidbody2D>().AddForce(dropDirection * force, ForceMode2D.Impulse);
            }
        }
    }
}
