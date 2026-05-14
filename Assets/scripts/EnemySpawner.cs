using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnInterval = 2f;
    public float spawnRange = 1f;
    public float groundY = 2;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Random position around spawner
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + x,
            groundY,
            transform.position.z + z
        );

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}