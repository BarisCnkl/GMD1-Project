using UnityEngine;
using UnityEngine.AI;

public class TeddyBearSpawner : MonoBehaviour
{
    public GameObject teddyBearPrefab;

    public float spawnInterval = 10f;
    public float spawnRange = 4f;
    public float minDistance = 1.5f;
    public float groundY = 0.5f;
    public int maxTeddyBears = 1;

    private int currentCount = 0;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        InvokeRepeating(nameof(TrySpawn), 1f, spawnInterval);
    }

    void TrySpawn()
    {
        if (currentCount < maxTeddyBears)
            SpawnTeddyBear();
    }

    void SpawnTeddyBear()
    {
        Vector3 center = player != null ? player.position : transform.position;

        Vector3 spawnPosition = Vector3.zero;
        bool found = false;

        for (int i = 0; i < 15; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float dist = Random.Range(minDistance, spawnRange);

            Vector3 candidate = new Vector3(
                center.x + Mathf.Cos(angle) * dist,
                center.y + 1f,
                center.z + Mathf.Sin(angle) * dist
            );

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                spawnPosition = new Vector3(hit.position.x, groundY, hit.position.z);
                found = true;
                break;
            }
        }

        if (!found)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float dist = Random.Range(minDistance, spawnRange);
            spawnPosition = new Vector3(
                center.x + Mathf.Cos(angle) * dist,
                groundY,
                center.z + Mathf.Sin(angle) * dist
            );
        }

        Instantiate(teddyBearPrefab, spawnPosition, Quaternion.identity);
        currentCount++;
    }

    public void OnTeddyBearPickedUp()
    {
        currentCount = Mathf.Max(0, currentCount - 1);
    }
}
