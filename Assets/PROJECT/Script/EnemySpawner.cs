using UnityEngine;
using System.Collections;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;             // Gán prefab zombie vào đây
    public Transform[] spawnPoints;            // Các vị trí spawn
    public float spawnInterval = 3f;           // Bao lâu spawn 1 lần
    public int maxEnemies = 10;                // Giới hạn số lượng

    private int enemiesSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (enemiesSpawned < maxEnemies)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
