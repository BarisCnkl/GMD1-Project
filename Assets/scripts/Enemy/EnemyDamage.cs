using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage = 10f;
    public float knockbackForce = 10f;
    public float damageCooldown = 1f;
    public float verticalKnockback = 2f;

    private float lastDamageTime = -999f;
    private EnemyAnimation enemyAnimation;

    private void Awake()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    private void OnCollisionStay(Collision collision)
    {
        TryDamage(collision.collider);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamage(other);
    }

    private void TryDamage(Collider other)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null) return;
        if (!health.CanBeHit()) return;

        Vector3 knockDir = other.transform.position - transform.position;
        knockDir.y = 0f;

        if (knockDir.sqrMagnitude < 0.0001f)
        {
            knockDir = Random.insideUnitSphere;
            knockDir.y = 0f;
        }

        knockDir.Normalize();

        Vector3 knockback = knockDir * knockbackForce + Vector3.up * verticalKnockback;

        if (enemyAnimation != null)
        {
            enemyAnimation.PlayAttack();
        }

        health.TakeDamage(damage, knockback);
        lastDamageTime = Time.time;
    }
}