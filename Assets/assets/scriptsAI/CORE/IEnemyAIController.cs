using UnityEngine;

public interface IEnemyAIController
{
    void Initialize(
        EnemyPerception perception,
        EnemyActions actions
    );

    void TickDecision();

    string CurrentAction { get; }
}
