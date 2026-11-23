
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyActions : MonoBehaviour
{
    [Header("Shared Settings")]
    public Transform player;
    public Transform[] patrolPoints;

    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5.5f;

    private NavMeshAgent agent;
    private int patrolIndex = 0;
    private bool patrolStarted = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Patrol()
    {
        if (!agent.isOnNavMesh)
            return;

        agent.speed = patrolSpeed;
        agent.stoppingDistance = 0.3f;

        if (patrolPoints == null ||
            patrolPoints.Length == 0)
            return;

        if (!patrolStarted)
        {
            SetPatrolDestination();
            patrolStarted = true;
            return;
        }

        if (!agent.pathPending &&
            agent.remainingDistance <=
            agent.stoppingDistance + 0.1f)
        {
            SetPatrolDestination();
        }
    }

    void SetPatrolDestination()
    {
        if (!agent.isOnNavMesh)
            return;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Transform point = patrolPoints[patrolIndex];

            patrolIndex =
                (patrolIndex + 1) % patrolPoints.Length;

            if (point != null)
            {
                agent.SetDestination(point.position);
                return;
            }
        }
    }

    public void Chase()
    {
        if (!agent.isOnNavMesh || player == null)
            return;

        patrolStarted = false;

        agent.speed = chaseSpeed;
        agent.stoppingDistance = 1.5f;

        agent.SetDestination(player.position);
    }

    public void Stop()
    {
        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }
}

