using UnityEngine;

public class SnakeFollower : MonoBehaviour
{
    public Transform target;
    private GameConfig config;
    private float fixedY = 0.067f;

    public void Initialize(Transform target, GameConfig config)
    {
        this.target = target;
        this.config = config;

        // Set initial Y
        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        Vector3 desiredPosition = target.position - direction.normalized * config.segmentSpacing;

        // Keep fixed Y
        desiredPosition.y = fixedY;

        // Lerp only X and Z naturally
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            config.followSpeed * Time.deltaTime
        );
    }
}