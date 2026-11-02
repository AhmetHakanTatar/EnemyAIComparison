
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;

    private NavMeshAgent agent;
    private int currentPoint = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
        {
            MoveToNextPoint();
        }
    }

    void Update()
    {
        if (patrolPoints.Length == 0 || !agent.isOnNavMesh)
            return;

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextPoint();
        }
    }

    void MoveToNextPoint()
    {
        if (!agent.isOnNavMesh)
            return;

        if (patrolPoints[currentPoint] != null)
        {
            agent.SetDestination(
                patrolPoints[currentPoint].position
            );
        }

        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }
}
