using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float bubbleBuffer = 0.3f;

    private Transform player;
    private NavMeshAgent agent;
    private PlayerProtection playerProtection;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerProtection = playerObj.GetComponent<PlayerProtection>();
        }
    }

    void Update()
    {
        if (player == null) return;

        if (playerProtection == null)
            playerProtection = player.GetComponent<PlayerProtection>();

        agent.stoppingDistance = 0f;

        if (playerProtection != null && playerProtection.IsProtected)
        {
            float radius = playerProtection.protectionRadius + bubbleBuffer;
            Vector3 fromPlayer = transform.position - player.position;
            fromPlayer.y = 0f;

            Vector3 dir = fromPlayer.sqrMagnitude > 0.0001f
                ? fromPlayer.normalized
                : Random.insideUnitSphere.normalized;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) dir = Vector3.right;

            Vector3 edgePos = player.position + dir * radius;

            if (NavMesh.SamplePosition(edgePos, out NavMeshHit hit, 3f, NavMesh.AllAreas))
                agent.SetDestination(hit.position);
            else
                agent.SetDestination(edgePos);
        }
        else
        {
            agent.SetDestination(player.position);
        }
    }
}
