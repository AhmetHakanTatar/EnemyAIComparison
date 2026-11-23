

using UnityEngine;

[RequireComponent(typeof(EnemyPerception))]
[RequireComponent(typeof(EnemyActions))]
public class EnemyBrain : MonoBehaviour
{
    public MonoBehaviour controllerComponent;
    public float decisionInterval = 0.2f;

    private EnemyPerception perception;
    private EnemyActions actions;
    private IEnemyAIController controller;

    private float decisionTimer;

    void Awake()
    {
        perception = GetComponent<EnemyPerception>();
        actions = GetComponent<EnemyActions>();
    }

    void Start()
    {
        controller = controllerComponent as IEnemyAIController;

        if (controller == null)
        {
            Debug.LogError("AI Controller not assigned!");
            enabled = false;
            return;
        }

        controller.Initialize(perception, actions);

        Debug.Log("EnemyBrain initialized successfully!");
    }

    void Update()
    {
        decisionTimer -= Time.deltaTime;

        if (decisionTimer <= 0f)
        {
            perception.UpdatePerception();
            controller.TickDecision();

            decisionTimer = Mathf.Max(0.01f, decisionInterval);
        }

        switch (controller.CurrentAction)
        {
            case "Patrol":
                actions.Patrol();
                break;

            case "Chase":
                actions.Chase();
                break;

            default:
                actions.Stop();
                break;
        }
    }
}
