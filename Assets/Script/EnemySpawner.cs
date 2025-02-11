using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab; 

    [SerializeField]
    private float spawnTime = 5.0f; 

    private float timeUntilSpawn;

    private void Awake()
    {
        // Validate that the enemy prefab is assigned
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned in the EnemySpawner script!", this);
            enabled = false; // Disable the script to prevent errors
            return;
        }

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
