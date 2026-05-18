using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Debug")]
    [SerializeField] private float currentSpeed;

    public bool IsDead { get; private set; }

    private NavMeshAgent agent;
    private Vector3 lastPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (animator == null || animator.runtimeAnimatorController == null)
        {
            Animator[] animators = GetComponentsInChildren<Animator>();

            foreach (Animator foundAnimator in animators)
            {
                if (foundAnimator.runtimeAnimatorController != null)
                {
                    animator = foundAnimator;
                    break;
                }
            }
        }

        lastPosition = transform.position;
    }

    private void Update()
    {
        if (IsDead) return;
        if (animator == null || animator.runtimeAnimatorController == null) return;

        float speed = 0f;

        if (agent != null)
        {
            speed = agent.velocity.magnitude;
        }

        if (speed < 0.01f)
        {
            Vector3 movement = transform.position - lastPosition;
            movement.y = 0f;
            speed = movement.magnitude / Time.deltaTime;
        }

        currentSpeed = speed;
        animator.SetFloat("Speed", currentSpeed);

        lastPosition = transform.position;
    }

    public void PlayAttack()
    {
        if (IsDead) return;

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void PlayDeath()
    {
        if (IsDead) return;

        IsDead = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }

        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }

        EnemyDamage enemyDamage = GetComponent<EnemyDamage>();
        if (enemyDamage != null)
        {
            enemyDamage.enabled = false;
        }

        Collider enemyCollider = GetComponent<Collider>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetTrigger("Die");
        }
    }
}