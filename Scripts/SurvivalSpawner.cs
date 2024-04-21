using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalSpawner : MonoBehaviour
{
    public Vector3[] spawnBoundCorners;
    public GameObject normalEnemyPrefab;
    public GameObject slimePrefab;
    public GameObject elementalPrefab;

    public float initialSpawnDelay = 5.0f; 
    public float minimumSpawnDelay = 0.5f; 
    public float spawnSpeedupRate = 0.9f; 

    private float spawnDelay;
    private float timer;

    private List<GameObject> mobsList = new List<GameObject>();

    void Start()
    {
        spawnDelay = initialSpawnDelay;
        timer = spawnDelay;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnEnemy();
            timer = spawnDelay;
            spawnDelay = Mathf.Max(minimumSpawnDelay, spawnDelay * spawnSpeedupRate);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPoint = GetRandomSpawnPoint();

        int enemyType = Random.Range(0, 10);
        GameObject enemyPrefab = null;

        if (enemyType < 5)
            enemyPrefab = normalEnemyPrefab;
        else if (enemyType < 9)
            enemyPrefab = slimePrefab;
        else
            enemyPrefab = elementalPrefab;

        if (enemyPrefab != null)
        {
            GameObject enemy = (GameObject)Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            mobsList.Add(enemy);
        }
    }

    Vector3 GetRandomSpawnPoint()
    {
        // GETTING A RANDOM POINT ON A POLYGON SHOULD NOT BE THIS HARD. LIKE WHAT EVEN IS THIS?
        bool useFirstTriangle = Random.value > 0.5f;

        Vector3 point1 = spawnBoundCorners[0];
        Vector3 point2 = useFirstTriangle ? spawnBoundCorners[1] : spawnBoundCorners[2];
        Vector3 point3 = useFirstTriangle ? spawnBoundCorners[2] : spawnBoundCorners[3];

        float alpha = Random.value;
        float beta = Random.value * (1 - alpha);
        float gamma = 1 - alpha - beta;

        return alpha * point1 + beta * point2 + gamma * point3;
    }

    public void Reset()
    {
        foreach (GameObject enemy in mobsList)
        {
            Destroy(enemy);
        }
        spawnDelay = initialSpawnDelay;
        timer = spawnDelay;
    }
}
