using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Attack Visual Offset")]
    [SerializeField] private Transform visualModel;
    [SerializeField] private float attackYOffset = 0.25f;
    [SerializeField] private float attackOffsetDuration = 0.4f;

    [Header("Debug")]
    [SerializeField] private float currentSpeed;

    private NavMeshAgent agent;
    private Vector3 lastPosition;
    private Vector3 originalVisualLocalPosition;
    private Coroutine attackOffsetRoutine;

    public bool IsDead { get; private set; }

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

        if (visualModel == null && animator != null)
        {
            visualModel = animator.transform;
        }

        if (visualModel != null)
        {
            originalVisualLocalPosition = visualModel.localPosition;
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

        if (visualModel != null)
        {
            if (attackOffsetRoutine != null)
            {
                StopCoroutine(attackOffsetRoutine);
            }

            attackOffsetRoutine = StartCoroutine(AttackYOffsetRoutine());
        }
    }

    private IEnumerator AttackYOffsetRoutine()
    {
        Vector3 raisedPosition = originalVisualLocalPosition + Vector3.up * attackYOffset;

        visualModel.localPosition = raisedPosition;

        yield return new WaitForSeconds(attackOffsetDuration);

        visualModel.localPosition = originalVisualLocalPosition;
        attackOffsetRoutine = null;
    }

    public void PlayDeath()
    {
        if (IsDead) return;

        IsDead = true;

        if (attackOffsetRoutine != null)
        {
            StopCoroutine(attackOffsetRoutine);
            attackOffsetRoutine = null;
        }

        if (visualModel != null)
        {
            visualModel.localPosition = originalVisualLocalPosition;
        }

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