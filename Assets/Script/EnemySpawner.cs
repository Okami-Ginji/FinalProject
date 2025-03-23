using UnityEngine;
using System.Collections;
using Cinemachine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject EnemySeriesPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject bossPrefab;

    [SerializeField] private float despawnDistance;
    [SerializeField] private float spawnDistance;

    [SerializeField] private GameObject followCamera;
    [SerializeField] private GameObject spawnBossCamera;

    private Transform player;
    private float gameTime = 0f;
    private float spawnInterval = 5f;
    private int enemiesPerSpawn = 2;
    private bool bossSpawned = false;
    private int maxMeleeEnemies = 20;
    private int maxRangedEnemies = 10;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(CheckAndDespawnEnemies());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawnBoss();
        }


    }
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            PlayerControl foundPlayer = FindObjectOfType<PlayerControl>();
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
                gameTime += spawnInterval;
                AdjustSpawnParameters();
                SpawnWave();
            }
            else
            {
                Debug.LogWarning("Không tìm thấy PlayerControl, đang chờ...");
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void AdjustSpawnParameters()
    {
        if (gameTime >= 120f) { enemiesPerSpawn = 7; spawnInterval = 3f; } // 2 phút
        else if (gameTime >= 300f) { enemiesPerSpawn = 10; spawnInterval = 2.5f; } // 5 phút
        else if (gameTime >= 480f) { enemiesPerSpawn = 15; spawnInterval = 2f; } // 8 phút
    }

    private void SpawnWave()
    {
        if (gameTime >= 540f && !bossSpawned) // 9 phút: Boss xuất hiện
        {
            spawnBoss();
        }
        else
        {
            int currentMeleeCount = GameObject.FindGameObjectsWithTag("EnemySeries").Length;
            int currentRangedCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                GameObject enemyToSpawn;
                bool isRanged = Random.Range(0f, 1f) < GetEnemyRate();

                if (isRanged && currentRangedCount < maxRangedEnemies)
                {
                    enemyToSpawn = EnemyPrefab;
                    currentRangedCount++;
                }
                else if (!isRanged && currentMeleeCount < maxMeleeEnemies)
                {
                    enemyToSpawn = EnemySeriesPrefab;
                    currentMeleeCount++;
                }
                else
                {
                    continue; // Bỏ qua nếu đã đạt giới hạn
                }

                Instantiate(enemyToSpawn, GetSpawnPosition(), Quaternion.identity);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        if (player == null) return transform.position;
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnDistance;

        Debug.Log("aaaaa: " + spawnDistance);
        return player.position + offset;

    }

    private float GetEnemyRate()
    {
        if (gameTime >= 480f) return 0.5f; // 8 phút: 50% ranged
        if (gameTime >= 300f) return 0.4f; // 5 phút: 40% ranged
        return 0.3f; // Mặc định: 30% ranged
    }

    private IEnumerator CheckAndDespawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemySeries");
            foreach (GameObject enemy in enemies)
            {
                if (Vector3.Distance(player.position, enemy.transform.position) > despawnDistance)
                {
                    Destroy(enemy);
                }
            }

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (Vector3.Distance(player.position, enemy.transform.position) > despawnDistance)
                {
                    Destroy(enemy);
                }
            }
        }
    }

    private void destroyAllEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemySeries");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    public void spawnBoss()
    {
        destroyAllEnemy();
        StopCoroutine(SpawnEnemies());
        StopCoroutine(CheckAndDespawnEnemies());
        followCamera.SetActive(false);
        Instantiate(bossPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        bossSpawned = true;
        StartCoroutine(ReactivateCamera());
    }

    private IEnumerator ReactivateCamera()
    {
        yield return new WaitForSeconds(2f);
        BossAI bossScript = FindObjectOfType<BossAI>();
        if (bossScript != null)
        {
            bossScript.enabled = true;
        }
        followCamera.SetActive(true);
    }

}
