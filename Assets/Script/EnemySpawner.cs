using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float spawnTime = 10.0f;

    private float timeUntilSpawn;

    private void Awake()
    {
        timeUntilSpawn = spawnTime; // Initialize the spawn timer
    }

    private void Update()
    {

        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            SpawnEnemy();
            timeUntilSpawn = spawnTime;
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
