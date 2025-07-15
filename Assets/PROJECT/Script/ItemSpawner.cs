using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject healthPickupPrefab;
    public GameObject speedBoostPrefab;
    public GameObject powerBoostPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 5f;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    [Header("Max Items")]
    public int maxHealth = 10;
    public int maxSpeedBoost = 5;
    public int maxPowerBoost = 5;

    private List<GameObject> healthItems = new List<GameObject>();
    private List<GameObject> speedItems = new List<GameObject>();
    private List<GameObject> powerItems = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Clean null (đã bị nhặt/hủy)
            healthItems.RemoveAll(item => item == null);
            speedItems.RemoveAll(item => item == null);
            powerItems.RemoveAll(item => item == null);

            // Spawn nếu chưa đạt max
            if (healthItems.Count < maxHealth)
                SpawnItem(healthPickupPrefab, healthItems);

            if (speedItems.Count < maxSpeedBoost)
                SpawnItem(speedBoostPrefab, speedItems);

            if (powerItems.Count < maxPowerBoost)
                SpawnItem(powerBoostPrefab, powerItems);
        }
    }

    void SpawnItem(GameObject prefab, List<GameObject> trackingList)
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        GameObject item = Instantiate(prefab, spawnPos, Quaternion.identity);
        trackingList.Add(item);
    }
}
