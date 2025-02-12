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
        SetTimeUntilSpawn();
    }

    // Update is called once per frame
    private void Update()
    {

        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            SetTimeUntilSpawn();
            timeUntilSpawn = spawnTime;
        }
    }
    private void SetTimeUntilSpawn()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
