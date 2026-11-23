
using UnityEngine;

public class FSMController : MonoBehaviour, IEnemyAIController
{
    private enum State
    {
        Patrol,
        Chase
    }

    private State currentState = State.Patrol;

    private EnemyPerception perception;
    private EnemyActions actions;

    [Header("FSM Settings")]
    public float lostPlayerDelay = 2f;

    private float lastDetectionTime;

    public string CurrentAction =>
        currentState.ToString();

    public void Initialize(
        EnemyPerception sharedPerception,
        EnemyActions sharedActions)
    {
        perception = sharedPerception;
        actions = sharedActions;

        currentState = State.Patrol;
        lastDetectionTime = -lostPlayerDelay;
    }

    public void TickDecision()
    {
        if (perception == null || actions == null)
            return;

        if (perception.PlayerDetected)
        {
            lastDetectionTime = Time.time;

            if (currentState != State.Chase)
            {
                ChangeState(State.Chase);
            }
        }
        else if (currentState == State.Chase)
        {
            if (Time.time - lastDetectionTime
                >= lostPlayerDelay)
            {
                ChangeState(State.Patrol);
            }
        }
    }

    private void ChangeState(State newState)
    {
        currentState = newState;

        Debug.Log(
            "FSM changed state to: " + currentState
        );
    }
}

