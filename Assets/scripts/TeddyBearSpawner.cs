using UnityEngine;

public class TeddyBearSpawner : MonoBehaviour
{
    public GameObject teddyBearPrefab;

    public float spawnInterval = 10f;
    public float spawnRange = 10f;
    public float groundY = 0.5f;
    public int maxTeddyBears = 1;

    private int currentCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnTeddyBear), 1f, spawnInterval);
    }

    void SpawnTeddyBear()
    {
        if (currentCount >= maxTeddyBears)
            return;

        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPosition = new Vector3(
            transform.position.x + x,
            groundY,
            transform.position.z + z
        );

        Instantiate(teddyBearPrefab, spawnPosition, Quaternion.identity);
        currentCount++;
    }

    public void OnTeddyBearPickedUp()
    {
        currentCount--;
    }
}
