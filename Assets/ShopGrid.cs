using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ShopGrid : MonoBehaviour
{


    public GameObject[] shopItems; // Sprites of the items/abilities/cost that can be bought
    public GameObject gridElement;
    public GridLayoutGroup grid;
    public string room;
    void Start()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            GameObject element = Instantiate(gridElement, transform.position, Quaternion.identity);
            element.transform.SetParent(grid.transform);
            element.transform.localScale = new Vector3(1, 1, 1);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(room);
        }
    }
}
