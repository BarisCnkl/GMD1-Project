using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    public GameObject stonePrefab;

    public float spawnInterval = 2f;
    public float spawnRange = 10f;
    public float groundY = 0.5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnStone), 1f, spawnInterval);
    }

    void SpawnStone()
    {
        // Random position around spawner
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + x,
            groundY,
            transform.position.z + z
        );

        Instantiate(stonePrefab, spawnPosition, Quaternion.identity);
    }
}