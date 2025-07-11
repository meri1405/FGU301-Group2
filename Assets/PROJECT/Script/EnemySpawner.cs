using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject normalZombiePrefab;
    public GameObject fastZombiePrefab;
    public GameObject tankZombiePrefab;

    public Transform[] spawnPoints;
    public float spawnInterval = 0.5f;

    private List<GameObject> spawnedZombies = new List<GameObject>();

    public void SpawnZombiesForRound(int round, int totalZombies)
    {
        StartCoroutine(SpawnRoutine(round, totalZombies));
    }

    IEnumerator SpawnRoutine(int round, int totalZombies)
    {
        for (int i = 0; i < totalZombies; i++)
        {
            GameObject zombieToSpawn = GetZombieTypeForRound(round);
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            GameObject zombie = Instantiate(zombieToSpawn, spawnPoints[spawnIndex].position, Quaternion.identity);

            Debug.Log($"[Round {round}] Spawned: {zombieToSpawn.name} at point {spawnIndex}");

            spawnedZombies.Add(zombie);
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    GameObject GetZombieTypeForRound(int round)
    {
        if (round == 1)
            return normalZombiePrefab;

        if (round == 2)
            return Random.value < 0.5f ? normalZombiePrefab : fastZombiePrefab;

        if (round == 3)
            return Random.value < 0.5f ? normalZombiePrefab : tankZombiePrefab;

        
        // Round 5 trở lên
        float rand = Random.value;
        if (rand < 0.33f) return normalZombiePrefab;
        else if (rand < 0.66f) return fastZombiePrefab;
        else return tankZombiePrefab;
    }

    public void ClearAllZombies()
    {
        foreach (var z in spawnedZombies)
        {
            if (z != null)
                Destroy(z);
        }
        spawnedZombies.Clear();
    }
}
