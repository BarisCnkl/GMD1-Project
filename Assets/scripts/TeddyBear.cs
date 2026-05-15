using UnityEngine;

public class TeddyBear : MonoBehaviour
{
    private TeddyBearSpawner spawner;
    private bool pickedUp = false;

    void Awake()
    {
        spawner = FindFirstObjectByType<TeddyBearSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (pickedUp) return;
        if (!other.CompareTag("Player")) return;

        pickedUp = true;

        PlayerProtection protection = other.GetComponent<PlayerProtection>();
        if (protection == null)
            protection = other.gameObject.AddComponent<PlayerProtection>();

        protection.ActivateProtection();
        spawner?.OnTeddyBearPickedUp();
        Destroy(gameObject);
    }
}
