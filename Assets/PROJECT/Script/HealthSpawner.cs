using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    public GameObject healthPickupPrefab;
    public float spawnInterval = 5f;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    public int maxHealthPickups = 20;

    private List<GameObject> spawnedHealthPickups = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnHealthPickup());
    }

    IEnumerator SpawnHealthPickup()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            
            spawnedHealthPickups.RemoveAll(item => item == null);

            if (spawnedHealthPickups.Count >= maxHealthPickups)
                continue;

            Vector2 spawnPos = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            GameObject newHP = Instantiate(healthPickupPrefab, spawnPos, Quaternion.identity);
            spawnedHealthPickups.Add(newHP);
        }
    }
}
