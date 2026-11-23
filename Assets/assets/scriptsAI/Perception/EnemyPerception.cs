
using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Vision")]
    public float viewDistance = 12f;

    [Range(0f, 360f)]
    public float viewAngle = 110f;

    public LayerMask visionMask = ~0;

    [Header("Hearing")]
    public float walkHearingRange = 4f;
    public float runHearingRange = 8f;
    public float runningThreshold = 6f;

    [Header("Detection Results")]
    public bool canSeePlayer;
    public bool canHearPlayer;

    private CharacterController playerController;

    public bool PlayerDetected =>
        canSeePlayer || canHearPlayer;

    void Awake()
    {
        if (player != null)
        {
            playerController =
                player.GetComponent<CharacterController>();
        }
    }

    public void UpdatePerception()
    {
        canSeePlayer = CheckVision();
        canHearPlayer = CheckHearing();
    }



    bool CheckVision()
    {
        if (player == null)
            return false;


        Vector3 eyePosition =
            transform.position + Vector3.up * 1.5f;

        // Oyuncunun Collider merkezini hedefle
        CharacterController playerCollider =
            player.GetComponent<CharacterController>();

        Vector3 targetPosition =
            playerCollider != null
            ? playerCollider.bounds.center
            : player.position;


        Vector3 direction = targetPosition - eyePosition;
        float distance = direction.magnitude;

        if (distance > viewDistance)
            return false;

        Vector3 horizontalDirection =
            Vector3.ProjectOnPlane(direction, Vector3.up);

        if (horizontalDirection.sqrMagnitude > 0.001f)
        {
            float angle = Vector3.Angle(
                transform.forward,
                horizontalDirection
            );

            if (angle > viewAngle / 2f)
                return false;
        }

        RaycastHit[] hits = Physics.RaycastAll(
            eyePosition,
            direction.normalized,
            distance + 0.1f,
            visionMask,
            QueryTriggerInteraction.Ignore
        );

        System.Array.Sort(
            hits,
            (a, b) => a.distance.CompareTo(b.distance)
        );

        foreach (RaycastHit hit in hits)
        {
            // Düþmanýn kendi Collider'larýný atla
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
                continue;

            bool isPlayer =
                hit.transform == player ||
                hit.transform.IsChildOf(player);

            Debug.DrawLine(
                eyePosition,
                hit.point,
                isPlayer ? Color.green : Color.red,
                0.3f
            );

            Debug.Log(
                "Vision Hit: " + hit.collider.name +
                " | Is Player: " + isPlayer
            );

            return isPlayer;
        }

        Debug.DrawLine(
            eyePosition,
            targetPosition,
            Color.magenta,
            0.3f
        );

        Debug.Log("Vision: No collider hit!");

        return false;
    }



    bool CheckHearing()
    {
        if (player == null)
            return false;

        if (playerController == null)
            return false;

        Vector3 velocity = playerController.velocity;
        velocity.y = 0f;

        float speed = velocity.magnitude;

        if (speed < 0.15f)
            return false;

        float hearingRange =
            speed >= runningThreshold
            ? runHearingRange
            : walkHearingRange;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        return distance <= hearingRange;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            viewDistance
        );

        Vector3 leftDirection =
            Quaternion.Euler(0, -viewAngle / 2f, 0)
            * transform.forward;

        Vector3 rightDirection =
            Quaternion.Euler(0, viewAngle / 2f, 0)
            * transform.forward;

        Vector3 eye =
            transform.position + Vector3.up * 1.5f;

        Gizmos.DrawRay(
            eye,
            leftDirection * viewDistance
        );

        Gizmos.DrawRay(
            eye,
            rightDirection * viewDistance
        );

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position,
            runHearingRange
        );
    }
}

