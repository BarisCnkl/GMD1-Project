using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage = 10f;
    public float knockbackForce = 10f;
    public float damageCooldown = 1f;
    public float verticalKnockback = 2f;

    private float lastDamageTime = -999f;

    void OnCollisionStay(Collision collision)
    {
        TryDamage(collision.collider);
    }

    void OnTriggerStay(Collider other)
    {
        TryDamage(other);
    }

    void TryDamage(Collider other)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null) return;
        if (!health.CanBeHit()) return;

        Vector3 knockDir = other.transform.position - transform.position;
        knockDir.y = 0f;

        if (knockDir.sqrMagnitude < 0.0001f)
            knockDir = Random.insideUnitSphere;

        knockDir.Normalize();

        Vector3 knockback = knockDir * knockbackForce + Vector3.up * verticalKnockback;

        health.TakeDamage(damage, knockback);
        lastDamageTime = Time.time;
    }
}
