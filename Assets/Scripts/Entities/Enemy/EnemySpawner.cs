using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject Enemy1;
    [SerializeField]
    private GameObject Enemy2;
    [SerializeField]
    private GameObject Enemy3;
    [SerializeField]
    private GameObject Enemy4;

    [SerializeField]
    private float Enemy1Interval = 3.5f;
    [SerializeField]
    private float Enemy2Interval = 10f;
    [SerializeField]
    private float Enemy3Interval = 3.5f;
    [SerializeField]
    private float Enemy4Interval = 10f;

    [SerializeField]
    private float playerSafeRadius = 5f;

    public static int numEnemies; //Increment when spawn, decrement it when enemy is killed

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(Enemy1Interval, Enemy1));
        StartCoroutine(spawnEnemy(Enemy2Interval, Enemy2));
        StartCoroutine(spawnEnemy(Enemy3Interval, Enemy3));
        StartCoroutine(spawnEnemy(Enemy4Interval, Enemy4));

        SpawnEnemy(Enemy1);
        SpawnEnemy(Enemy2);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        Vector3 _spawnPoint = new Vector3(0, 0, 0) + transform.position;
        float _distanceFromPlayer = (player.transform.position - _spawnPoint).magnitude;
        if (_distanceFromPlayer > playerSafeRadius)
        {
            SpawnEnemy(enemy);
        }
        StartCoroutine(spawnEnemy(interval, enemy));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerSafeRadius);
    }

    private void SpawnEnemy(GameObject enemy)
    {
        Vector3 _spawnPoint = new Vector3(0, 0, 0) + transform.position;
        float _distanceFromPlayer = (player.transform.position - _spawnPoint).magnitude;
        GameObject newEnemy = Instantiate(enemy, _spawnPoint, Quaternion.identity);
        numEnemies++;
    }
}
