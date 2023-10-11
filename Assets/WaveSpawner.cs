using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public WaveManager[] waves;
    //public Transform spawnPoint;
    private int waveIndex = 0;

    private void Start ()
    {
        if (waveIndex < waves.Length)
        {
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave ()
    {

        for(int i = 0; i < waves.Length; i++)
        {
            WaveManager wave = waves[waveIndex];

            yield return new WaitForSeconds(wave.secondsBeforeSpawning);

            for (int j = 0; j < wave.howManyEnemies; j++)
            {
                SpawnEnemy(wave.enemy);

                //how fast an enemy spawns after the other
                yield return new WaitForSeconds(1f / wave.enemiesPerSecond);
            }

            waveIndex++;

            if(waveIndex == waves.Length)
            {
                Debug.Log("Waves Complete");
                this.enabled = false;
            }

        }
    }

    void SpawnEnemy (GameObject enemy)
    {
        Instantiate(enemy, this.transform.position, this.transform.rotation);
    }
}
