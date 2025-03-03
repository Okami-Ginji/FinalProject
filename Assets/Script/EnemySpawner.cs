using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float spawnIntervalse = 10.0f; 

    private static bool isFirstSpawned = false; 
    private static int spawnOrderCounter = 0; 
    private static float delayBetweenSpawns = 5.0f; 

    private int spawnOrder; 

    private void Start()
    {
        spawnOrder = spawnOrderCounter++; 
        StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
        if (!isFirstSpawned)
        {
            
            SpawnEnemy();
            isFirstSpawned = true;
            yield return new WaitForSeconds(5f); 
        }
        else
        {
            
            yield return new WaitForSeconds(spawnOrder * delayBetweenSpawns);
        }

        
        while (true)
        {
            yield return new WaitForSeconds(spawnIntervalse);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
