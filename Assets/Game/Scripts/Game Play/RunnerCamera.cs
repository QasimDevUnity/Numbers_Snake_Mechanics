using UnityEngine;

public class RunnerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -8);

    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = false;

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;

        Vector3 currentPosition = transform.position;

        if (!followX)
            desiredPosition.x = currentPosition.x;

        if (!followY)
            desiredPosition.y = currentPosition.y;

        transform.position = Vector3.SmoothDamp(
            currentPosition,
            desiredPosition,
            ref velocity,
            1f / followSpeed
        );
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}