using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float knockbackDuration = 0.3f;
    public float invincibilityAfterHit = 0.5f;

    public bool IsInKnockback { get; private set; }
    public bool IsDead { get; private set; }

    private Rigidbody rb;
    private float knockbackTimer;
    private float lastHitTime = -999f;
    private PlayerProtection protection;

    void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        protection = GetComponent<PlayerProtection>();
    }

    public bool CanBeHit()
    {
        if (IsDead) return false;
        if (protection != null && protection.IsProtected) return false;
        if (Time.time - lastHitTime < invincibilityAfterHit) return false;
        return true;
    }

    public void TakeDamage(float amount, Vector3 knockbackVelocity)
    {
        if (!CanBeHit()) return;

        currentHealth -= amount;
        lastHitTime = Time.time;
        Debug.Log($"[PlayerHealth] -{amount} HP. Current: {currentHealth}/{maxHealth}");

        if (rb != null)
        {
            rb.linearVelocity = new Vector3(knockbackVelocity.x, rb.linearVelocity.y, knockbackVelocity.z);
        }

        IsInKnockback = true;
        knockbackTimer = knockbackDuration;

        if (currentHealth <= 0f) Die();
    }

    void Update()
    {
        if (IsInKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f) IsInKnockback = false;
        }
    }

    void Die()
    {
        IsDead = true;
        Debug.Log("[PlayerHealth] Player died");
    }
}
