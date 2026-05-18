using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamage : MonoBehaviour
{
    public float damage = 10f;
    public float knockbackForce = 10f;
    public float damageCooldown = 1f;
    public float verticalKnockback = 2f;

    [Header("Attack Timing")]
    public float attackHitDelay = 0.35f;
    public float attackStopDuration = 0.7f;

    private float lastDamageTime = -999f;
    private bool isAttacking;

    private EnemyAnimation enemyAnimation;
    private NavMeshAgent agent;
    private EnemyMovement enemyMovement;

    private void Awake()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();
        agent = GetComponent<NavMeshAgent>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void OnCollisionStay(Collision collision)
    {
        TryStartAttack(collision.collider);
    }

    private void OnTriggerStay(Collider other)
    {
        TryStartAttack(other);
    }

    private void TryStartAttack(Collider other)
    {
        if (isAttacking) return;
        if (Time.time - lastDamageTime < damageCooldown) return;
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null) return;
        if (!health.CanBeHit()) return;

        StartCoroutine(AttackAfterDelay(other));
    }

    private IEnumerator AttackAfterDelay(Collider playerCollider)
    {
        isAttacking = true;
        lastDamageTime = Time.time;

        StopEnemyMovement();

        if (enemyAnimation != null)
        {
            enemyAnimation.PlayAttack();
        }

        yield return new WaitForSeconds(attackHitDelay);

        if (playerCollider != null)
        {
            PlayerHealth health = playerCollider.GetComponent<PlayerHealth>();

            if (health != null && health.CanBeHit())
            {
                Vector3 knockDir = playerCollider.transform.position - transform.position;
                knockDir.y = 0f;

                if (knockDir.sqrMagnitude < 0.0001f)
                {
                    knockDir = Random.insideUnitSphere;
                    knockDir.y = 0f;
                }

                knockDir.Normalize();

                Vector3 knockback = knockDir * knockbackForce + Vector3.up * verticalKnockback;

                health.TakeDamage(damage, knockback);
            }
        }

        yield return new WaitForSeconds(Mathf.Max(0f, attackStopDuration - attackHitDelay));

        ResumeEnemyMovement();

        isAttacking = false;
    }

    private void StopEnemyMovement()
    {
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }
    }

    private void ResumeEnemyMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false;
        }

        if (enemyMovement != null)
        {
            enemyMovement.enabled = true;
        }
    }
}