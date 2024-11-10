using UnityEngine;

public class FirstEyeMovement : MonoBehaviour
{
    public Transform player;
    public Vector2 horizontalRange = new Vector2(-0.2f, 0.2f);
    public Vector2 verticalRange = new Vector2(-0.2f, 0.2f);
    public float smoothSpeed = 0.1f;
    public float minDistance = 1.0f;
    public float maxDistance = 5.0f;
    public float maxVerticalOffset = -0.1f;
    public float maxPupilConvergence = 0.1f;
    public float closeDistanceThreshold = 2.0f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            Vector3 localDirection = transform.parent.InverseTransformDirection(directionToPlayer).normalized;

            float distanceToPlayer = directionToPlayer.magnitude;

            float distanceFactor = Mathf.InverseLerp(maxDistance, minDistance, distanceToPlayer);
            float additionalVerticalOffset = Mathf.Lerp(0, maxVerticalOffset, distanceFactor);

            float pupilConvergence = Mathf.Lerp(0, maxPupilConvergence, distanceFactor);

            Vector3 clampedPosition = new Vector3(
                Mathf.Clamp(localDirection.x, horizontalRange.x, horizontalRange.y),
                Mathf.Clamp(localDirection.y, verticalRange.x, verticalRange.y),
                initialPosition.z
            );

            Vector3 firstTargetPosition = new Vector3(
                initialPosition.x + clampedPosition.x - pupilConvergence / 2,
                initialPosition.y + clampedPosition.y + additionalVerticalOffset,
                initialPosition.z
            );

            transform.localPosition = Vector3.Lerp(transform.localPosition, firstTargetPosition, smoothSpeed);
        }
    }
}