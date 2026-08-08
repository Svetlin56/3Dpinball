using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField]
    private Vector3 offset = new Vector3(0f, 8f, -8f);

    [SerializeField] private float smoothTime = 0.2f;

    [Header("Rotation")]
    [SerializeField] private bool lookAtTarget = true;
    [SerializeField] private float rotationSpeed = 8f;

    private Vector3 currentVelocity;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref currentVelocity,
            smoothTime);

        if (!lookAtTarget)
        {
            return;
        }

        Vector3 direction =
            target.position - transform.position;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion desiredRotation =
            Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}